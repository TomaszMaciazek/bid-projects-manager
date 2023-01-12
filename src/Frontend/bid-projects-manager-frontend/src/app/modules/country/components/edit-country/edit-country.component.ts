import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { UpdateCountryCommand } from 'src/app/commands/update-country-command.model';
import { Currency } from 'src/app/models/currency.model';
import { CountryService } from 'src/app/services/country.service';

@Component({
  selector: 'edit-country',
  templateUrl: './edit-country.component.html',
  styleUrls: ['./edit-country.component.scss']
})
export class EditCountryComponent implements OnInit {
  @Input() currencies : Array<Currency> = [];
  @Input() visible : boolean;
  @Output() countryUpdated = new EventEmitter<boolean>();
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();


  private id: number;

  public form : FormGroup;

  constructor(
    private countryService: CountryService,
    private spinner: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private toastr : ToastrService,
    ){}

  ngOnInit(): void {
    this.createForm();
  }

  onHide() {
    this.reset();
    this.onClose.emit(true);
  }

  setValues(id: number, name: string, code: string, currency: Currency){
    this.id = id;
    this.form.controls['Name'].setValue(name);
    this.form.controls['Code'].setValue(code);
    this.form.controls['Currency'].setValue(currency);
  }
  
  reset(){
    this.form.controls['Name'].setValue(null);
    this.form.controls['Code'].setValue(null);
    this.form.controls['Currency'].setValue(null);
  }

  submit(){
    if(this.form.valid){
      var command = new UpdateCountryCommand({
        Id: this.id,
        Name: this.form.controls['Name'].value,
        Code: this.form.controls['Code'].value,
        CurrencyId: this.form.controls['Currency'].value.id,
      });
      this.spinner.show();
      this.countryService.updateCountry(command).then(res => {
        this.toastr.success('Country updated');
        this.spinner.hide();
        this.reset();
        this.countryUpdated.emit(true);
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
