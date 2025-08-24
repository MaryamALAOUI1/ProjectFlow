import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { UiStateService } from '../../services/ui-state';
import { FormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    FormsModule,
    ButtonModule,
    IconFieldModule,
    InputIconModule,
    InputTextModule
  ],
  templateUrl: './main.html',
  styleUrls: ['./main.css']
})
export class MainLayoutComponent {
  searchText = '';

  constructor(private uiStateService: UiStateService) {}

  openAddProjectDialog(): void {
    this.uiStateService.showAddProjectDialog();
  }

  onSearchChange(): void {
    this.uiStateService.setSearchText(this.searchText);
  }
}