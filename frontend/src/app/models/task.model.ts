export interface Task {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  createdDate: string;
}

export interface CreateTaskTD {
  title: string;
  description?: string;
}

export interface UpdateTaskTD {
  title: string;
  description?: string;
  isCompleted: boolean;
}