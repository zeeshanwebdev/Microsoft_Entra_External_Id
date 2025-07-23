using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Context;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class TaskManagementController : Controller
    {
        private readonly string _apiUrl = "https://localhost:7088/api/";
        private readonly TaskManagementContext _context;
        private readonly HttpClient _httpClient;
        public TaskManagementController(TaskManagementContext taskManagementContext, TaskManagementContext context, HttpClient httpClient)
        {
            _context = taskManagementContext;
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index() => View(await _httpClient.
            GetFromJsonAsync<List<TaskManagementModel>>(_apiUrl + "todolist"));

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskManagementModel model) => View(await _httpClient.PostAsJsonAsync(_apiUrl + "todolist", model));

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _httpClient.GetFromJsonAsync<TaskManagementModel>(_apiUrl + "todolist/" + id);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskManagementModel task)
        {
            await _httpClient.PutAsJsonAsync<TaskManagementModel>(_apiUrl + "todolist/" + task.Id, task);
            return RedirectToAction("Index");
        }
        public IActionResult Detail()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id) => View(await _httpClient.GetFromJsonAsync<TaskManagementModel>(_apiUrl + "todolist/" + id));

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync(_apiUrl + "todolist/" + id);
            return RedirectToAction("Index");
        }
    }
}
