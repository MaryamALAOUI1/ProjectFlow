import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UiStateService {
  private addProjectDialogVisible = new BehaviorSubject<boolean>(false);
  public addProjectDialogVisible$ = this.addProjectDialogVisible.asObservable();

  private searchText = new BehaviorSubject<string>('');
  public searchText$ = this.searchText.asObservable();

  constructor() { }

  public showAddProjectDialog(): void {
    this.addProjectDialogVisible.next(true);
  }

  public hideAddProjectDialog(): void {
    this.addProjectDialogVisible.next(false);
  }

  public setSearchText(text: string): void {
    this.searchText.next(text);
  }
}