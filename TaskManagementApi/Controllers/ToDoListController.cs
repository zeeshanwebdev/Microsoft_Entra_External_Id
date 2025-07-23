using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Context;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoContext _toDoContext;

        public ToDoListController(ToDoContext toDoContext)
        {
            _toDoContext = toDoContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync() => Ok(await _toDoContext.ToDos.ToListAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id) => Ok(await _toDoContext.ToDos
            .FirstOrDefaultAsync(t => t.Id == id));

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ToDo toDo)
        {
            if (toDo == null)
            {
                return BadRequest("ToDo cannot be null");
            }
            await _toDoContext.ToDos.AddAsync(toDo);
            await _toDoContext.SaveChangesAsync();
            return Created($"/api/todo/{toDo.Id}", toDo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ToDo toDo)
        {

            var existingToDo = await _toDoContext.ToDos.FindAsync(id);
            if (existingToDo == null)
                return NotFound();
            
            
            existingToDo.Title = toDo.Title;
            existingToDo.Description = toDo.Description;
            existingToDo.StartDate = toDo.StartDate;
            existingToDo.EndDate = toDo.EndDate;

            _toDoContext.ToDos.Update(existingToDo);
            await _toDoContext.SaveChangesAsync();
            
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var toDo = await _toDoContext.ToDos.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            _toDoContext.ToDos.Remove(toDo);
            await _toDoContext.SaveChangesAsync();
            return NoContent();
        }
    }

}