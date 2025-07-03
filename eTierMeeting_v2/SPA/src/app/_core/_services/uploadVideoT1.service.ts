import {  HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from '../../../environments/environment';
import { PaginatedResult } from "../_models/pagination";
import { eTM_Video } from "../_models/t1uploadVideo/eTM_Video";
import { batchDeleteParam, eTMVideoParam } from "../_models/t1uploadVideo/eTM_Video_Param";
import { TMVideoDto } from "../_models/t1uploadVideo/tm-video-dto";
import { UnitInfo } from "../_models/t1uploadVideo/unit-Info";
import { KeyValuePair } from "../_utilities/key-value-pair";

@Injectable({
  providedIn: 'root'
})
export class UploadVideoT1Service {
  baseUrl = environment.apiUrl; 
  constructor(private http: HttpClient) { }
  getListVideoKind() {
    return this.http.get<string[]>(this.baseUrl + 'uploadVideoT1/getAllVideoKind', {});
  }

  search(page?, itemsPerPage?, text?: eTMVideoParam): Observable<PaginatedResult<TMVideoDto[]>> {
    const paginatedResult: PaginatedResult<TMVideoDto[]> = new PaginatedResult<TMVideoDto[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.post<any>(this.baseUrl + 'uploadVideoT1/search/', text, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }),
      );
  }

  searchOfUser(page?, itemsPerPage?, text?: batchDeleteParam): Observable<PaginatedResult<eTM_Video[]>> {
    const paginatedResult: PaginatedResult<eTM_Video[]> = new PaginatedResult<eTM_Video[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.post<any>(this.baseUrl + 'uploadVideoT1/searchOfUser/', text, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }),
      );
  }

  delete(model: TMVideoDto) {
    return this.http.post<boolean>(this.baseUrl + 'uploadVideoT1/delete', model);
  }

  uploadVideo(data: any) {
    return this.http.post<any>(`${this.baseUrl}uploadVideoT1/upload`, data, {});
  }

  deleteAllBySearch(param: eTM_Video[]) {
    return this.http.post<boolean>(this.baseUrl + 'uploadVideoT1/deleteAllBySearch', param);
  }


  getCenters() {
    return this.http.get<string[]>(this.baseUrl + 'uploadVideoT1/getCenters');
  }
  getTiers() {
    return this.http.get<string[]>(this.baseUrl + 'uploadVideoT1/getTiers');
  }
  getSections() {
    return this.http.get<string[]>(this.baseUrl + 'uploadVideoT1/getSections');
  }
  getUnits() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'uploadVideoT1/getUnits');
  }
  getUnitsInParam(center: string, tier: string, section: string) {
    let params = new HttpParams().set('center', center)
                                  .set('tier', tier)
                                  .set('section', section);
    return this.http.get<UnitInfo[]>(this.baseUrl + 'uploadVideoT1/getUnitsInParam', {params});
  }
}
