import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError} from "rxjs";
import {SearchFilter} from "../../models/ISearchFilter";

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient) {
  }

  autocomplete(query: string) {
    return this.http.get<string[]>(environment.apiUrl + `/shopping/autocomplete?value=${query}`);
  }

  search(query: string, filter?: SearchFilter) {
    return new Promise((resolve, reject) => {

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

      this.http.get(environment.apiUrl + `/shopping/search/${query}${filter != null ? queryString : ''}`)
        .pipe(
          catchError((error) => {
            reject(error);
            return error;
          })
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }
}
