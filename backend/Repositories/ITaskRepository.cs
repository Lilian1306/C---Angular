namespace backend.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItems>> GetAllAsync();
    Task<TaskItems?> GetByIdAsync(int id);
    Task AddAsync(TaskItems task);
    Task UpdateAsync(TaskItems task);
    Task DeleteAsync(int id);
}