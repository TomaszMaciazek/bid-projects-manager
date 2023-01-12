import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AddCurrencyCommand } from 'src/app/commands/add-currency-command.model';
import { CurrencyService } from 'src/app/services/currency.service';

@Component({
  selector: 'add-currency',
  templateUrl: './add-currency.component.html',
  styleUrls: ['./add-currency.component.scss']
})
export class AddCurrencyComponent {
  @Input() visible : boolean;
  @Output() currencyCreated = new EventEmitter<boolean>();
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  public form : FormGroup;

  constructor(
    private currencyService: CurrencyService,
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
  }

  submit(){
    if(this.form.valid){
      var command = new AddCurrencyCommand({
        Name: this.form.controls['Name'].value,
        Code: this.form.controls['Code'].value
      });
      this.spinner.show();
      this.currencyService.addCurrency(command).then(res => {
        this.toastr.success('Currency added');
        this.spinner.hide();
        this.reset();
        this.currencyCreated.emit(true);
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
    })
  }
}
