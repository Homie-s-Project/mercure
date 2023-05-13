import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError} from "rxjs";
import {SearchFilter} from "../../models/ISearchFilter";
import {IPaginationProductModel} from "../../models/IPaginationProductModel";
import {FilterService} from "../filter/filter.service";

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient, private filterService: FilterService) {
  }

  autocomplete(query: string) {
    return this.http.get<string[]>(environment.apiUrl + `/shopping/autocomplete?value=${query}`);
  }

  search(query: string, pageIndex?: number, pageSize = 10): Promise<IPaginationProductModel> {
    return new Promise<IPaginationProductModel>((resolve, reject) => {

      let queryStringGenerated = this.filterService.generateQueryString();

      this.http.get<IPaginationProductModel>(environment.apiUrl + `/shopping/search/${query}${queryStringGenerated.length != 0 ? `?${queryStringGenerated}` : '?'}${pageIndex != null ? `&pageIndex=${pageIndex}` : ''}${pageSize != null ? `&pageSize=${pageSize}` : ''}`)
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
