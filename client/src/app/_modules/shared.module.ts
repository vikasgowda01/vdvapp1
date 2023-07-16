import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MatTabsModule } from '@angular/material/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    //TabsModule.forRoot(),
    NgxGalleryModule,
    MatTabsModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
      
    })
  ],
  exports:[
    ReactiveFormsModule,
    SelectDropDownModule,
    ToastrModule,
    MatTabsModule,
    NgxGalleryModule
    

  ]
})
export class SharedModule { }
