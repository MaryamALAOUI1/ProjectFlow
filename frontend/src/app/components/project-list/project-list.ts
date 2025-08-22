import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, Project, Task } from '../../services/api';
import { ProjectCardComponent } from '../project-card/project-card';

import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ChipModule } from 'primeng/chip';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ProjectCardComponent,
    ButtonModule, DialogModule, InputTextModule, ProgressSpinnerModule,
    ToastModule, ConfirmDialogModule, ChipModule, IconFieldModule, InputIconModule
  ],
  templateUrl: './project-list.html',
  styleUrl: './project-list.css',
  providers: [MessageService, ConfirmationService]
})
export class ProjectListComponent implements OnInit {
  
  private allProjects: Project[] = [];
  public filteredProjects: Project[] = [];
  public isLoading = true;
  public searchText = '';
  
  displayAddProjectDialog = false;
  newProjectName = '';
  isSubmittingProject = false;
  projectErrorMessage = '';

  displayProjectDetailsDialog = false;
  selectedProject: Project | null = null;
  newTaskTitle = '';
  isSubmittingTask = false;
  
  private statusCycle = ['ToDo', 'InProgress', 'InReview', 'Done'];

  constructor(
    private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.apiService.getProjects().subscribe(data => {
      this.allProjects = data;
      this.filteredProjects = this.allProjects;
      this.isLoading = false;
    });
  }

  onSearchChange(): void {
    const searchTerm = this.searchText.toLowerCase().trim();
    if (!searchTerm) {
      this.filteredProjects = this.allProjects;
    } else {
      this.filteredProjects = this.allProjects.filter(project =>
        project.name.toLowerCase().includes(searchTerm)
      );
    }
  }
  
  showAddProjectDialog(): void {
    this.newProjectName = '';
    this.projectErrorMessage = '';
    this.isSubmittingProject = false;
    this.displayAddProjectDialog = true;
  }

  onAddProjectSubmit(): void {
    if (!this.newProjectName.trim()) return;
    this.isSubmittingProject = true;
    this.projectErrorMessage = '';
    this.apiService.createProject(this.newProjectName).subscribe({
      next: (newProject) => {
        this.allProjects.push(newProject);
        this.onSearchChange();
        this.displayAddProjectDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Project created!' });
        this.isSubmittingProject = false;
      },
      error: (err) => {
        this.projectErrorMessage = err.error?.errors?.Name?.[0] || 'An error occurred.';
        this.isSubmittingProject = false;
      }
    });
  }

  showProjectDetails(project: Project): void {
    this.selectedProject = project;
    this.newTaskTitle = '';
    this.isSubmittingTask = false;
    this.displayProjectDetailsDialog = true;
  }
  
  onDeleteProject(project: Project): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${project.name}"? This will also delete all of its tasks.`,
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.deleteProject(project.id).subscribe({
          next: () => {
            this.allProjects = this.allProjects.filter(p => p.id !== project.id);
            this.onSearchChange();
            this.displayProjectDetailsDialog = false;
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Project deleted.' });
          },
          error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not delete project.' })
        });
      }
    });
  }

  onUpdateTaskStatus(task: Task): void {
    const currentIndex = this.statusCycle.indexOf(task.status);
    const nextIndex = (currentIndex + 1) % this.statusCycle.length;
    const newStatus = this.statusCycle[nextIndex];
    const oldStatus = task.status;
    
    task.status = newStatus;

    this.apiService.updateTask(task).subscribe({
      error: () => {
        task.status = oldStatus;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not update task status.' });
      }
    });
  }

  onAddTaskSubmit(): void {
    if (!this.newTaskTitle.trim() || !this.selectedProject) return;

    this.isSubmittingTask = true;
    this.apiService.createTask(this.newTaskTitle, this.selectedProject.id).subscribe({
      next: (newTask) => {
        this.selectedProject?.tasks.push(newTask);
        this.newTaskTitle = '';
        this.isSubmittingTask = false;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not add task.' });
        this.isSubmittingTask = false;
      }
    });
  }
}