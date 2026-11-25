import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IApiResponse } from '../models/interfaces/api-response.model';

export abstract class GenericCrudService<T> {
  protected readonly http = inject(HttpClient);
  protected readonly resourceUrl: string;

  protected constructor(resource: string) {
    this.resourceUrl = `${environment.API_URL}/${resource}`;
  }

  getAll(): Observable<IApiResponse<T[]>> {
    return this.http.get<IApiResponse<T[]>>(this.resourceUrl);
  }

  getById(id: number): Observable<IApiResponse<T>> {
    return this.http.get<IApiResponse<T>>(`${this.resourceUrl}/${id}`);
  }

  create(payload: Partial<T>): Observable<IApiResponse<T>> {
    return this.http.post<IApiResponse<T>>(this.resourceUrl, payload);
  }

  update(id: number, payload: Partial<T>): Observable<IApiResponse<T>> {
    return this.http.put<IApiResponse<T>>(`${this.resourceUrl}/${id}`, payload);
  }

  delete(id: number): Observable<IApiResponse<null>> {
    return this.http.delete<IApiResponse<null>>(`${this.resourceUrl}/${id}`);
  }
}
