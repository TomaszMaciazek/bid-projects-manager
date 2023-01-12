import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AddCountryCommand } from 'src/app/commands/add-country-command.model';
import { Currency } from 'src/app/models/currency.model';
import { CountryService } from 'src/app/services/country.service';

@Component({
  selector: 'add-country',
  templateUrl: './add-country.component.html',
  styleUrls: ['./add-country.component.scss']
})
export class AddCountryComponent implements OnInit {

  @Input() currencies : Array<Currency> = [];
  @Input() visible : boolean;
  @Output() countryCreated = new EventEmitter<boolean>();
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  public form : FormGroup;

  constructor(
    private countryService: CountryService,
    private spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private toastr : ToastrService
    ){}

  ngOnInit(): void {
    this.createForm();
  }

  onHide() {
    this.reset();
    this.onClose.emit(true);
  }
  
  reset(){
    this.form.controls['Name'].setValue(null);
    this.form.controls['Code'].setValue(null);
    this.form.controls['Currency'].setValue(null);
  }

  submit(){
    if(this.form.valid){
      var command = new AddCountryCommand({
        Name: this.form.controls['Name'].value,
        Code: this.form.controls['Code'].value,
        CurrencyId: this.form.controls['Currency'].value.id,
      });
      this.spinner.show();
      this.countryService.addCountry(command).then(res => {
        this.toastr.success('Country added');
        this.spinner.hide();
        this.reset();
        this.countryCreated.emit(true);
      })
      .catch(error => {
        this.toastr.error(error);
        this.spinner.hide();
      });
    }
  }

  private createForm(){
    this.form = this.formBuilder.group({
      'Name': this.formBuilder.control(null, Validators.required),
      'Code': this.formBuilder.control(null, Validators.required),
      'Currency': this.formBuilder.control(null, Validators.required),
    })
  }
}
