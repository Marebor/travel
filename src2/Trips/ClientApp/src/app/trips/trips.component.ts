import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Trip } from '../models/trip';
import { TripService } from '../services/trip.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html'
})
export class TripsComponent implements OnInit {
  editedTrip: Trip;
  waitingForResponse: boolean;
  destinations: string[];
  editTripForm = this.formBuilder.group({
    destination: null,
    isCancelled: null,
    owner: null,
  });
  trips: Trip[];

  constructor(
    private tripService: TripService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.tripService.getAllTrips().subscribe(trips => this.trips = trips);
  }

  newTrip() {
    this.editedTrip = {
      id: null,
      destination: null,
      participants: null,
      isCancelled: null,
      owner: null
    };

    this.updateDestinations();
  }

  editTrip(trip: Trip) {
    this.editedTrip = trip;

    this.updateDestinations();
  }

  cancelEdit() {
    this.editedTrip = null;
    this.editTripForm.reset();
  }

  private updateDestinations() {
    this.tripService.getAvailableDestinations().subscribe(
      res => this.destinations = res,
      error => this.handleError(error));
  }

  editFormSubmitted() {
    if (this.editedTrip.id) {
      this.tripService.updateTrip({
        id: this.editedTrip.id,
        ...this.editTripForm.value,
      }).subscribe(
        response => {
          for(var k in this.editTripForm.value) {
            if (!!this.editTripForm.value[k]) {
              this.editedTrip[k]=this.editTripForm.value[k];
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
      this.tripService.createTrip(this.editTripForm.value.destination).subscribe(
        response => {
          this.trips.push(response);
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
