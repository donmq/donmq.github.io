import { take } from 'rxjs/operators';
import { Component, EventEmitter, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BarcodeFormat } from '@zxing/library';

@Component({
  selector: 'app-scan-modal',
  templateUrl: './scan-modal.component.html',
  styleUrls: ['./scan-modal.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ScanModalComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  @Output() qrResultString = new EventEmitter<string>();
  @ViewChild('scanner', { static: false }) scanner: ZXingScannerComponent;

  hasDevices: boolean;
  check: boolean;
  enableScanner: boolean = false;
  formatsEnabled: BarcodeFormat[] = [
    BarcodeFormat.CODE_128,
    BarcodeFormat.DATA_MATRIX,
    BarcodeFormat.EAN_13,
    BarcodeFormat.QR_CODE,
  ];

  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo = null;

  showChildModal(): void {
    if (this.currentDevice) this.scanner.device = this.currentDevice
    this.childModal.show();
    this.childModal.onHide.pipe(take(1)).subscribe(() => this.enableScanner = false);
    this.enableScanner = true
  }

  constructor() { }

  ngOnInit() {
  }

  onCamerasFound(devices: MediaDeviceInfo[]): void {
    this.availableDevices = devices;
    this.hasDevices = Boolean(devices && devices.length);
    // Tự động chọn thiết bị đầu tiên nếu chưa có thiết bị nào được chọn
    if (!this.currentDevice && this.availableDevices.length > 0) {
      this.currentDevice = this.availableDevices[0];
      this.scanner.device = this.currentDevice; // Đồng bộ với scanner
    }
  }

  handleQrCodeResult(resultString: string) {
    if (resultString != null) {
      this.qrResultString.emit(resultString);
      this.scanner.reset();
      this.childModal.hide();
    }
  }

  onDeviceSelectChange(selectedValue: string): void {
    const device = this.availableDevices.find(x => x.deviceId === selectedValue);
    this.currentDevice = device || null;

    // Đồng bộ scanner với thiết bị được chọn
    if (this.currentDevice) {
      this.scanner.device = this.currentDevice;
    }
  }
}
