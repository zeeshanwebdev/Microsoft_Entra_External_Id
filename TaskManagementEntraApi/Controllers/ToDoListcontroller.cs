using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementEntraApi.Context;
using TaskManagementEntraApi.Models;

namespace TaskManagementEntraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListcontroller : ControllerBase
    {
        private readonly TaskManagementDbcontext _toDoContext;

        public ToDoListcontroller(TaskManagementDbcontext toDoContext)
        {
            _toDoContext = toDoContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync() => Ok(await _toDoContext.TaskManagements.ToListAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id) => Ok(await _toDoContext.TaskManagements
            .FirstOrDefaultAsync(t => t.Id == id));

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TaskManagementModel toDo)
        {
            if (toDo == null)
            {
                return BadRequest("ToDo cannot be null");
            }
            await _toDoContext.TaskManagements.AddAsync(toDo);
            await _toDoContext.SaveChangesAsync();
            return Created($"/api/todo/{toDo.Id}", toDo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] TaskManagementModel toDo)
        {

            var existingToDo = await _toDoContext.TaskManagements.FindAsync(id);
            if (existingToDo == null)
                return NotFound();


            existingToDo.Title = toDo.Title;
            existingToDo.Description = toDo.Description;
            existingToDo.StartDate = toDo.StartDate;
            existingToDo.EndDate = toDo.EndDate;

            _toDoContext.TaskManagements.Update(existingToDo);
            await _toDoContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var toDo = await _toDoContext.TaskManagements.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            _toDoContext.TaskManagements.Remove(toDo);
            await _toDoContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
