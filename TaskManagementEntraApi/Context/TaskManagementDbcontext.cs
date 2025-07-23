using Microsoft.EntityFrameworkCore;
using TaskManagementEntraApi.Models;

namespace TaskManagementEntraApi.Context
{
    public class TaskManagementDbcontext : DbContext
    {
        public TaskManagementDbcontext(DbContextOptions<TaskManagementDbcontext> options) : base(options)
        {
        }
        public DbSet<TaskManagementModel> TaskManagements { get; set; }
    }
}
