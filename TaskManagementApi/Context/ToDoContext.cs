using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Context
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
    }
}
