import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MatTabsModule } from '@angular/material/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MdbDropdownModule } from 'mdb-angular-ui-kit/dropdown';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';
import {MatSelectModule} from '@angular/material/select';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SelectDropDownModule,
    //TabsModule.forRoot(),
    NgxGalleryModule,
    MatTabsModule,
    MdbDropdownModule, 
    MdbRippleModule,
    MatSelectModule,
    NgxSpinnerModule.forRoot({
        type:'line-scale-party'
    }),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
      
    }),
    FileUploadModule
  ],
  exports:[
    ReactiveFormsModule,
    SelectDropDownModule,
    ToastrModule,
    MatTabsModule,
    NgxGalleryModule,
    MdbDropdownModule, 
    MdbRippleModule,
    MatSelectModule,
    NgxSpinnerModule,
    FileUploadModule
    

  ]
})
export class SharedModule { }
