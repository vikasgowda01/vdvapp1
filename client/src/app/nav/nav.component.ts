import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrModule } from 'ngx-toastr/toastr/toastr.module';
import { ToastrService } from 'ngx-toastr';
import { AfterViewInit, ViewChild } from '@angular/core';
import { MdbDropdownDirective } from 'mdb-angular-ui-kit/dropdown';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: any={};
 
  //currentUser$:Observable<User | null>=of(null);


  constructor(public accountService: AccountService, private router: Router,private toastr:ToastrService) {}

  ngOnInit(): void {
    }
  //this.currentUser$=this.accountService.currentUser$;
  
 

  login(){
    this.accountService.login(this.model).subscribe({
      next: _=>
      {
      this.router.navigateByUrl('/members'),
      this.model = {};
      //error: error =>this.toastr.error(error.error)
      }
                
    })
  }
  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
    
   
  }

}
