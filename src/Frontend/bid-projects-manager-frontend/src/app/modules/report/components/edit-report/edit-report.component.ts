import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationService } from 'primeng/api';
import { CreateReportDefinitionCommand } from 'src/app/commands/create-report-definition-command.model';
import { UpdateReportDefinitionCommand } from 'src/app/commands/update-report-definition-command.model';
import { ReportDefinition } from 'src/app/models/report-definition.model';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-edit-report',
  templateUrl: './edit-report.component.html',
  styleUrls: ['./edit-report.component.scss']
})
export class EditReportComponent implements OnInit {
  public isEdit : boolean = false;
  public form : FormGroup;
  public id : number = 0;

  public reportDefinition : ReportDefinition | null = null;

  constructor(
    private reportService : ReportService,
    private toastr : ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private formBuilder: FormBuilder
  ){}

  ngOnInit(): void {
    this.spinner.show();
    this.activatedRoute.params.subscribe((routeParams: Params) => {
      this.isEdit = !(routeParams['id'] == '0')
      this.id = routeParams['id'];
      this.createForm();
      if(this.isEdit){
          this.reportService.getReportById(this.id).then(res => {this.reportDefinition = res; this.fillFormWithReportData()});
      }
      this.spinner.hide();
    });
  }

  returnToReports(){
    this.router.navigate(["/reports/"]);
  }

  save(){
    if(this.form.valid){
      this.spinner.show();
      if(!this.isEdit){
        let command = new CreateReportDefinitionCommand({
          Title : this.form.controls["Title"].value,
          Group : this.form.controls["Group"].value,
          MaxRow : this.form.controls["MaxRows"].value,
          Description : this.form.controls["Description"].value,
          XmlDefinition : this.form.controls["XmlDefinition"].value
        });
        this.reportService.createReport(command).then(res => {
          this.spinner.hide();
          if(res){
            this.router.navigate(["/reports/"]);
          }
          else{
            this.toastr.error("Failed to create report definition")
          }
        })
        .catch(error => {
          this.toastr.error(error);
          this.spinner.hide();
        });
      }
      else{
        let command = new UpdateReportDefinitionCommand({
          Id : this.id,
          Title : this.form.controls["Title"].value,
          Group : this.form.controls["Group"].value,
          MaxRow : this.form.controls["MaxRows"].value,
          Version : this.form.controls["Version"].value,
          Description : this.form.controls["Description"].value,
          IsActive : this.form.controls["Active"].value,
          XmlDefinition : this.form.controls["XmlDefinition"].value
        });
        this.reportService.updateReport(command).then(res => {
          this.spinner.hide();
          if(res){
            this.router.navigate(["/reports/"]);
          }
          else{
            this.toastr.error("Failed to save report definition")
          }
        })
        .catch(error => {
          this.toastr.error(error);
          this.spinner.hide();
        });
      }
    }
    else{
      this.toastr.warning("Form not valid");
    }
  }

  private createForm(){
    this.form = this.formBuilder.group({
      'Title' : this.formBuilder.control(null, Validators.required),
      'Group' : this.formBuilder.control(null, Validators.required),
      'Version' : this.formBuilder.control(null, this.isEdit ? Validators.required : Validators.nullValidator),
      'Active' : this.formBuilder.control(null, this.isEdit ? Validators.required : Validators.nullValidator),
      'Description' : this.formBuilder.control(null, Validators.required),
      'MaxRows' : this.formBuilder.control(10000, Validators.required),
      'XmlDefinition' : this.formBuilder.control(null, Validators.required)
    })
  }

  private fillFormWithReportData(){
    if(this.reportDefinition != null){
      this.form.controls["Version"].setValue(this.reportDefinition.version);
      this.form.controls["Title"].setValue(this.reportDefinition.title);
      this.form.controls["Group"].setValue(this.reportDefinition.group);
      this.form.controls["Active"].setValue(this.reportDefinition.isActive);
      this.form.controls["Description"].setValue(this.reportDefinition.description);
      this.form.controls["Version"].setValue(this.reportDefinition.version);
      this.form.controls["MaxRows"].setValue(this.reportDefinition.maxRow);
      this.form.controls["XmlDefinition"].setValue(this.reportDefinition.xmlDefinition);
    }
  }
}
