import {ICategoriesModel} from "./ICategoriesModel";
import {IStockModel} from "./IStockModel";

export interface IProductModel {
  productId: number;
  productName: string;
  productBrandName: string;
  productDescription: string;
  productPrice: number;
  productType: string;
  productInfo: string;
  productCreationDate?: Date;
  productLastUpdate?: Date;
  stock?: IStockModel;
  categories?: ICategoriesModel[];
}

// productWeight pas encore implémenté dans le back-end
// productType pas encore implémenté dans le back-end et c'est enft une catégorie?
