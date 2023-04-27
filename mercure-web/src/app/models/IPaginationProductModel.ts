import {IProductModel} from "./IProductModel";

export interface IPaginationProductModel {
  totalPages: number,
  totalProducts: number,
  currentPage: number,
  pageSize: number,
  products: IProductModel[]
}
