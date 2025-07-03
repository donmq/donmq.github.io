import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { HSEDataSearchDto, HSESearchParam } from '../../../../_models/hseResultUpload/hse-data';
import { ImageDataUpload, ImageRemark } from '../../../../_models/hseResultUpload/image-file-info';
import { PaginatedResult } from '../../../../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class HseResultUploadService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getBuildings() {
    return this.http.get<string[]>(this.apiUrl + 'HSEUResultUpload/getBuildings');
  }
  getDeptsInBuilding(building: string) {
    let params = new HttpParams().set('building', building);
    return this.http.get<string[]>(this.apiUrl + 'HSEUResultUpload/getDeptInBuilding', {params});
  }
  getTemplate() {
    return this.http.get(`${this.apiUrl}HSEUResultUpload/downLoadTemplateExcel`, {responseType: 'blob' });
  }
  uploadExcel(form: FormData) {
    return this.http.post<boolean>(this.apiUrl + 'HSEUResultUpload/uploadExcel', form);
  }
  
  search(page?, itemsPerPage?, text?: HSESearchParam): Observable<PaginatedResult<HSEDataSearchDto[]>> {
    const paginatedResult: PaginatedResult<HSEDataSearchDto[]> = new PaginatedResult<HSEDataSearchDto[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.post<any>(this.apiUrl + 'HSEUResultUpload/search/', text, { observe: 'response', params })
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
  removeHseScore(id: number) {
    return this.http.delete<boolean>(this.apiUrl + 'HSEUResultUpload/delete/' + id);
  }
  updateScoreData(model: HSEDataSearchDto) {
    return this.http.put<boolean>(this.apiUrl + 'HSEUResultUpload/updateScoreData', model);
  }
  uploadImages(data: ImageDataUpload) {
    const formData = new FormData();
    formData.append('HseID', `${data.hseID}`);
    data.images.forEach(item => {
      formData.append('Images', item);
    });
    data.remarks.forEach( item => {
      formData.append('Remarks', item);
    });
    return this.http.post<boolean>(this.apiUrl + 'HSEUResultUpload/uploadImages', formData);
  }

  editImages(data: ImageDataUpload) {
    const formData = new FormData();
    formData.append('HseID', `${data.hseID}`);
    data.images.forEach(item => {
      formData.append('Images', item);
    });
    data.remarks.forEach( item => {
      formData.append('Remarks', item);
    });
    return this.http.post<boolean>(this.apiUrl + 'HSEUResultUpload/editImages', formData);
  }
  
  getListImageToHseID(id: number) {
    return this.http.get<ImageRemark[]>(this.apiUrl + 'HSEUResultUpload/getListImageToHseID/' + id);
  }
}
