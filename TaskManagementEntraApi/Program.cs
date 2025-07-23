using Microsoft.EntityFrameworkCore;
using TaskManagementEntraApi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TaskManagementDbcontext>(options =>
{
    options.UseInMemoryDatabase("TaskManagements");
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
