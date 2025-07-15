import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
})
export class LogoutComponent implements OnInit {

  constructor(private _router: Router,) { }

  ngOnInit() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this._router.navigate(['/home']);
  }
}
