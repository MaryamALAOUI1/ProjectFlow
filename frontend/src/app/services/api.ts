import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

const API_BASE_URL = 'https://localhost:7059';

export interface Project {
  id: number;
  name: string;
  creationDate: string;
  dueDate: string | null; 
  tasks: Task[];
}

export interface Task {
  id: number;
  title: string;
  description: string | null;
  status: string;
  dueDate: string | null;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) { }

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${API_BASE_URL}/api/Projects`);
  }

  getProjectById(id: number): Observable<Project> {
    return this.http.get<Project>(`${API_BASE_URL}/api/Projects/${id}`);
  }

  createProject(name: string, dueDate: Date | null): Observable<Project> {
    const command = {
      name: name,
      dueDate: dueDate ? dueDate.toISOString() : null
    };
    return this.http.post<Project>(`${API_BASE_URL}/api/Projects`, command);
  }

  deleteProject(id: number): Observable<any> {
    return this.http.delete(`${API_BASE_URL}/api/Projects/${id}`);
  }

  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${API_BASE_URL}/api/Tasks/${id}`);
  }

  createTask(title: string, projectId: number, dueDate: Date | null): Observable<Task> {
    const command = {
      title: title,
      projectId: projectId,
      dueDate: dueDate ? dueDate.toISOString() : null
    };
    return this.http.post<Task>(`${API_BASE_URL}/api/Tasks`, command);
  }

 updateTask(task: Task): Observable<any> {
    const command = {
      taskId: task.id,
      title: task.title,
      description: task.description,
      status: task.status,
      dueDate: task.dueDate
    };
    return this.http.put(`${API_BASE_URL}/api/Tasks/${task.id}`, command);
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${API_BASE_URL}/api/Tasks/${id}`);
  }
}