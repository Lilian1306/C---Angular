import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';
import { TaskFormComponent } from '../task-form/task-form.component';
import { LucideAngularModule, SquarePen, Trash2} from 'lucide-angular';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, TaskFormComponent, LucideAngularModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  loading: boolean = true;
  editingTask: Task | null = null;

  readonly SquarePen = SquarePen;
  readonly Trash2 = Trash2;

  constructor(
    private taskService: TaskService, 
    private cdr: ChangeDetectorRef, 
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.loadTasks();
    })
  }

  loadTasks(): void {
    this.loading = true;

    this.taskService.getTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Full error object:', err);
        this.toastr.error('Failed to load tasks. Please make sure the backend is running.', 'Error');
        this.loading = false;
        this.cdr.markForCheck();
        console.error('Error loading tasks:', err);
      },
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  toggleTaskCompletion(task: Task): void {
    const newCompletedStatus = !task.isCompleted;
    
    const updatedTask: Task = {
      id: task.id,
      title: task.title,
      description: task.description || '',
      isCompleted: newCompletedStatus,
      createdDate: task.createdDate
    };

    this.taskService.updateTask(task.id, updatedTask).subscribe({
      next: (returnedTask) => {
        task.isCompleted = returnedTask.isCompleted;
        this.cdr.markForCheck();
        this.toastr.success(`Task "${task.title}" marked as ${newCompletedStatus ? 'completed' : 'NOT COMPLETED'}`);
      },
      error: (err) => {
        console.error('Error updating task completion:', err);
        this.cdr.markForCheck();
        alert('Failed to update task status. Please try again.');
      }
    });
  }

  // Delete a task
  deleteTask(task: Task): void {
    if (confirm(`Are you sure you want to delete "${task.title}"?`)) {
      this.taskService.deleteTask(task.id).subscribe({

        next: () => {
          this.loadTasks(); 
          this.toastr.success(`"${task.title}" deleted successfully!`, 'Task Deleted'); 
          
        },
        error: (err) => {
          console.error('Error deleting task:', err);
          alert('Failed to delete task. Please try again.');
        }
      });
    }
  }

  // Start editing a task
  editTask(task: Task): void {
    this.editingTask = { ...task }; 
  }

  // Cancel editing
  cancelEdit(): void {
    this.editingTask = null;
  }

  // Handle task updated
  onTaskUpdated(): void {
    this.editingTask = null;
    this.loadTasks();
  }
}