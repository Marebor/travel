import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private baseUrl: string;
  private client: HttpClient;

  constructor(client: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.client = client;
    this.baseUrl = baseUrl;
  }

  getAllCustomers() : Observable<Customer[]> {
    return this.client.get<Customer[]>(this.baseUrl + 'api/customers');
  }

  createCustomer(name: string) : Observable<Customer> {
    return this.client.post<Customer>(this.baseUrl + 'api/customers', { name });
  }

  updateCustomer(customer: Customer) : Observable<void> {
    return this.client.put<void>(this.baseUrl + `api/customers/${customer.id}`, customer);
  }
}