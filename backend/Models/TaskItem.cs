using System.ComponentModel.DataAnnotations;

namespace backend.Models
{ 
    public class TaskItem
    {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } 

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsCompleted { get; set; }

    public TaskItem()
    {
        IsCompleted = false;
    }
    }
}
