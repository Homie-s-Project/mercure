import { Injectable } from '@angular/core';
import { UnionToIntersection } from 'chart.js/types/utils';
import { IProductModel } from 'src/app/models/IProductModel';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private products: IProductModel[] = [
    {
      Id: 1,
      Brand: "Mercure Felina",
      Type: "Croquette pour chat",
      Description: "Le paquet de croquettes pour chat Mercure Felina est un aliment savoureux et équilibré, conçu pour répondre aux besoins nutritionnels spécifiques de votre félin. Les croquettes sont élaborées à partir d'ingrédients de haute qualité, soigneusement sélectionnés pour assurer une alimentation saine et équilibrée à votre chat. Chaque croquette est riche en protéines, vitamines et minéraux, pour aider à maintenir une peau saine, un pelage brillant et une digestion optimale. Leur texture croquante favorise également l'hygiène dentaire en aidant à prévenir la formation de tartre. Le paquet est conçu pour être pratique et facile à utiliser. Il est refermable pour conserver la fraîcheur des croquettes et permettre une utilisation prolongée. Les instructions d'alimentation sont clairement indiquées sur l'emballage pour vous aider à nourrir votre chat avec la bonne quantité de nourriture en fonction de son poids et de son âge. Offrez à votre chat une alimentation saine et délicieuse avec les croquettes Mercure Felina !",
      Price: 24.50,
      Quantity: 500,
      Tags: ["Nourriture", "Chat", "Premium"]
    },
    {
      Id: 2,
      Brand: "Chat Royal",
      Type: "Croquette pour chat",
      Description: "Les croquettes pour chat Chat Royal sont élaborées à partir d'ingrédients sélectionnés avec soin pour répondre aux besoins nutritionnels de votre félin. Riches en protéines, vitamines et minéraux, elles contribuent à maintenir une bonne santé générale, une belle fourrure et une digestion optimale. Leur texture croquante favorise également l'hygiène dentaire. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chat en fonction de son poids et de son âge.",
      Price: 29.90,
      Quantity: 750,
      Tags: ["Nourriture", "Chat", "Haut de gamme"]
    },
    {
      Id: 3,
      Brand: "Mercure Canina",
      Type: "Croquette pour chien",
      Description: "Les croquettes pour chien Mercure Canina sont élaborées avec des ingrédients de qualité supérieure pour fournir une alimentation équilibrée à votre animal. Elles contiennent des protéines, des vitamines et des minéraux pour soutenir la santé générale, la peau et le pelage de votre chien, ainsi que sa digestion. Leur texture croquante favorise également l'hygiène dentaire. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chien en fonction de son poids et de son âge.",
      Price: 45.90,
      Quantity: 1200,
      Tags: ["Nourriture", "Chien", "Premium", "Eco-responsable"]
    }
  ]

  constructor() { }

  getProducts(): IProductModel[] {
    return this.products;
  }

  getProductById(id: number): IProductModel|undefined {
    return this.products.find((product: IProductModel) => product.Id == id);
  }
}
