import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '@env/environment';
import { NgSnotifyService } from './ng-snotify.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class HostApplicationLifetimeHubService {
  loginDetectUrl: string = environment.hostApplicationLifetimeUrl;
  hubConnection: HubConnection;
  startTime: Date;

  constructor(private _snotify: NgSnotifyService, private _translate: TranslateService) {
    this.startConnection();
  }

  startConnection() {
    this.startTime = new Date();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.loginDetectUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: () => 10000
      })
      .build();

    this.hubConnection.start().catch(error => console.log('Error while starting login detect connection: ' + error));
  }

  onApplicationStart() {
    this.hubConnection.on('Sr.StartApplication', () => {
      this._snotify.confirmOk(
        this._translate.instant('System.Message.NewVersion'),
        this._translate.instant('System.Caption.Warning'),
        () => {
          window.location.href = window.location.href + '?reload=' + new Date().getTime();
          window.location.reload();
        })
    });
  }
}
