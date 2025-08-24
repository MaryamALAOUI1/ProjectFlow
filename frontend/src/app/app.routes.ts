import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main/main';
import { ProjectListComponent } from './components/project-list/project-list';
import { ProjectDetailComponent } from './components/project-detail/project-detail';
export const routes: Routes = [
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            { path: '', redirectTo: 'projects', pathMatch: 'full' },
            { path: 'projects', component: ProjectListComponent },
             { path: 'projects/:id', component: ProjectDetailComponent }
        ]
    }
];