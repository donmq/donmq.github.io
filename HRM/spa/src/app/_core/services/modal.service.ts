import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class ModalService {
  private modals: any[] = [];
  defaultModal : string = 'modal'
  onHide: EventEmitter<any> = new EventEmitter()

  add(modal: any) {
    this.modals.push(modal);
  }

  remove(id: string = this.defaultModal) {
    this.modals = this.modals.filter((x) => x.id() !== id);
  }

  open(data?: any,id: string = this.defaultModal) {
    const modal: any = this.modals.find(x => x.id() === id);
    modal.open(data);
  }

  close(id: string = this.defaultModal) {
    const modal: any = this.modals.find(x => x.id() === id);
    modal.close();
  }
}
