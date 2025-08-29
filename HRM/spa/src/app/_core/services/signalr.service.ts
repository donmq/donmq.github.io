import { environment } from '@env/environment';
import { EventEmitter, Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { SignalRConstants } from '@constants/signalr.enum';
import { NgSnotifyService } from './ng-snotify.service';
import { TranslateService } from '@ngx-translate/core';
import { FunctionUtility } from '@utilities/function-utility';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  hubConnection: HubConnection;
  baseUrl = environment.signalrUrl;
  accountChangedEmitter: EventEmitter<string[]>;

  constructor(
    private _snotify: NgSnotifyService,
    private translate: TranslateService,
    private functionUtility: FunctionUtility
  ) {
    this.accountChangedEmitter = new EventEmitter();
  }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.baseUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch((err: string) => console.log('Error while starting connection: ' + err));
  }
  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .catch(err => console.log('Error while stopping connection: ' + err));
    }
  }
  addListeners() {
    this.hubConnection.on(SignalRConstants.ACCOUNT_CHANGED, (data: string[]) => this.accountChangedEmitter.emit(data));
    this.hubConnection.on(SignalRConstants.START_APPLICATION, () => {
      this._snotify.confirmOk(
        this.translate.instant('System.Message.SystemUpdate'),
        this.translate.instant('System.Caption.Error'),
        () => {
          this.functionUtility.hardReload()
        })
    });
  }
}
