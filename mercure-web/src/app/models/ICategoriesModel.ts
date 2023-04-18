import {IProductModel} from "./IProductModel";

export interface ICategoriesModel {
  categoryTitle: string;
  categoryDescription?: string;
  products?: IProductModel[];
}
