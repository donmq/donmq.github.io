import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { WebcamImage, WebcamInitError } from 'ngx-webcam';

@Injectable({
  providedIn: 'root',
})
export class WebcamService {
  private trigger: Subject<void> = new Subject<void>();
  private nextWebcam: Subject<boolean | string> = new Subject<boolean | string>();
  private errors: WebcamInitError[] = [];
  private deviceId: string | null = null;

  // Observable for trigger
  get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  // Observable for switching webcams
  get nextWebcamObservable(): Observable<boolean | string> {
    return this.nextWebcam.asObservable();
  }

  // Getter for errors
  get initErrors(): WebcamInitError[] {
    return this.errors;
  }

  // Getter for current deviceId
  get currentDeviceId(): string | null {
    return this.deviceId;
  }

  // Trigger a snapshot
  public triggerSnapshot(): void {
    this.trigger.next();
  }

  // Switch to next or specific webcam
  public switchWebcam(directionOrDeviceId: boolean | string): void {
    this.nextWebcam.next(directionOrDeviceId);
  }

  // Handle initialization errors
  public handleInitError(error: WebcamInitError): void {
    this.errors.push(error);
  }

  // Track current camera device
  public setDeviceId(deviceId: string): void {
    this.deviceId = deviceId;
  }
}
