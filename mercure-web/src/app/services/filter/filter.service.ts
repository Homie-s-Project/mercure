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
    this.filter.push({
      filterCategory: 'Prix',
      filterCategoryBackend: 'price',
      filterType: 'range',
      filterValues: [],
      filterRangeMax: 1000,
      filterRangeMin: 10,
    });

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
          filterType: 'select',
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
          filterType: 'select',
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

    // filtre avec données de type select
    this.filter.filter(f => f.filterCategory == 'select').forEach((category) => {

      let categoryQueryString: string[] = [];
      category.filterValues.forEach((filter) => {

        if (filter.checked) {
          categoryQueryString.push(filter.value);
        }
      });

      if (categoryQueryString.length > 0) {
        queryString += `${category.filterCategoryBackend}=${categoryQueryString.join(',')}&`;
      }
    });

    // filtre avec données de type range
    this.filter

    return queryString;
  }

  // Normalize string to remove accents and special characters
  private normalizeString(str: string): string {
    let normalized = str.replace(/[^a-z0-9]/gi, '-').toLowerCase();

    // Remove multiple dashes, bug with the regex not the same as the one in the back
    if (normalized.includes("---")) {
      normalized = normalized.replace("---", "-");
    }

    // La normalisation ne peut pas marcher à cause de la regex du back
    return str;
  }

  getSelectedCountFilterValue(category: string) {
    let count = 0;
    let filter = this.filter.find((filter) => {
      return filter.filterCategory === category;
    });

    if (filter) {
      filter.filterValues.forEach((filterValue) => {
        if (filterValue.checked) {
          count++;
        }
      });
    }

    return count;
  }

  setRangeValue(category: string, sliderMinValue: number, sliderMaxValue: number) {
    let filter = this.filter.find((filter) => {
      return filter.filterCategory === category;
    });

    if (filter) {
      filter.filterRangeMin = sliderMinValue;
      filter.filterRangeMax = sliderMaxValue;
    }
  }
}
