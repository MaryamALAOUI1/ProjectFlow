import { Component, Input } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Project } from '../../services/api';

import { CardModule } from 'primeng/card';
import { ChipModule } from 'primeng/chip';

@Component({
  selector: 'app-project-card',
  standalone: true,
  imports: [CommonModule, DatePipe, CardModule, ChipModule],
  templateUrl: './project-card.html',
  styleUrl: './project-card.css'
})
export class ProjectCardComponent {
  @Input() project!: Project;
}