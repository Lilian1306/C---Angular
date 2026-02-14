using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItems>> GetAllAsync() 
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<TaskItems?> GetByIdAsync(int id) 
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task AddAsync(TaskItems task) 
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItems task) 
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) 
    {
        var task = await GetByIdAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}