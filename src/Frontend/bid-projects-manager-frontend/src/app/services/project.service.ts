import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { toParams } from '../app-helpers';
import { CreateProjectDraftCommand } from '../commands/create-project-draft-command.model';
import { CreateSubmittedProjectCommand } from '../commands/create-submitted-project-command.model';
import { SaveProjectCommand } from '../commands/save-project-command.model';
import { SubmitProjectCommand } from '../commands/submit-project-command.model';
import { UpdateProjectDraftCommand } from '../commands/update-project-draft-command.model';
import { PaginatedList } from '../models/paginated-list.model';
import { ProjectDetails } from '../models/project-details.model';
import { ProjectListItem } from '../models/project-list-item.model';
import { ProjectExportQuery } from '../queries/project-export-query.model';
import { ProjectQuery } from '../queries/project-query.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private url = `${environment.apiUrl}/project`

  constructor(private http: HttpClient) { }

  getProjects(query : ProjectQuery): Promise<PaginatedList<ProjectListItem>> {
    return lastValueFrom(this.http.get<PaginatedList<ProjectListItem>>(this.url, {params: toParams(query)}))
    .catch(this.handleError.bind(this));
  }

  getProject(id : number) : Promise<ProjectDetails>{
    return lastValueFrom(this.http.get<ProjectDetails>(`${this.url}/${id}`))
    .catch(this.handleError.bind(this));
  }

  createDraftProject(model : CreateProjectDraftCommand){
    return lastValueFrom(this.http.post<boolean>(`${this.url}/draft`, model))
    .catch(this.handleError.bind(this));
  }

  createSubmittedProject(model: CreateSubmittedProjectCommand){
    return lastValueFrom(this.http.post<boolean>(`${this.url}/submit`, model))
    .catch(this.handleError.bind(this));
  }

  updateDraftProject(model: UpdateProjectDraftCommand){
    return lastValueFrom(this.http.put<boolean>(`${this.url}/draft`, model))
    .catch(this.handleError.bind(this));
  }

  submitProject(model: SubmitProjectCommand){
    return lastValueFrom(this.http.put<boolean>(`${this.url}/submit`, model))
    .catch(this.handleError.bind(this));
  }

  saveProject(model: SaveProjectCommand){
    return lastValueFrom(this.http.put<boolean>(`${this.url}/save`, model))
    .catch(this.handleError.bind(this));
  }

  deleteProject(id: number){
    return lastValueFrom(this.http.delete(`${this.url}/${id}`))
    .catch(this.handleError.bind(this));
  }

  approveProject(id: number){
    return lastValueFrom(this.http.patch<boolean>(`${this.url}/approve/${id}`,null))
    .catch(this.handleError.bind(this));
  }

  rejectProject(id: number){
    return lastValueFrom(this.http.patch<boolean>(`${this.url}/reject/${id}`,null))
    .catch(this.handleError.bind(this));
  }

  rollbackProject(id: number){
    return lastValueFrom(this.http.patch<boolean>(`${this.url}/rollback/${id}`,null))
    .catch(this.handleError.bind(this));
  }

  exportProjects(query: ProjectExportQuery){
    return lastValueFrom(this.http.get(`${this.url}/export`, { params: toParams(query), responseType: 'blob' as 'json' } ))
    .catch(this.handleError.bind(this));
  }

  exportProject(id: number){
    return lastValueFrom(this.http.get(`${this.url}/export/${id}`, { responseType: 'blob' as 'json' } ))
    .catch(this.handleError.bind(this));
  }

  private async handleError(error: any) {
    if (error.status === 404) {
      return Promise.reject('Not found');
    }
    else {
      return Promise.reject('Something went wrong');
    }
  }
}
