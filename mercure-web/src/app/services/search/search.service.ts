import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError} from "rxjs";
import {SearchFilter} from "../../models/ISearchFilter";
import {IPaginationProductModel} from "../../models/IPaginationProductModel";

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient) {
  }

  autocomplete(query: string) {
    return this.http.get<string[]>(environment.apiUrl + `/shopping/autocomplete?value=${query}`);
  }

  search(query: string, filter?: SearchFilter, pageIndex?: number, pageSize = 10): Promise<IPaginationProductModel> {
    return new Promise<IPaginationProductModel>((resolve, reject) => {

      let queryString = '';
      if (filter) {
        queryString = `?`;
        if (filter.brand) {
          queryString += `brand=${filter.brand}&`;
        }
        if (filter.category) {
          queryString += `category=${filter.category}&`;
        }
        if (filter.minPrice) {
          queryString += `minPrice=${filter.minPrice}&`;
        }
        if (filter.maxPrice) {
          queryString += `maxPrice=${filter.maxPrice}&`;
        }
      }

      this.http.get<IPaginationProductModel>(environment.apiUrl + `/shopping/search/${query}${filter != null ? queryString : '?'}${pageIndex != null ? `&pageIndex=${pageIndex}` : ''}${pageSize != null ? `&pageSize=${pageSize}` : ''}`)
        .pipe(
          catchError((error) => {
            reject(error);
            return error;
          })
        )
        .subscribe((data) => {
          // @ts-ignore
          resolve(data);
          return data;
        })
    });
  }
}
