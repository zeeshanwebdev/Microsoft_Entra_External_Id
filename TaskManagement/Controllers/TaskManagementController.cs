using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Context;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class TaskManagementController : Controller
    {
        private readonly TaskManagementContext _context;
        public TaskManagementController(TaskManagementContext taskManagementContext)
        {
            _context = taskManagementContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaskManagements.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskManagementModel model)
        {

            if (ModelState.IsValid) { 
                await _context.TaskManagements.AddAsync(model);
                await _context.SaveChangesAsync();
            }  
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id) => View(await _context.TaskManagements.FirstOrDefaultAsync(t => t.Id == id));
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskManagementModel task)
        {
            var dbtask = await _context.TaskManagements.FirstOrDefaultAsync(t => t.Id == task.Id);

            dbtask.Title = task.Title;
            dbtask.Description = task.Title;
            dbtask.StartDate = task.StartDate;
            dbtask.EndDate = task.EndDate;

            await _context.SaveChangesAsync();
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var dbtask = await _context.TaskManagements.FirstOrDefaultAsync(t => t.Id == id);
            return View(dbtask);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbtask = await _context.TaskManagements.FirstOrDefaultAsync(t => t.Id == id);
            _context.TaskManagements.Remove(dbtask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
