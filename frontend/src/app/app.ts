import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ProjectListComponent } from './components/project-list/project-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ProjectListComponent],
  template: `
    <div class="app-container">
      <main>
        <app-project-list></app-project-list>
      </main>
      <router-outlet></router-outlet>
    </div>
  `,
  styleUrl: './app.css'
})
export class AppComponent {
}