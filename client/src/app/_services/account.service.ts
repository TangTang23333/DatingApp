import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
// make request to api, services are singleton and injectable 

baseUrl = "https://localhost:5001/api/";

  constructor(private http: HttpClient) { }

  login(model: any) {

    return this.http.post(this.baseUrl + "account/login", model );

  }
}
