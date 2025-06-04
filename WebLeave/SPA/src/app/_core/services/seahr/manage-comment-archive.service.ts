import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Pagination } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { CommentArchive } from '../../models/common/comment-archive';

@Injectable({ providedIn: 'root' })
export class ManageCommentArchiveService {
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient
  ) { }

  getAll(pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<CommentArchive>(this.apiUrl + 'ManageComment/Search', { params });
  }
  
  addComment(comment: CommentArchive) {
    return this.http.post<OperationResult>(this.apiUrl + 'ManageComment/Add', comment);
  }

  deleteComment( id: number) {
    return this.http.delete<OperationResult>(this.apiUrl + `ManageComment/Delete?commentArchiveID=${id}`);
  } 
}