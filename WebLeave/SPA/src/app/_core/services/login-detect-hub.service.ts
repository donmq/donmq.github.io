import { Injectable } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { environment } from '@env/environment';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Users } from '@models/auth/users.model';
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LoginDetectHubService {
  loginDetectUrl: string = environment.loginDetectUrl;
  hubConnection: HubConnection;

  constructor(private authService: AuthService) {
    this.startConnection();
    this.onLoginDetect();
  }

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.loginDetectUrl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log('Error while starting login detect connection: ' + error));
  }

  onLoginDetect() {
    this.hubConnection.on('Sr.LoginDetect', async (ipClient: string) => {
      let user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
      let ipLocal: string = localStorage.getItem(LocalStorageConstants.IPLOCAL);
      if (ipClient === ipLocal) {
        let model: UserForLoginParam = <UserForLoginParam>{
          username: user.username,
          ipLocal: ipLocal,
        }
        await firstValueFrom(this.authService.logout(model, true, 'HasBeenLogged'));
      }
    });
  }
}
