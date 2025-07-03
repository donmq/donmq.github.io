import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { OperationResult } from '../_utilities/operation-result';
import { eTM_Meeting_Log } from '../_models/eTM_Meeting_Log';
import { Observable } from 'rxjs';
import { Injectable, EventEmitter, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RecordMeetingDurationService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  @Output() t2StartRecordEvent = new EventEmitter<string>();

  create(deptId: string, classType: string, tierLevel: string) {
    return this.http.post<OperationResult>(`${this.baseUrl}RecordMeetingDuration/Create?deptId=${deptId}&classType=${classType}&tierLevel=${tierLevel}`, {});
  }

  update(eTM_Meeting_Log: eTM_Meeting_Log) {
    return this.http.put<boolean>(`${this.baseUrl}RecordMeetingDuration/Update`, eTM_Meeting_Log);
  }

  get(record_ID: string): Observable<eTM_Meeting_Log> {
    return this.http.get<eTM_Meeting_Log>(`${this.baseUrl}RecordMeetingDuration/MeetingLog`, { params: { record_ID } });
  }

  t2StartRecord(deptId: string, class_Level: string){
    this.t2StartRecordEvent.emit(deptId);
  }
}
