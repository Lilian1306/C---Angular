import { Component, type OnInit } from '@angular/core';
import { Task, TaskService } from '../../services/task';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-list',
  imports: [CommonModule],
  templateUrl: './task-list.html'
})
export class TaskList implements OnInit {
    tasks: Task[] = [];

    constructor(private taskService: TaskService) {}

    ngOnInit(): void {
      this.taskService.getTasks().subscribe((data) => {
        this.tasks = data;
      });
    }

    toggleComplete(t: Task): void {
      t.isCompleted = !t.isCompleted;
      if(t.id) {
        this.taskService.updateTask(t.id, t).subscribe();
      }
    }
}
