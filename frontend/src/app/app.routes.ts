import { Routes } from '@angular/router';
import { TaskForm } from './components/task-form/task-form';
import { TaskList } from './components/task-list/task-list';

export const routes: Routes = [   
    {path: "", component: TaskForm},
    {path: "add-task", component: TaskList}                
]