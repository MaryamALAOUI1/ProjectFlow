import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, Project } from '../../services/api';
import { ProjectCardComponent } from '../project-card/project-card';
import { UiStateService } from '../../services/ui-state';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { DatePickerModule } from 'primeng/datepicker';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [
    CommonModule, FormsModule, ProjectCardComponent,
    ButtonModule, DialogModule, InputTextModule, ProgressSpinnerModule,
    ToastModule, ConfirmDialogModule, IconFieldModule, InputIconModule, DatePickerModule
  ],
  templateUrl: './project-list.html',
  styleUrl: './project-list.css'
})
export class ProjectListComponent implements OnInit, OnDestroy {
  
  private allProjects: Project[] = [];
  public filteredProjects: Project[] = [];
  public isLoading = true;
  
  displayAddProjectDialog = false;
  private dialogSubscription: Subscription | undefined;
  private searchSubscription: Subscription | undefined;
  
  newProjectName = '';
  newProjectDueDate: Date | null = null;
  isSubmittingProject = false;
  projectErrorMessage = '';

  constructor(
    private apiService: ApiService,
    private messageService: MessageService,
    private uiStateService: UiStateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.apiService.getProjects().subscribe(data => {
      this.allProjects = data;
      this.filteredProjects = this.allProjects;
      this.isLoading = false;
    });

    this.dialogSubscription = this.uiStateService.addProjectDialogVisible$.subscribe(isVisible => {
      if (isVisible) {
        this.newProjectName = '';
        this.newProjectDueDate = null;
        this.projectErrorMessage = '';
        this.isSubmittingProject = false;
      }
      this.displayAddProjectDialog = isVisible;
    });

    this.searchSubscription = this.uiStateService.searchText$.subscribe(searchTerm => {
      const lowerCaseSearchTerm = searchTerm.toLowerCase().trim();
      if (!lowerCaseSearchTerm) {
        this.filteredProjects = this.allProjects;
      } else {
        this.filteredProjects = this.allProjects.filter(project =>
          project.name.toLowerCase().includes(lowerCaseSearchTerm)
        );
      }
    });
  }

  ngOnDestroy(): void {
    this.dialogSubscription?.unsubscribe();
    this.searchSubscription?.unsubscribe();
  }
  
  showAddProjectDialog(): void {
    this.uiStateService.showAddProjectDialog();
  }

  onAddProjectSubmit(): void {
    if (!this.newProjectName.trim()) return;
    this.isSubmittingProject = true;
    this.projectErrorMessage = '';
    this.apiService.createProject(this.newProjectName, this.newProjectDueDate).subscribe({
      next: (newProject) => {
        this.allProjects.push(newProject);
        this.uiStateService.setSearchText('');
        this.uiStateService.hideAddProjectDialog();
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Project created!' });
        this.isSubmittingProject = false;
      },
      error: (err) => {
        this.projectErrorMessage = err.error?.errors?.Name?.[0] || 'An error occurred.';
        this.isSubmittingProject = false;
      }
    });
  }

  onDialogHide(): void {
    this.uiStateService.hideAddProjectDialog();
  }

  navigateToProject(project: Project): void {
    this.router.navigate(['/projects', project.id]);
  }
}