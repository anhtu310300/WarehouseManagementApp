import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_Response } from '../models/interfaces/common.model';

export abstract class GenericCrudService<T> {
  protected readonly http = inject(HttpClient);
  protected readonly resourceUrl: string;

  protected constructor(resource: string) {
    this.resourceUrl = `${environment.API_URL}/${resource}`;
  }

  getAll(): Observable<API_Response<T[]>> {
    return this.http.get<API_Response<T[]>>(this.resourceUrl);
  }

  getById(id: number): Observable<API_Response<T>> {
    return this.http.get<API_Response<T>>(`${this.resourceUrl}/${id}`);
  }

  create(payload: Partial<T>): Observable<API_Response<T>> {
    return this.http.post<API_Response<T>>(this.resourceUrl, payload);
  }

  update(id: number, payload: Partial<T>): Observable<API_Response<T>> {
    return this.http.put<API_Response<T>>(`${this.resourceUrl}/${id}`, payload);
  }

  delete(id: number): Observable<API_Response<null>> {
    return this.http.delete<API_Response<null>>(`${this.resourceUrl}/${id}`);
  }
}
