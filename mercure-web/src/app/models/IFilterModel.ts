import {IFilterValueModel} from "./IFilterValueModel";

export interface IFilterModel {
  filterCategory: string;
  filterCategoryBackend: string;
  filterValues: IFilterValueModel[];
}
