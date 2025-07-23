using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ToDoContext>(options =>
{
    options.UseInMemoryDatabase("ToDos");
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
