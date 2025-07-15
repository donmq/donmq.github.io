import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployPlnoService {
  url = environment.apiUrl;

  constructor(private http: HttpClient) { }

}
