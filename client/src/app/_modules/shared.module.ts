import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { HttpClientModule} from '@angular/common/http';
import { FileUploadModule } from 'ng2-file-upload';
import { ReactiveFormsModule } from '@angular/forms';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker'

@NgModule({
  declarations: [],
  imports: [
    HttpClientModule,
    CommonModule,
    ToastrModule.forRoot({ positionClass: "toast-bottom-right" }),
    BsDropdownModule.forRoot(), 
    TabsModule.forRoot(),
    NgxGalleryModule ,
    FileUploadModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
  ],
  exports: [
    ToastrModule,
    BsDropdownModule,
    TabsModule,
    NgxGalleryModule,
    FileUploadModule,
    ReactiveFormsModule,
    BsDatepickerModule
  ]
})
export class SharedModule { }
