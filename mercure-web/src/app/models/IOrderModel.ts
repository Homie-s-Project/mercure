import {IProductModel} from "./IProductModel";

export interface IOrderModel {
    orderNumber: number;
    orderStatus: boolean;
    orderPrice: number;
    orderDate: Date;
    products: IProductModel[];
}
