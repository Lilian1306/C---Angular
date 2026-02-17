using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskTodo : ITaskTodo
{
    private readonly AppDbContext _context;

    public TaskTodo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync() 
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id) 
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task AddTaskAsync(TaskItem task) 
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskItem?> UpdateTaskCompletionAsync(TaskItem task) 
    {
        var existingTask = await _context.Tasks.FindAsync(task.Id);
        if (existingTask == null)
        {
            return null;
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(int id) 
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;
        
         _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> MarkTaskAsCompleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return false;
        }

        task.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}