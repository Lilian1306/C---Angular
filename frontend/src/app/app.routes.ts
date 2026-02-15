import { Routes } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { TaskForm } from './components/task-form/task-form';

export const routes: Routes = [   
    {path: "", component: TaskForm},
    {path: "add-task", component: TaskList},
    {path: '', redirectTo: '/tasks', pathMatch: 'full' }              
]