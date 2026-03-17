using backend.Data;
using backend.Models;
using ExpertsEncryption.Sdk.Services;
using ExpertsEncryption.Sdk.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskTodo : ITaskTodo
{
    private readonly AppDbContext _context;
    private readonly IPiiEncryptionService _encryptionService;

    // Constructor con Inyección de Dependencias
    public TaskTodo(AppDbContext context, IPiiEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    // Helper para mantener el contexto de seguridad de ExpertS consistente
    private EncryptionContext GetContext(string fieldName) => new EncryptionContext 
    { 
        System = "ExpertS", 
        Environment = "dev", 
        Field = fieldName 
    };

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync() 
    {
        var tasks = await _context.Tasks.ToListAsync();

        // Desciframos los títulos antes de enviarlos a Angular
        foreach (var task in tasks)
        {
            if (_encryptionService.IsEncrypted(task.Title))
            {
                task.Title = await _encryptionService.DecryptAsync(task.Title, GetContext("TaskTitle"));
            }
        }
        return tasks;
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id) 
    {
        var task = await _context.Tasks.FindAsync(id);
        
        if (task != null && _encryptionService.IsEncrypted(task.Title))
        {
            task.Title = await _encryptionService.DecryptAsync(task.Title, GetContext("TaskTitle"));
        }
        
        return task;
    }

    public async Task AddTaskAsync(TaskItem task) 
    {
        // CIFRAR: Protegemos el título antes de que toque la base de datos
        if (!_encryptionService.IsEncrypted(task.Title))
        {
            task.Title = await _encryptionService.EncryptAsync(task.Title, GetContext("TaskTitle"));
        }

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskItem?> UpdateTaskCompletionAsync(TaskItem task) 
    {
        var existingTask = await _context.Tasks.FindAsync(task.Id);
        if (existingTask == null) return null;

        // Si el título cambió, lo ciframos de nuevo
        if (!_encryptionService.IsEncrypted(task.Title))
        {
            existingTask.Title = await _encryptionService.EncryptAsync(task.Title, GetContext("TaskTitle"));
        }
        else
        {
            existingTask.Title = task.Title;
        }

        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;

        await _context.SaveChangesAsync();
        
        // Devolvemos el objeto descifrado para el frontend
        existingTask.Title = await _encryptionService.DecryptAsync(existingTask.Title, GetContext("TaskTitle"));
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
        if (task == null) return false;

        task.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}