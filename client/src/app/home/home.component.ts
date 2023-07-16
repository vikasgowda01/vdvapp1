
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  registerMode =false;
  users:any;
  constructor() {}
  ngOnInit(): void {
    //this.getUsers();
  }
  /*getUsers(){
    this.http.get('http://localhost:5285/api/Users').subscribe({
      next: response => this.users =response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }*/
  registerToggle(){
    this.registerMode = !this.registerMode;
  }
  cancelRegisterMode(event : boolean){
    this.registerMode =event;
  }

}
