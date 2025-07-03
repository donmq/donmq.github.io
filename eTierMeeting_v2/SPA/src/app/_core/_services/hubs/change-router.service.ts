import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChangeRouterService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageReceived = new Subject<string>();
  constructor() {
    this.createHubConnection()
  }

  async createHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'hubs/changeRouter')
      .withAutomaticReconnect()
      .build();
    await this.hubConnection.start();
    this.hubConnection.on('ReceiveMessage', (router: string) => {
      this.messageReceived.next(`${router}`);
    });

    this.hubConnection.stop().then(() => {
      // Start a new connection
      this.startConnection();
    });

  }

  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('Error while starting SignalR: ' + err));
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message);
  }

  getMessageObservable(): Observable<string> {
    return this.messageReceived.asObservable();
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch(error => console.log(error));
  }

}
