import {IFilterValueModel} from "./IFilterValueModel";

export interface IFilterModel {
  filterCategory: string;
  filterCategoryBackend: string;
  filterType: "select" | "range";
  filterValues: IFilterValueModel[];
  filterRangeMin?: number;
  filterRangeMax?: number;
}
