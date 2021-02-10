import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Customer } from '../models/customer';
import { Trip } from '../models/trip';
import { CustomerService } from '../services/customer.service';
import { TripService } from '../services/trip.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  trips: Trip[];
  tripDetails: Trip;
  availableCustomers: Customer[];
  selectedCustomer: Customer;
  assignForm = this.formBuilder.group({
    customerId: null
  });

  constructor(
    private tripService: TripService, 
    private customerService: CustomerService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.tripService.getAllTrips().subscribe(
      trips => this.trips = trips
    );
  }

  assignNewCustomer() {
    this.customerService.getAllCustomers().subscribe(
      customers => this.availableCustomers = customers.filter(c => !this.tripDetails.participants.find(p => p.id === c.id)),
      error => alert("Request failed: " + error.message)
    )
  }

  assignFormSubmitted() {
    let customer = this.availableCustomers.find(c => c.id == this.assignForm.value.customerId);

    this.tripService.assignCustomerToTrip(this.tripDetails.id, customer.id).subscribe(
      _ => {
        this.tripDetails.participants.push(customer);
        this.tripDetails = null;
        this.availableCustomers = null;
        this.assignForm.reset();
      },
      error => alert("Request failed: " + error.message)
    );
  }
}
