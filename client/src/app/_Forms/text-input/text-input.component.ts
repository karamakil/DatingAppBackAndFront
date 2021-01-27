import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() type = "text"; //initial value is text


  constructor(@Self() public ngControl: NgControl) {
     this.ngControl.valueAccessor = this;
    // console.log(this.label)
    // console.log(this.type)
  }

  ngAfterViewInit(){
    console.log("ngAfterViewInit")
    console.log(this.ngControl)
    console.log(this.label);
  }


  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }


}
