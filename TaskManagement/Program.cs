using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagement.Context;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration);

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SaveTokens = true;

    options.Events.OnRedirectToIdentityProviderForSignOut += async context =>
    {
        await NotifyToExternalAPIs(context.HttpContext);
    };
    // Post login call to extend claims and use later
    options.Events.OnTokenValidated += async context =>
    {
        // Extend claims or call service which responsible to extend claims
        // This only calls when user is login and add claims 

        await ExtenClaimsAsync(context);
    };
});
builder.Services.AddDbContext<TaskManagementContext>(options =>
{
    options.UseInMemoryDatabase("TaskManagements");
});


builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


async Task NotifyToExternalAPIs(HttpContext context)
{
    // Actual async work here
    Console.WriteLine("Performing pre-logout actions");
    await Task.Delay(1);
}

async Task ExtenClaimsAsync(TokenValidatedContext context)
{

    // we can add db logic here or call db service to fetch data and then
    // can extend claims as i did below.

    // You can now use todoService
    // Actual async work here
    if (context.Principal.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
    {
        // Add or modify claims here
        if (!identity.HasClaim(c => c.Type == "CustomClaim"))
        {
            identity.AddClaim(new Claim("CustomClaim", "new custom claim"));
        }
    }
    await Task.Delay(1);
}