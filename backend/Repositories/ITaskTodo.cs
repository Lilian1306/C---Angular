using backend.Models;

namespace backend.Repositories;

public interface ITaskTodo
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task AddTaskAsync(TaskItem task);
    Task<TaskItem?> UpdateTaskCompletionAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(int id);

    Task<bool> MarkTaskAsCompleteAsync(int id);
}