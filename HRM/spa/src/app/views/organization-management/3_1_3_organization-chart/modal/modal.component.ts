import { InjectBase } from '@utilities/inject-base-app';
import {
  AfterViewInit,
  Component,
  input,
  ViewChild,
} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ClassButton, IconButton } from '@constants/common.constants';
import { ModalService } from '@services/modal.service';
import { INodeDto } from '@models/organization-management/3_1_3_organization-chart';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent extends InjectBase implements AfterViewInit {
  @ViewChild('modal', { static: false }) directive: ModalDirective;
  id = input<string>('modal')
  data: INodeDto = <INodeDto>{};

  IconButton = IconButton;
  classButton = ClassButton;

  constructor(private modalService: ModalService) { super() }

  ngAfterViewInit(): void { this.modalService.add(this); }
  ngOnDestroy(): void { this.modalService.remove(this.id()); }

  open(data: any): void {
    this.data = data
    this.directive.show()
  }
  close = () => this.directive.hide();
}
