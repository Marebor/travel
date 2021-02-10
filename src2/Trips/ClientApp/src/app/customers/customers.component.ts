import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Customer } from '../models/customer';
import { CustomerService } from '../services/Customer.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html'
})
export class CustomersComponent implements OnInit {
  editedCustomer: Customer;
  waitingForResponse: boolean;
  destinations: string[];
  editCustomerForm = this.formBuilder.group({
    name: null,
    owner: null,
  });
  customers: Customer[];

  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.customerService.getAllCustomers().subscribe(customers => this.customers = customers);
  }

  newCustomer() {
    this.editedCustomer = {
      id: null,
      name: null,
      owner: null
    };
  }

  editCustomer(Customer: Customer) {
    this.editedCustomer = Customer;
  }

  cancelEdit() {
    this.editedCustomer = null;
    this.editCustomerForm.reset();
  }

  editFormSubmitted() {
    if (this.editedCustomer.id) {
      this.customerService.updateCustomer({
        id: this.editedCustomer.id,
        ...this.editCustomerForm.value,
      }).subscribe(
        response => {
          for(var k in this.editCustomerForm.value) {
            if (!!this.editCustomerForm.value[k]) {
              this.editedCustomer[k]=this.editCustomerForm.value[k];
            }
          }

          this.cancelEdit();
        },
        error => this.handleError(error),
        () => {
          this.waitingForResponse = false;
        }
      );
    } else {
      this.customerService.createCustomer(this.editCustomerForm.value.name).subscribe(
        response => {
          this.customers.push(response);
          this.cancelEdit();
        },
        error => this.handleError(error),
        () => {
          this.waitingForResponse = false;
        }
      );
    }

    this.waitingForResponse = true;
  }

  private handleError(error: any) {
    alert('Request failed: ' + error.message);
  }
}
