import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
//import {CanActivate} from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthGaurd {

constructor(private accountService: AccountService, private toastr: ToastrService){}
canActivate(): Observable<boolean> {
  return this.accountService.currentUser$.pipe(
    map(user=>{
      if(user) return true;
      else{
        this.toastr.error('you will not pass');
        return false;
      }
    })

  )
  
}}