import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ParamType } from 'src/app/enums/param-type';
import { ReportGenerationPage } from 'src/app/models/report-generation-page.model';
import { GeneratorParamValue } from 'src/app/queries/generator-param-value.model';
import { GeneratorQuery } from 'src/app/queries/generator-query.model';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-generate-report',
  templateUrl: './generate-report.component.html',
  styleUrls: ['./generate-report.component.scss'],
  providers: [DatePipe]
})
export class GenerateReportComponent implements OnInit {

    public form: FormGroup;

    public pageSettings : ReportGenerationPage;
    private reportId : number = 0;

    public paramType = ParamType;

    constructor(
      private reportService : ReportService,
      private toastr : ToastrService,
      private spinner: NgxSpinnerService,
      private router: Router,
      private activatedRoute: ActivatedRoute,
      public datepipe: DatePipe,
      private formBuilder: FormBuilder
    ){}

  ngOnInit(): void {
    this.spinner.show();
    this.activatedRoute.params.subscribe((routeParams: Params) => {
      this.reportId = routeParams['id'];
      this.createForm();
      this.reportService.getReportGeneratorPage(this.reportId).then(res => {
        this.pageSettings = res;
        this.fillFormWithParams();
        this.spinner.hide();
      });
    });
  }

  returnToReports(){
    this.router.navigate(["/reports/"]);
  }

  fillFormWithParams(){
    if(this.pageSettings){
      this.pageSettings.params.forEach(param => {
        if(param.type == ParamType.Dis || param.type == ParamType.Lst){
          (this.form.controls["Params"] as FormGroup).addControl(param.name, this.formBuilder.control([], param.isRequired ? Validators.required : Validators.nullValidator));
        }
        else{
          (this.form.controls["Params"] as FormGroup).addControl(param.name, this.formBuilder.control(null, param.isRequired ? Validators.required : Validators.nullValidator));
        }
      });
    }
  }

  generate(){
    this.markFields();
    if(this.form.valid){
      this.spinner.show();
      var query = new GeneratorQuery();
      query.MaxRows = this.form.controls['MaxRows'].value;
      query.Reportid = this.reportId;
      query.ParamsValues = [];

      let formParams = this.form.controls['Params'] as FormGroup;

      this.params.forEach(param => {
        if(Array.isArray(formParams.controls[param.name].value)){
          query.ParamsValues.push(new GeneratorParamValue({Key: param.name, Values: formParams.controls[param.name].value}));
        }
        else if(param.type == this.paramType.Dat){
          var dateValue = formParams.controls[param.name].value;
          var valueString : string | null = dateValue != null
            ? new Date(Date.UTC(new Date(dateValue).getFullYear(), new Date(dateValue).getMonth(), new Date(dateValue).getDate())).toISOString()
            : null;

            query.ParamsValues.push(new GeneratorParamValue({Key: param.name, Values: [valueString]}));
        }
        else{
          query.ParamsValues.push(new GeneratorParamValue({Key: param.name, Values: [formParams.controls[param.name].value]}));
        }
      })

      this.reportService.generateReport(query).then(res => {
        this.spinner.hide();
        let blob = new Blob([res as BlobPart], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
        var a = document.createElement("a");
        a.href = URL.createObjectURL(blob);
        a.download = 'Report_' + this.datepipe.transform(new Date(),'dd_MM_yyyy_HH_mm_ss');
        a.click();
      })
      .catch(error => {
        this.toastr.error(error);
        this.spinner.hide();
      });
    }
    else{
      this.toastr.warning("Fill all required parameters");
    }
  }

  private createForm(){
    this.form = this.formBuilder.group({
      'MaxRows': this.formBuilder.control(null, Validators.nullValidator),
      'Params': this.formBuilder.group([])
    });
  }

  private markFields(){
    Object.keys(this.paramsGroup.controls).forEach((key) => {
      this.paramsGroup.controls[key].markAsDirty();
    });
  }

  public get paramsGroup(){
    return this.form.controls["Params"] as FormGroup
  }

  public get params(){
    if(this.pageSettings != null){
      return this.pageSettings.params;
    }
    return [];
  }
}
