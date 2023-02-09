import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationService } from 'primeng/api';
import { ReportSortOption } from 'src/app/enums/report-sort-option';
import { ReportDefinitionListItem } from 'src/app/models/report-definition-list-item.model';
import { ReportQuery } from 'src/app/queries/report-query.model';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  public titleFilter: string | null = null;
  public groupFilter: string | null = null;

  public pageSizeOptions = [10, 20, 50];
  public displayedColumns: string[] = ['title', 'group', 'version', 'actions'];
  public displayedColumnsForAdmin: string[] = ['title', 'group', 'version', 'active', 'actions'];
  public length = 0;
  public pageSize = 20;
  public pageIndex = 0;
  public showFilters : boolean = false;
  private sortOption : ReportSortOption = ReportSortOption.TitleAsc;

  public reports: Array<ReportDefinitionListItem> = [];

  constructor(
    private reportService: ReportService,
    private spinner: NgxSpinnerService,
    private toastr : ToastrService,
    private confirmationService: ConfirmationService,
    private keycloakService: KeycloakService,
    private router: Router
  ){}
  ngOnInit(): void {
    this.spinner.show();
    this.getReports(this.getReportQuery());
  }

  public get isAdmin(){
    return this.keycloakService.isUserInRole("Administrator");
  }

  addReport(){
    this.router.navigate(['/reports/edit', { id: 0 }]);
  }

  editReport(id: number){
    this.router.navigate(['/reports/edit', { id: id }]);
  }

  goToGenerator(id: number){
    this.router.navigate(['/reports/generate', { id: id }]);
  }

  getReports(query: ReportQuery){
    this.reportService.getReports(query)
    .then(res => {
      this.reports = res.items;
      this.length = res.totalCount;
      this.pageIndex = res.pageIndex - 1;
      this.spinner.hide();
    })
    .catch(error => {
      this.toastr.error(error);
      this.spinner.hide();
    });
  }

  public resetFilters(){
    this.spinner.show();
    this.pageIndex = 0;
    this.titleFilter = null;
    this.groupFilter = null;
    this.getReports(this.getReportQuery())
  }

  public search(){
    this.spinner.show();
    this.pageIndex = 0;
    this.getReports(this.getReportQuery())
  }

  reportSort(e: any){
    this.spinner.show();
    const isAsc = e.direction === 'asc';
    switch(e.active){
      case 'title':
        this.sortOption = isAsc ? ReportSortOption.TitleAsc : ReportSortOption.TitleDesc;
        break;
      case 'group':
        this.sortOption = isAsc ? ReportSortOption.GroupAsc : ReportSortOption.GroupDesc;
        break;
      default:
        this.sortOption = ReportSortOption.TitleAsc;
        break;
    }
    this.getReports(this.getReportQuery());
  }

  handlePage(e: any) {
    this.spinner.show();
    this.pageIndex = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getReports(this.getReportQuery());
  }  

  private getReportQuery() : ReportQuery{
    return new ReportQuery({
      Title: this.titleFilter,
      Group: this.groupFilter,
      PageSize: this.pageSize,
      PageNumber: this.pageIndex + 1,
      SortOption : this.sortOption
    });
  }

  public get columns(){
    return this.isAdmin ? this.displayedColumnsForAdmin : this.displayedColumns;
  }
}
