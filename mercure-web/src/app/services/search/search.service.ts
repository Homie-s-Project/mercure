import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
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

  search(query: string, pageIndex?: number, pageSize = 10): Observable<IPaginationProductModel> {

    let queryStringGenerated = this.filterService.generateQueryString();

    return this.http.get<IPaginationProductModel>(
      environment.apiUrl +
      `/shopping/search/${query}
        ${queryStringGenerated.length != 0 ? `?${queryStringGenerated}` : '?'}
        ${pageIndex != null ? `&pageIndex=
        ${pageIndex}` : ''}${pageSize != null ? `&pageSize=${pageSize}` : ''}`);
  }
}
