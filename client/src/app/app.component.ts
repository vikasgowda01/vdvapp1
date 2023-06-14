import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'VDVAPP';
  users: any; //users can be of any type cast in javascript it might be string,list of users,number.
  constructor(private http: HttpClient) {}
  ngOnInit(): void {
    this.http.get('http://localhost:5285/api/Users').subscribe({
    next: response => this.users =response,
    error: error => console.log(error),
    complete: () => console.log('Request has completed')
  })
  }
}
