import {ICartProductModel} from "./ICartProductModel";

export interface ICartModel {
  userId: string;
  products: ICartProductModel[];
}
