import { EventEmitter, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { SignalRConstants } from '../_constants/signalr.enum';
import { SweetAlertService } from './sweet-alert.service';
import { TranslateService } from '@ngx-translate/core';
import * as signalR from '@microsoft/signalr';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';

const ip = window.location.hostname;
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;
  usersCount = new EventEmitter<number>();
  baseUrl = {};

  constructor(private sweetAlertifyService: SweetAlertService, private translate: TranslateService,) {
    this.buildConnection();
    this.startConnection();
    this.startConnectionNewVersion();
    this.addListenersNewVersion();
  }

  buildConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`${environment.baseUrl}hitcounter`, {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    }).build();
  }

  startConnection = () => {
    this.hubConnection.start();
    this.hubConnection.on('updateCount', (data: number) => {
      this.usersCount.emit(data);
    });
  }

  startConnectionNewVersion() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.signalrUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch((err: string) => console.log('Error while starting connection: ' + err));
  }

  addListenersNewVersion() {
    this.hubConnection.on(SignalRConstants.START_APPLICATION, () => {
      this.sweetAlertifyService.confirm(this.translate.instant('common.error'), this.translate.instant('error.system_update'), () => {
        window.location.reload();
      }, true);
    });
  }
}
