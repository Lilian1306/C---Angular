
using Microsoft.EntityFrameworkCore; 
using backend;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<TaskItems> Tasks { get; set; }
}