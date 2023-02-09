import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { toParams } from '../app-helpers';
import { CreateReportDefinitionCommand } from '../commands/create-report-definition-command.model';
import { UpdateReportDefinitionCommand } from '../commands/update-report-definition-command.model';
import { PaginatedList } from '../models/paginated-list.model';
import { ReportDefinitionListItem } from '../models/report-definition-list-item.model';
import { ReportDefinition } from '../models/report-definition.model';
import { ReportGenerationPage } from '../models/report-generation-page.model';
import { GeneratorQuery } from '../queries/generator-query.model';
import { ReportQuery } from '../queries/report-query.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private url = `${environment.apiUrl}/report`

  constructor(private http: HttpClient) { }

  getReports(query: ReportQuery){
    return lastValueFrom(this.http.get<PaginatedList<ReportDefinitionListItem>>(`${this.url}`, {params: toParams(query)})).catch(this.handleError.bind(this));
  }

  getReportById(id: number){
    return lastValueFrom(this.http.get<ReportDefinition>(`${this.url}/${id}`)).catch(this.handleError.bind(this));
  }

  getReportGeneratorPage(id : number){
    return lastValueFrom(this.http.get<ReportGenerationPage>(`${this.url}/page/${id}`)).catch(this.handleError.bind(this));
  }

  createReport(command : CreateReportDefinitionCommand){
    return lastValueFrom(this.http.post<boolean>(`${this.url}`, command)).catch(this.handleError.bind(this));
  }

  generateReport(query : GeneratorQuery){
    return lastValueFrom(this.http.post(`${this.url}/generate`, query, { responseType: 'blob' as 'json' } ))
    .catch(this.handleError.bind(this));
  }

  updateReport(command : UpdateReportDefinitionCommand){
    return lastValueFrom(this.http.put<boolean>(`${this.url}`, command)).catch(this.handleError.bind(this));
  }

  private async handleError(error: any) {
    if (error.status === 404) {
      return Promise.reject('Not found');
    }
    else if(error.status === 409){
      return Promise.reject('Country with given code already exists');
    }
    else {
      return Promise.reject('Something went wrong');
    }
  }
}
