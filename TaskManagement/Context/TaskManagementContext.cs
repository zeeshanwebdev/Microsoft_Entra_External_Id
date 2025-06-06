using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Context
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options): base(options)
        {
        }
        public DbSet<TaskManagementModel> TaskManagements { get; set; }
    }
}
