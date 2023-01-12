import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { UpdateCurrencyCommand } from 'src/app/commands/update-currency-command.model';
import { CurrencyService } from 'src/app/services/currency.service';

@Component({
  selector: 'edit-currency',
  templateUrl: './edit-currency.component.html',
  styleUrls: ['./edit-currency.component.scss']
})
export class EditCurrencyComponent {
  @Input() visible : boolean;
  @Output() currencyUpdated = new EventEmitter<boolean>();
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();


  private id: number;

  public form : FormGroup;

  constructor(
    private currencyService: CurrencyService,
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

  setValues(id: number, name: string, code: string){
    this.id = id;
    this.form.controls['Name'].setValue(name);
    this.form.controls['Code'].setValue(code);
  }
  
  reset(){
    this.form.controls['Name'].setValue(null);
    this.form.controls['Code'].setValue(null);
  }

  submit(){
    if(this.form.valid){
      var command = new UpdateCurrencyCommand({
        Id: this.id,
        Name: this.form.controls['Name'].value,
        Code: this.form.controls['Code'].value
      });
      this.spinner.show();
      this.currencyService.updateCurrency(command).then(res => {
        this.toastr.success('Currency updated');
        this.spinner.hide();
        this.reset();
        this.currencyUpdated.emit(true);
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
      'Code': this.formBuilder.control(null, Validators.required)
    })
  }
}
