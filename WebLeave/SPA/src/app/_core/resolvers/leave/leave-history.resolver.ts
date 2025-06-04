import { LeaveHistoryService } from '@services/leave/leave-history.service';
import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { LangConstants } from '../../constants/lang.constants';
import { LocalStorageConstants } from '../../constants/local-storage.enum';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({ providedIn: 'root' })
export class LeaveHistoryResolver implements Resolve<KeyValuePair[]> {
    lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;

    constructor(private leaveHistoryService: LeaveHistoryService) {
    }

    resolve(): Observable<KeyValuePair[]> {
        const search = this.leaveHistoryService.getCategory(this.lang);
        return search;
    }
}
