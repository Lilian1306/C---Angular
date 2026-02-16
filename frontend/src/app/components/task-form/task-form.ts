import { Component, OnChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TaskService } from '../../services/task';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './task-form.html'
})
export class TaskForm {
  taskForm: FormGroup;

  constructor(private fb: FormBuilder, private service: TaskService, private router: Router) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: ['']
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {
      this.service.createTask(this.taskForm.value).subscribe(() => {
        this.router.navigate(['/']); 
      });
    }
  }
}