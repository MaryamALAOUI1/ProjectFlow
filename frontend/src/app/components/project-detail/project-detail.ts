import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiService, Project, Task } from '../../services/api';
import { FormsModule } from '@angular/forms';
import { MessageService, ConfirmationService } from 'primeng/api';

import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DatePickerModule } from 'primeng/datepicker';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    ButtonModule,
    ProgressSpinnerModule,
    DialogModule,
    InputTextModule,
    ToastModule,
    ConfirmDialogModule,
    DatePickerModule
  ],
  templateUrl: './project-detail.html',
  styleUrl: './project-detail.css',
  providers: [MessageService]
})
export class ProjectDetailComponent implements OnInit {
  project: Project | null = null;
  isLoading = true;
  error: string | null = null;

  toDoTasks: Task[] = [];
  inProgressTasks: Task[] = [];
  doneTasks: Task[] = [];

  displayAddTaskDialog = false;
  newTaskTitle = '';
  newTaskDueDate: Date | null = null;
  isSubmittingTask = false;

  private statusCycle = ['ToDo', 'InProgress', 'Done'];

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const projectId = this.route.snapshot.paramMap.get('id');
    if (projectId) {
      this.apiService.getProjectById(+projectId).subscribe({
        next: (data) => {
          this.project = data;
          this.categorizeTasks();
          this.isLoading = false;
        },
        error: (err) => {
          this.error = "Could not load the project. It may not exist.";
          this.isLoading = false;
        }
      });
    } else {
        this.error = "No project ID provided.";
        this.isLoading = false;
    }
  }

  private categorizeTasks(): void {
    if (!this.project?.tasks) return;
    this.toDoTasks = this.project.tasks.filter(t => t.status === 'ToDo');
    this.inProgressTasks = this.project.tasks.filter(t => t.status === 'InProgress');
    this.doneTasks = this.project.tasks.filter(t => t.status === 'Done');
  }

  cycleTaskStatus(task: Task): void {
    const oldStatus = task.status;
    const currentIndex = this.statusCycle.indexOf(oldStatus);
    
    if (currentIndex === -1 || currentIndex === this.statusCycle.length - 1) {
      return;
    }

    const nextIndex = currentIndex + 1;
    const newStatus = this.statusCycle[nextIndex];

    task.status = newStatus;
    this.categorizeTasks();

    this.apiService.updateTask(task).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: `Task moved to ${newStatus}` });
      },
      error: () => {
        task.status = oldStatus;
        this.categorizeTasks();
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not update task status.' });
      }
    });
  }

  showAddTaskDialog(): void {
    this.newTaskTitle = '';
    this.newTaskDueDate = null;
    this.isSubmittingTask = false;
    this.displayAddTaskDialog = true;
  }

  onAddTaskSubmit(): void {
    if (!this.newTaskTitle.trim() || !this.project) return;
    this.isSubmittingTask = true;
    this.apiService.createTask(this.newTaskTitle, this.project.id, this.newTaskDueDate).subscribe({
      next: (newTask) => {
        this.project?.tasks.push(newTask);
        this.categorizeTasks();
        this.displayAddTaskDialog = false;
        this.isSubmittingTask = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Task added successfully!' });
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not add task.' });
        this.isSubmittingTask = false;
      }
    });
  }

  confirmDeleteProject(): void {
    if (!this.project) return;

    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${this.project.name}"? This action cannot be undone.`,
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.deleteProject(this.project!.id).subscribe({
          next: () => {
            this.router.navigate(['/projects']);
          },
          error: () => {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not delete project.' });
          }
        });
      }
    });
  }
}