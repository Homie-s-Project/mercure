import {Injectable} from '@angular/core';
import {IFilterModel} from "../../models/IFilterModel";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {catchError} from "rxjs";
import {IFilterValueModel} from "../../models/IFilterValueModel";

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  filter: IFilterModel[] = []

  constructor(private http: HttpClient) {
  }

  getFilter() {
    return Promise.all([this.getBrands(), this.getCategories()])
      .then((data) => {
        let brands = data[0];
        let filterValuesBrands: IFilterValueModel[] = [];

        brands.forEach((brand) => {
          filterValuesBrands.push({
            name: brand,
            value: this.normalizeString(brand),
            checked: false
          });
        });

        this.filter.push({
          filterCategory: 'Marque',
          filterCategoryBackend: 'brand',
          filterValues: filterValuesBrands
        });

        let categories = data[1];
        let filterValuesCategories: IFilterValueModel[] = [];
        categories.forEach((category) => {
          filterValuesCategories.push({
            name: category,
            value: this.normalizeString(category),
            checked: false
          });
        });

        this.filter.push({
          filterCategory: 'Catégorie',
          filterCategoryBackend: 'category',
          filterValues: filterValuesCategories
        });

        return this.filter;
      })
      .catch((err) => {
        if (!environment.production) {
          console.log(err);
        }
      });
  }

  getBrands(): Promise<string[]> {
    return new Promise<any>((resolve, reject) => {
      this.http.get<string[]>(environment.apiUrl + '/shopping/brands')
        .pipe(
          catchError(
            (err) => {
              reject(err);
              return err;
            }
          )
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }

  getCategories(): Promise<string[]> {
    return new Promise<any>((resolve, reject) => {
      this.http.get<string[]>(environment.apiUrl + '/shopping/categories')
        .pipe(
          catchError(
            (err) => {
              reject(err);
              return err;
            }
          )
        )
        .subscribe((data) => {
          resolve(data);
          return data;
        })
    });
  }

  setFilterValue(filterCategory: string, filterName: string, filterValue: string, isChecked: boolean): void {
    let category = this.filter.find((category) => {
      return category.filterCategory === filterCategory;
    });

    if (category) {
      let filter = category.filterValues.find((filter) => {
        return filter.name === filterName;
      });

      if (filter) {
        filter.checked = isChecked;
      }
    }
  };

  generateQueryString(): string {
    let queryString = '';
    this.filter.forEach((category) => {
      category.filterValues.forEach((filter) => {
        if (filter.checked) {
          queryString += `${category.filterCategoryBackend}=${filter.value}&`;
        }
      });
    });

    return queryString;
  }

  // Normalize string to remove accents and special characters
  private normalizeString(str: string): string {
    let normalized = str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    normalized = normalized.replace(/[^a-zA-Z0-9]/g, '-');
    return normalized.toLowerCase();
  }
}
