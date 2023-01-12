import { formatDate } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SelectItem } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { CreateProjectDraftBuilder } from 'src/app/builders/create-project-draft-builder.model';
import { UpdateProjectDraftCommandBuilder } from 'src/app/builders/update-project-draft-command-builder.model';
import { CreateCapexCommand } from 'src/app/commands/create-capex-command.model';
import { CreateEbitCommand } from 'src/app/commands/create-ebit-command.model';
import { CreateOpexCommand } from 'src/app/commands/create-opex-command.model';
import { FinancialData } from 'src/app/commands/financial-data.model';
import { UpdateCapexCommand } from 'src/app/commands/update-capex-command.model';
import { UpdateEbitCommand } from 'src/app/commands/update-ebit-command.model';
import { UpdateOpexCommand } from 'src/app/commands/update-opex-command.model';
import { BidPriority } from 'src/app/enums/bid-priority';
import { BidProbability } from 'src/app/enums/bid-probability';
import { BidStatus } from 'src/app/enums/bid-status';
import { ProjectStage } from 'src/app/enums/project-stage';
import { ProjectType } from 'src/app/enums/project-type';
import { Country } from 'src/app/models/country.model';
import { Currency } from 'src/app/models/currency.model';
import { ProjectDetails } from 'src/app/models/project-details.model';
import { CountryService } from 'src/app/services/country.service';
import { CurrencyService } from 'src/app/services/currency.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.scss']
})
export class EditProjectComponent implements OnInit {

  public isEdit : boolean = false;
  public form : FormGroup;

  public countries: Array<Country> = [];
  public currencies : Array<Currency> = [];
  public projectStage  = ProjectStage;
  public bidStatus = BidStatus;
  public years : Array<number> = [];
  public yearsFromProject : Array<number> = [];


  private projectDetails : ProjectDetails | null = null;
  public id : number;
  public selectedCountry: Country | null = null;
  public projectCurrency: Currency | null = null;

  public get stage(){
    return this.isEdit && this.projectDetails != null ? this.projectDetails.stage : null;
  }

  public projectTypes : SelectItem[] = [
    {label: 'Tender Offer', value: ProjectType.TenderOffer},
    {label: 'Acquisition', value: ProjectType.Acquisition}
  ];

  public priorities : SelectItem[] = [
    {label: 'Low', value: BidPriority.Low},
    {label: 'Medium', value: BidPriority.Medium},
    {label: 'High', value: BidPriority.High}
  ];

