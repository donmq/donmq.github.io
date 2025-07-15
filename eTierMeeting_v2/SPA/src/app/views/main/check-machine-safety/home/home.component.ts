import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { ScanModalComponent } from '../../scan-modal/scan-modal.component';
import { CheckMachineSafetyService } from '../../../../_core/_services/check-machine-safety.service';
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { Machine_Safe_Check, Machine_Safe_Check_Scan } from '../../../../_core/_models/machine_safe_check';
import { WebcamImage } from 'ngx-webcam';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';
import { WebcamService } from '../../../../_core/_services/webcam.service';
import { DestroyService } from '../../../../_core/_services/destroy.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [DestroyService]
})
export class HomeComponent implements OnInit {
  scan: boolean = true;
  getMachine: boolean = false;
  machineName: string;
  assnoID: string;
  ownerFty: string;
  itemCheckMachine: Machine_Safe_Check = null;
  qrResultString: string;
  lang: string;
  userName: string;
  modalRef: BsModalRef;
  surveyQuestions: Machine_Safe_Check_Scan[] = [];
  @ViewChild('modelScan') modelScan: ScanModalComponent;
  public showWebcam = true;
  public allowCameraSwitch = true;
  public videoOptions: MediaTrackConstraints = {};
  public capturedImage: WebcamImage | null = null;
  currentQuestion: Machine_Safe_Check_Scan | null = null;

  // #region constructor
  constructor(private _service: CheckMachineSafetyService,
    private _functionUtilityService: FunctionUtility,
    private _spinnerService: NgxSpinnerService,
    private _alertifyService: AlertifyService,
    private modalService: BsModalService,
    private webcamService: WebcamService,
    private translate: TranslateService) { }

  // #region ngOnInit
  ngOnInit() {
    this.lang = localStorage.getItem('lang');
    const userLogedin = JSON.parse(localStorage.getItem('user'));
    if (userLogedin) {
      this.userName = userLogedin.userName;
    }

    const listCheckDataParseJson = localStorage.getItem('itemCheckMachineSafety');
    if (listCheckDataParseJson !== '' && listCheckDataParseJson !== null) {
      this.itemCheckMachine = JSON.parse(listCheckDataParseJson);
      this.machineName = this.itemCheckMachine.ownerFty + this.itemCheckMachine.machineID;
      this.getDataMachine();
    }
    this.loadQuestion();
  }

  // #region area webcam
  triggerSnapshot(): void {
    this.webcamService.triggerSnapshot();
  }

  // Khi chọn đáp án
  openWebcame(question: Machine_Safe_Check_Scan, answer: 'y' | 'n' | 'n/a', template?: TemplateRef<any>) {
    question.answer = answer;
    if (answer === 'n' && template) {
      this.currentQuestion = question;
      this.modalRef = this.modalService.show(template);
    } else {
      question.image = null;
    }
  }

  handleImage(webcamImage: WebcamImage): void {
    this.capturedImage = webcamImage;
    if (this.currentQuestion) {
      this.currentQuestion.image = webcamImage.imageAsDataUrl;
    }
  }

  toggleWebcam(): void {
    this.showWebcam = !this.showWebcam;
  }

  handleInitError(error: any): void {
    this.webcamService.handleInitError(error);
  }

  cameraSwitched(deviceId: string): void {
    this.webcamService.setDeviceId(deviceId);
  }

  get triggerObservable() {
    return this.webcamService.triggerObservable;
  }

  get nextWebcamObservable() {
    return this.webcamService.nextWebcamObservable;
  }
  // #region end webcam

  // #region loadQuestion
  loadQuestion() {
    this._service.getQuestion(this.lang).subscribe(res => {
      this.surveyQuestions = res;
    });
  }

  // #region scan thu cong hoac scan machine
  scanManually() {
    this.scan = !this.scan;
  }

