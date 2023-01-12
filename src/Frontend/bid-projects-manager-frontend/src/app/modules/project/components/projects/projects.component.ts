import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectItem } from 'primeng/api';
import { BidStatus } from 'src/app/enums/bid-status';
import { ProjectSortOption } from 'src/app/enums/project-sort-option';
import { ProjectStage } from 'src/app/enums/project-stage';
import { Country } from 'src/app/models/country.model';
import { ProjectListItem } from 'src/app/models/project-list-item.model';
import { ProjectQuery } from 'src/app/queries/project-query.model';
import { CountryService } from 'src/app/services/country.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit {
  public pageSizeOptions = [10, 20, 50, 100];
  public displayedColumns: string[] = ['project-id', 'name', 'stage', 'status', 'country', 'actions'];
  public length = 0;
  public pageSize = 20;
  public pageIndex = 0;
  private sortOption : ProjectSortOption = ProjectSortOption.IdAscending;
  private selectedCountries : Array<Country> | null = [];

  public form: FormGroup;

  public showFilters: boolean = false;
  public projectStage = ProjectStage;
  public bidStatus = BidStatus;

  public projects: Array<ProjectListItem> = [];
  public countries: Array<Country> = [];
  private exportFileTitle = 'projects';

  public projectStagesOptions : SelectItem[] = [
    {label: 'Draft', value: ProjectStage.Draft},
    {label: 'Submitted', value: ProjectStage.Submitted},
    {label: 'Approved', value: ProjectStage.Rejected},
    {label: 'Approved', value: ProjectStage.Rejected}
  ];

  public bidStatusesOptions : SelectItem[] = [
    {label: 'Bid Preparation', value: BidStatus.BidPreparation},
    {label: 'Won', value: BidStatus.Won},
    {label: 'Lost', value: BidStatus.Lost},
    {label: 'No Bid', value: BidStatus.NoBid},
  ];

  constructor(
    private projectService: ProjectService,
    private countryService : CountryService,
    private spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private router: Router
    ){}
  
  
  ngOnInit(): void {
    this.spinner.show();
    this.createForm();
    let query = new ProjectQuery({
      Name : null, 
      Stages : null,
      Statuses : null,
      CountriesIds : null,
      PageNumber : this.pageIndex + 1,
      PageSize : this.pageSize,
      SortOption : this.sortOption
    })
    this.countryService.getAllCountries().then(res => this.countries = res.sort(this.compareCountries));
    this.getProjects(query);
  }

  addProject(){
    this.router.navigate(['/projects/edit', { id: 0 }]);
  }

  editProject(id: number){
    this.router.navigate(['/projects/edit', { id: id }]);
  }

  getCountryName(id : number){
    let country = this.countries.find(x => x.id == id);
    if(country)
      return country.name;
    return '';
  }
  
  handlePage(e: any) {
    this.spinner.show();
    this.pageIndex = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getProjects(this.getProjectQuery());
  }

  projectSort(e: any){
    this.spinner.show();
    const isAsc = e.direction === 'asc';
    switch(e.active){
      case 'project-id':
        this.sortOption = isAsc ? ProjectSortOption.IdAscending : ProjectSortOption.IdDescending;
        break;
      case 'name':
        this.sortOption = isAsc ? ProjectSortOption.NameAscending : ProjectSortOption.NameDescending;
        break;
      default:
        this.sortOption = ProjectSortOption.IdAscending;
        break;
    }
    this.getProjects(this.getProjectQuery());
  }

  public resetFilters(){
    this.spinner.show();
    this.pageIndex = 0;
    this.form.controls['Name'].setValue(null);
    this.form.controls['Countries'].setValue(null);
    this.form.controls['Stages'].setValue(null);
    this.form.controls['Statuses'].setValue(null);
    this.getProjects(this.getProjectQuery())
  }

  public search(){
    this.spinner.show();
    this.pageIndex = 0;
    this.getProjects(this.getProjectQuery())
  }

  private getProjects(query : ProjectQuery){
    this.projectService.getProjects(query)
    .then(res => {
      this.projects = res.items;
      this.length = res.totalPages;
      this.pageIndex = res.pageIndex - 1;
      this.spinner.hide();
    })
  }

  private getProjectQuery() : ProjectQuery
  {    
    this.selectedCountries = this.form.controls['Countries'].value;

    let query = new ProjectQuery({
      Name : this.form.controls['Name'].value, 
      Stages : this.form.controls['Stages'].value,
      Statuses : this.form.controls['Statuses'].value,
      CountriesIds : this.selectedCountries != null && this.selectedCountries.length > 0
      ? this.selectedCountries.map(x => x.id)
      : null,
      PageNumber : this.pageIndex + 1,
      PageSize : this.pageSize,
      SortOption : this.sortOption
    });
    return query;
  }

  private createForm(){
    this.form = this.formBuilder.group({
      'Countries': this.formBuilder.control([], Validators.nullValidator),
      'Stages': this.formBuilder.control(null, Validators.nullValidator),
      'Statuses': this.formBuilder.control(null, Validators.nullValidator),
      'Name': this.formBuilder.control(null, Validators.nullValidator)
    });
  }

  private compareCountries = (a: Country, b: Country) => {
    if (a.name < b.name)
      return -1;
    if (a.name > b.name)
      return 1;
    return 0;
  };

}
