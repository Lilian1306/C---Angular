using Microsoft.AspNetCore.Mvc;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskTodo _taskTodo;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskTodo taskRepository, ILogger<TasksController> logger)
    {
        _taskTodo = taskRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        // El repositorio devolverá los títulos ya descifrados para Angular
        var tasks = await _taskTodo.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _taskTodo.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> AddTask(TaskItem task)
    {
        if(string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Title is required");
        }

        // El repositorio se encargará de cifrar el título antes de guardar
        await _taskTodo.AddTaskAsync(task);
        
        // Importante: devolvemos el objeto tal cual, pero el cliente (Angular) 
        // recibirá el objeto con el ID generado.
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> UpdateTask(int id, TaskItem task)
    {
        if (id != task.Id) return BadRequest("Task ID mismatch");
        if (string.IsNullOrWhiteSpace(task.Title)) return BadRequest("Title is required");

        try
        {
            // El repositorio detecta si el título viene en plano para cifrarlo
            // o si ya está cifrado para mantenerlo así en la BD.
            var updatedTask = await _taskTodo.UpdateTaskCompletionAsync(task);

            if (updatedTask == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            return Ok(updatedTask); // Devolvemos la tarea descifrada
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {TaskId}", id);
            return StatusCode(500, "An error occurred while updating the task");
        }
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> MarkTaskAsComplete(int id)
    {
        try
        {
            var success = await _taskTodo.MarkTaskAsCompleteAsync(id);
            if (!success) return NotFound(new { message = $"Task with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking task {TaskId} as complete", id);
            return StatusCode(500, "Error updating task status");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        bool success = await _taskTodo.DeleteTaskAsync(id);
        if(!success) return NotFound();

        return NoContent();
    }
}