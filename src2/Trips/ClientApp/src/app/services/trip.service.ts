import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Trip } from '../models/trip';

@Injectable({
  providedIn: 'root',
})
export class TripService {

  constructor(private client: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  getAllTrips() : Observable<Trip[]> {
    return this.client.get<Trip[]>(this.baseUrl + 'api/trips');
  }

  getAvailableDestinations() : Observable<string[]> {
    return this.client.get<string[]>(this.baseUrl + 'api/destinations');
  }

  createTrip(destination: string) : Observable<Trip> {
    return this.client.post<Trip>(this.baseUrl + 'api/trips', { destination });
  }

  updateTrip(trip: Trip) : Observable<void> {
    return this.client.put<void>(this.baseUrl + `api/trips/${trip.id}`, trip);
  }

  assignCustomerToTrip(tripId: number, customerId: number) : Observable<void> {
    return this.client.post<void>(this.baseUrl + `api/trips/${tripId}/participants`, { customerId });
  }
}