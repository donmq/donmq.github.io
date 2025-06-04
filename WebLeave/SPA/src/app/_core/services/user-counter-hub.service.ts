import { environment } from '@env/environment';
import { EventEmitter, Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class UserCounterHubService {
  userCounterUrl: string = environment.userCounterUrl;
  hubConnection: HubConnection;
  connectedUsersEmitter: EventEmitter<number>;
  connectedUsers: number = 0;

  constructor() {
    this.connectedUsersEmitter = new EventEmitter();
    this.startConnection();
    this.onCountUsers();
  }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.userCounterUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log('Error while starting connection: ' + error));
  }

  onCountUsers() {
    this.hubConnection.on('Sr.ConnectedUsers', (data: number) => {
      this.connectedUsersEmitter.emit(data);
      this.connectedUsers = data;
    });
  }
}
