import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { toParams } from '../app-helpers';
import { CreateProjectDraftCommand } from '../commands/create-project-draft-command.model';
import { UpdateProjectDraftCommand } from '../commands/update-project-draft-command.model';
import { PaginatedList } from '../models/paginated-list.model';
import { ProjectDetails } from '../models/project-details.model';
import { ProjectListItem } from '../models/project-list-item.model';
import { ProjectQuery } from '../queries/project-query.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private url = `${environment.apiUrl}/project`

  constructor(private http: HttpClient) { }

  getProjects(query : ProjectQuery): Promise<PaginatedList<ProjectListItem>> {
    return lastValueFrom(this.http.get<PaginatedList<ProjectListItem>>(this.url, {params: toParams(query)}))
    .catch(this.handleError.bind(this));;
  }

  getProject(id : number) : Promise<ProjectDetails>{
    return lastValueFrom(this.http.get<ProjectDetails>(`${this.url}/${id}`))
    .catch(this.handleError.bind(this));;
  }

  createDraftProject(model : CreateProjectDraftCommand){
    return lastValueFrom(this.http.post(`${this.url}/draft`, model))
    .catch(this.handleError.bind(this));;
  }

  updateDraftProject(model: UpdateProjectDraftCommand){
    return lastValueFrom(this.http.put(`${this.url}/draft`, model))
    .catch(this.handleError.bind(this));;
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