  // #region saveMachineSafetyCheck
  saveMachineSafetyCheck(): void {
    const formData = new FormData();
    const errorMessages: string[] = []; // Danh sách lỗi

    // Kiểm tra lỗi trước khi xử lý FormData
    this.surveyQuestions.forEach((q, index) => {
      if (q.answer === 'n' && !q.image) {
        errorMessages.push(`${index + 1}`);
      }
    });

    // Nếu có lỗi, hiển thị thông báo và dừng lại
    if (errorMessages.length > 0) {
      const messTrans = this.translate.instant('checkmachineSafety.please_select_image');
      this._alertifyService.error(`${messTrans} ${errorMessages.join(', ')}`);
      return;
    }

    // Thêm dữ liệu vào FormData nếu không có lỗi
    this.surveyQuestions.forEach((q, index) => {
      formData.append(`questions[${index}][key]`, q.key);
      formData.append(`questions[${index}][value]`, q.value);
      formData.append(`questions[${index}][answer]`, q.answer);

      // Kiểm tra nếu có hình ảnh thì chuyển đổi base64 sang Blob và append vào FormData
      if (q.image) {
        const blob = this._functionUtilityService.base64ToBlob(q.image, 'image/jpeg');
        formData.append(`questions[${index}].image`, blob, `question-${index}.jpg`);
      }
    });
    formData.append('AssnoID', this.assnoID);
    formData.append('OwnerFty', this.ownerFty);
    formData.append('UserName', this.userName);

    this._spinnerService.show();
    this._service.saveMachineSafetyCheck(formData).subscribe({
      next: (res) => {
        if (res.success) {
          this._alertifyService.success(this.translate.instant('alert.alert-success-move'));
          this.clearData();
        } else {
          this._alertifyService.error(this.translate.instant('alert.alert_error_occurred'));
        }
        this._spinnerService.hide();
      },
      error: () => {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('error.system_error'));
      }
    });
  }

  isSurveyCompleted(): boolean {
    // Kiểm tra nếu tất cả các câu hỏi đã được trả lời
    return this.surveyQuestions.every((q) => q.answer);
  }

  // #region Get Machine
  getDataMachine() {
    if (!this.machineName?.trim()) return;
    this._spinnerService.show();
    if (localStorage.getItem('factory') === 'TSH' && this.machineName.substring(0, 1) !== 'U') {
      this.machineName = 'U' + this.machineName;
    }
    const machine = <Machine_Safe_Check>{
      machineID: this.machineName.trim().substring(1, this.machineName.length),
      ownerFty: this.machineName.trim().substring(0, 1)
    };

    this.assnoID = machine.machineID;
    this.ownerFty = machine.ownerFty;
    this._service.getMachine(machine.ownerFty + machine.machineID, this.lang).subscribe(res => {
      if (res) {
        this.itemCheckMachine = res;
        const itemCheckMachine = JSON.stringify(this.itemCheckMachine);
        localStorage.setItem('itemCheckMachineSafety', itemCheckMachine);
        this.loadQuestion();
      } else {
        this._spinnerService.hide();
        this.itemCheckMachine = null;
        this._alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
      }

    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
    this._spinnerService.hide();

    this.machineName = '';
  }

  // #region clearData
  clearData(isShowMessage?: boolean) {
    this._spinnerService.show();
    this.itemCheckMachine = null;
    this.assnoID = '';
    this.ownerFty = '';
    localStorage.removeItem('itemCheckMachineSafety');
    if (isShowMessage)
      this._alertifyService.success(this.translate.instant('checkmachineSafety.cancel_all_data'));
    this._spinnerService.hide();
  }

  refresh() {
    window.location.reload();
  }

  // #region openScan
  openScan() {
    this.machineName = '';
    this.modelScan.showChildModal();
    this.codeQr(this.qrResultString);
  }

  // #region codeQr
  codeQr(event: string) {
    this.qrResultString = event;
    if (this.qrResultString !== undefined && this.qrResultString !== null) {
      this.machineName = this.qrResultString;
      this.getDataMachine();
      this.qrResultString = null
    }
  }

}
