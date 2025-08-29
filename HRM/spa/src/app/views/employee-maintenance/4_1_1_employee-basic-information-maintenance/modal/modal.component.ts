import { AfterViewInit, Component, input, ViewChild } from '@angular/core';
import { IconButton } from '@constants/common.constants';
import { UploadResultDto } from '@models/employee-maintenance/4_1_1_employee-basic-information-maintenance';
import { ModalService } from '@services/modal.service';
import { InjectBase } from '@utilities/inject-base-app';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-modal-4-1-1',
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent411 extends InjectBase implements AfterViewInit {
  @ViewChild('modal', { static: false }) directive: ModalDirective;
  id = input<string>(this.modalService.defaultModal)

  iconButton = IconButton;
  data: UploadResultDto = <UploadResultDto>{};

  constructor(private modalService: ModalService) { super(); }

  ngAfterViewInit(): void { this.modalService.add(this); }
  ngOnDestroy(): void { this.modalService.remove(this.id()); }

  open(source: UploadResultDto): void {
    this.data = source;
    this.directive.show()
  }
  close() {
    this.directive.hide()
  }
}
