import { Component, Input } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Project } from '../../services/api';

@Component({
  selector: 'app-project-card',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './project-card.html',
  styleUrl: './project-card.css'
})
export class ProjectCardComponent {
  @Input() project!: Project;

  getCompletedTasksCount(): number {
    if (!this.project || !this.project.tasks) {
      return 0;
    }
    return this.project.tasks.filter(task => task.status === 'Done').length;
  }

  getTaskProgressPercentage(): number {
    if (!this.project || !this.project.tasks || this.project.tasks.length === 0) {
      return 0;
    }
    const completedCount = this.getCompletedTasksCount();
    return (completedCount / this.project.tasks.length) * 100;
  }
}