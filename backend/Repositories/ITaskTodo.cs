using backend.Models;

namespace backend.Repositories;

public interface ITaskTodo
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task AddTaskAsync(TaskItem task);
    Task<bool> UpdateTaskCompletionAsync(int id);
    Task<bool> DeleteTaskAsync(int id);
}