  public probabilities : SelectItem[] = [
    {label: 'Low', value: BidProbability.Low},
    {label: 'Medium', value: BidProbability.Medium},
    {label: 'High', value: BidProbability.High}
  ];

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
    private projectService : ProjectService,
    private countryService : CountryService,
    private currencyService : CurrencyService,
    private toastr : ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder
  ){}

  ngOnInit(): void {
    this.spinner.show();
    this.activatedRoute.params.subscribe((routeParams: Params) => {
      this.isEdit = !(routeParams['id'] == '0')
      this.id = routeParams['id'];
      this.createProjectForm();
      if(this.isEdit){
        forkJoin([
          this.countryService.getAllCountries().then(res => this.countries = res),
          this.currencyService.getAllCurrencies().then(res => this.currencies = res),
          this.projectService.getProject(this.id).then(res => this.projectDetails = res)
        ])
        .subscribe(res => {this.fillProjectFormFields(); this.spinner.hide();});
      }
      else{
        forkJoin([
          this.countryService.getAllCountries().then(res => this.countries = res),
          this.currencyService.getAllCurrencies().then(res => this.currencies = res)
        ])
        .subscribe(res => this.spinner.hide());
      }
    });
  }

  addLeadingZeros(num: number, totalLength: number): string {
    return String(num).padStart(totalLength, '0');
  }

  onCountryChange($event: any){
    let countryId = $event.value.id;
    let country = this.countries.find(x => x.id === countryId);
    let currency = this.currencies.find(x => x.id == country?.currencyId);
    this.form.controls["Currency"].setValue(currency);
  }

  onCountryClear(){
    this.form.controls["Currency"].setValue(null);
  }

  addNewEstimation(){
    let year = this.years.length > 0 ? this.years[this.years.length-1] + 1 : new Date().getFullYear();
    this.estimations.push(this.formBuilder.group({
      "Year": this.formBuilder.control(year, Validators.required),
      "CapexValue": this.formBuilder.control(0, Validators.required),
      "CapexId": this.formBuilder.control(0, Validators.nullValidator),
      "OpexValue": this.formBuilder.control(0, Validators.required),
      "OpexId": this.formBuilder.control(0, Validators.nullValidator),
      "EbitValue": this.formBuilder.control(0, Validators.required),
      "EbitId": this.formBuilder.control(0, Validators.nullValidator)
    }));
    this.years.push(year);
  }

  removeEstimation(index: number){
    this.years.splice(-1);
    this.estimations.removeAt(index);
  }

  saveDraft(){
    this.markFields(false);
    if(this.form.valid){
      this.spinner.show();
      if(!this.isEdit){
        this.createDraft();
      }
      else{
        this.updateDraft();
      }
    }
  }

  returnToProjects(){
    this.router.navigate(["/projects/"])
  }

  private createProjectForm(){
    this.form = this.formBuilder.group({
      'Name': this.formBuilder.control(null, Validators.required),
      'Country': this.formBuilder.control(null, Validators.required),
      'Currency': this.formBuilder.control(null, Validators.nullValidator),
      'Status': this.formBuilder.control(null, Validators.nullValidator),
      'Type': this.formBuilder.control(null, Validators.nullValidator),
      'Description': this.formBuilder.control(null, Validators.nullValidator),
      'NumberOfVechicles': this.formBuilder.control(null, Validators.nullValidator),
      'BidOperationStart': this.formBuilder.control<Date | null>(null, Validators.nullValidator),
      'BidEstimatedOperationEnd': this.formBuilder.control<Date | null>(null, Validators.nullValidator),
      'NoBidReason': this.formBuilder.control(null, Validators.nullValidator),
      'OptionalExtensionYears': this.formBuilder.control(null, Validators.nullValidator),
      'LifetimeInThousandsKilometers': this.formBuilder.control(null, Validators.nullValidator),
      'TotalCapex': this.formBuilder.control(0.00, Validators.nullValidator),
      'TotalOpex': this.formBuilder.control(0.00, Validators.nullValidator),
      'TotalEbit': this.formBuilder.control(0.00, Validators.nullValidator),
      'Probability': this.formBuilder.control(null, Validators.nullValidator),
      'Priority': this.formBuilder.control(null, Validators.nullValidator),
      'Estimations': this.formBuilder.array([]),
      'Ebits': this.formBuilder.array([]),
      'Capexes': this.formBuilder.array([]),
      'Opexes': this.formBuilder.array([])
    });
    if(!this.isEdit){
      let currentYear = new Date().getFullYear();
      this.years = [currentYear, currentYear+1, currentYear+2];
      for (let i = 0; i < this.years.length; i++) {
        let year = this.years[i];
        this.estimations.push(this.formBuilder.group({
          "Year": this.formBuilder.control(year, Validators.required),
          "CapexValue": this.formBuilder.control(0, Validators.required),
          "CapexId": this.formBuilder.control(0, Validators.nullValidator),
          "OpexValue": this.formBuilder.control(0, Validators.required),
          "OpexId": this.formBuilder.control(0, Validators.nullValidator),
          "EbitValue": this.formBuilder.control(0, Validators.required),
          "EbitId": this.formBuilder.control(0, Validators.nullValidator)
        }));
      }
    }
  }

  private fillProjectFormFields(){
    if(this.isEdit && this.projectDetails != null){
      this.form.controls["Name"].setValue(this.projectDetails.name);
      let country = this.countries.find(x => x.id === this.projectDetails?.countryId);
      let currency = this.currencies.find(x => x.id == country?.currencyId);
      this.form.controls["Country"].setValue(country);
      this.form.controls["Currency"].setValue(currency);
      this.form.controls["Status"].setValue(this.projectDetails.status);
      this.form.controls["Type"].setValue(this.projectDetails.type);
      this.form.controls["Description"].setValue(this.projectDetails.description);
      this.form.controls["NumberOfVechicles"].setValue(this.projectDetails.numberOfVechicles);
      this.form.controls["BidOperationStart"].setValue(this.projectDetails.bidOperationStart);
      this.form.controls["BidEstimatedOperationEnd"].setValue(this.projectDetails.bidEstimatedOperationEnd);
      this.form.controls["NoBidReason"].setValue(this.projectDetails.noBidReason);
      this.form.controls["OptionalExtensionYears"].setValue(this.projectDetails.optionalExtensionYears);
      this.form.controls["LifetimeInThousandsKilometers"].setValue(this.projectDetails.lifetimeInThousandsKilometers);
      this.form.controls["TotalCapex"].setValue(this.projectDetails.totalCapex);
      this.form.controls["TotalOpex"].setValue(this.projectDetails.totalOpex);
      this.form.controls["TotalEbit"].setValue(this.projectDetails.totalEbit);
      this.form.controls["Probability"].setValue(this.projectDetails.probability);
      this.form.controls["Priority"].setValue(this.projectDetails.priority);

      this.years = this.projectDetails.capexes.map(x => x.year);
      this.yearsFromProject = this.projectDetails.capexes.map(x => x.year);
      for (let i = 0; i < this.years.length; i++) {
        let year = this.years[i];
        let capex = this.projectDetails.capexes.length > i ? this.projectDetails.capexes[i] : null;
        let opex = this.projectDetails.opexes.length > i ? this.projectDetails.opexes[i] : null;
        let ebit = this.projectDetails.ebits.length > i ? this.projectDetails.ebits[i] : null;
        this.estimations.push(this.formBuilder.group({
          "Year": this.formBuilder.control(year, Validators.required),
          "CapexValue": this.formBuilder.control(capex != null ? capex.value : 0, Validators.required),
          "CapexId": this.formBuilder.control(capex != null ? capex.id : 0, Validators.nullValidator),
          "OpexValue": this.formBuilder.control(opex != null ? opex.value : 0, Validators.required),
          "OpexId": this.formBuilder.control(opex != null ? opex.id : 0, Validators.nullValidator),
          "EbitValue": this.formBuilder.control(ebit != null ? ebit.value : 0, Validators.required),
          "EbitId": this.formBuilder.control(ebit != null ? ebit.id : 0, Validators.nullValidator)
        }));
      }

      if(this.stage === ProjectStage.Approved){
        this.form.addControl("ApprovalDate", this.formBuilder.control(this.projectDetails.approvalDate, Validators.nullValidator));
      }
    }
  }

  private markFields(isSubmit : boolean){
    Object.keys(this.form.controls).forEach((key) => {
      this.form.controls[key].markAsDirty();
      if(isSubmit && this.form.controls[key].value == null && key != 'ApprovalDate'){
        this.form.controls[key].setErrors({'incorrect': true});
      }
    });
  }
  

  get estimations() {
    return this.form.controls["Estimations"] as FormArray;
  }

  get status() {
    return this.form.controls["Status"].value;
  }

  private createDraft(){
    let builder = new CreateProjectDraftBuilder();

    let financialData = this.createFinancialData();

    let command = builder
    .name(this.form.controls['Name'].value)
    .description(this.form.controls['Description'].value)
    .bidOperationStart(this.form.controls['BidOperationStart'].value)
    .bidEstimatedOperationEnd(this.form.controls['BidEstimatedOperationEnd'].value)
    .bidProbability(this.form.controls['Probability'].value)
    .priority(this.form.controls['Priority'].value)
    .countryId(this.form.controls['Country'].value.id)
    .currencyId(this.form.controls['Currency'].value.id)
    .type(this.form.controls['Type'].value)
    .status(this.form.controls['Status'].value)
    .lifetimeInThousandsKilometers(this.form.controls['LifetimeInThousandsKilometers'].value)
    .numberOfVechicles(this.form.controls['NumberOfVechicles'].value)
    .optionalExtensionYears(this.form.controls['OptionalExtensionYears'].value)
    .totalCapex(this.form.controls['TotalCapex'].value)
    .totalOpex(this.form.controls['TotalOpex'].value)
    .totalEbit(this.form.controls['TotalEbit'].value)
    .capexes(financialData.NewCapexes)
    .opexes(financialData.NewOpexes)
    .ebits(financialData.NewEbits)
    .noBidReason(this.form.controls['NoBidReason'].value)
    .build();

    this.projectService.createDraftProject(command).then(res => {
      this.toastr.success('Project created');
      this.spinner.hide();
      this.router.navigate(['/projects/']);
    })
    .catch(error => {
      this.toastr.error(error);
      this.spinner.hide();
    });
  }

  private updateDraft(){
    let builder = new UpdateProjectDraftCommandBuilder();

    var startDate : Date = new Date(this.form.controls['BidOperationStart'].value);
    var endDate : Date = new Date(this.form.controls['BidEstimatedOperationEnd'].value);

    let financialData = this.createFinancialData();

    let command = builder
    .id(this.id)
    .name(this.form.controls['Name'].value)
    .description(this.form.controls['Description'].value)
    .bidOperationStart(new Date(Date.UTC(startDate.getFullYear(), startDate.getMonth(), startDate.getDate())).toISOString())
    .bidEstimatedOperationEnd(new Date(Date.UTC(endDate.getFullYear(), endDate.getMonth(), endDate.getDate())).toISOString())
    .bidProbability(this.form.controls['Probability'].value)
    .priority(this.form.controls['Priority'].value)
    .countryId(this.form.controls['Country'].value.id)
    .currencyId(this.form.controls['Currency'].value.id)
    .type(this.form.controls['Type'].value)
    .status(this.form.controls['Status'].value)
    .lifetimeInThousandsKilometers(this.form.controls['LifetimeInThousandsKilometers'].value)
    .numberOfVechicles(this.form.controls['NumberOfVechicles'].value)
    .optionalExtensionYears(this.form.controls['OptionalExtensionYears'].value)
    .totalCapex(this.form.controls['TotalCapex'].value)
    .totalOpex(this.form.controls['TotalOpex'].value)
    .totalEbit(this.form.controls['TotalEbit'].value)
    .newCapexes(financialData.NewCapexes)
    .newOpexes(financialData.NewOpexes)
    .newEbits(financialData.NewEbits)
    .opexes(financialData.Opexes)
    .capexes(financialData.Capexes)
    .ebits(financialData.Ebits)
    .noBidReason(this.form.controls['NoBidReason'].value)
    .yearsToRemove(financialData.YearsToRemove)
    .build();

    this.projectService.updateDraftProject(command).then(res => {
      this.toastr.success('Project created');
      this.spinner.hide();
      this.router.navigate(['/projects/']);
    })
    .catch(error => {
      this.toastr.error(error);
      this.spinner.hide();
    });
  }

  private getYearsToRemove(){
    return this.yearsFromProject.filter(x => this.years.indexOf(x)===-1);
  }

  private createFinancialData(){
    let data = new FinancialData();
    data.YearsToRemove = this.getYearsToRemove();

    this.estimations.controls.forEach(element => {
      let group = element as FormGroup;
      if(group.controls['CapexId'].value == 0){
        data.NewCapexes.push(new CreateCapexCommand({Year : group.controls['Year'].value, Value: group.controls['CapexValue'].value}));
      }
      else{
        data.Capexes.push(new UpdateCapexCommand({Id : group.controls['CapexId'].value, Value :  group.controls['CapexValue'].value}));
      }
      if(group.controls['OpexId'].value == 0){
        data.NewOpexes.push(new CreateOpexCommand({Year : group.controls['Year'].value, Value: group.controls['OpexValue'].value}));
      }
      else{
        data.Opexes.push(new UpdateOpexCommand({Id : group.controls['OpexId'].value, Value :  group.controls['OpexValue'].value}));
      }
      if(group.controls['EbitId'].value == 0){
        data.NewEbits.push(new CreateEbitCommand({Year : group.controls['Year'].value, Value: group.controls['EbitValue'].value}));
      }
      else{
        data.Ebits.push(new UpdateEbitCommand({Id : group.controls['EbitId'].value, Value :  group.controls['EbitValue'].value}));
      }
    });
    return data;
  }
}
