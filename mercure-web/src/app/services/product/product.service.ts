import { Injectable } from '@angular/core';
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
      Info: "500 g",
      Tags: ["Nourriture", "Chat", "Premium"]
    },
    {
      Id: 2,
      Brand: "Chat Royal",
      Type: "Croquette pour chat",
      Description: "Les croquettes pour chat Chat Royal sont élaborées à partir d'ingrédients sélectionnés avec soin pour répondre aux besoins nutritionnels de votre félin. Riches en protéines, vitamines et minéraux, elles contribuent à maintenir une bonne santé générale, une belle fourrure et une digestion optimale. Leur texture croquante favorise également l'hygiène dentaire. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chat en fonction de son poids et de son âge.",
      Price: 29.90,
      Info: "750 g",
      Tags: ["Nourriture", "Chat", "Haut de gamme"]
    },
    {
      Id: 3,
      Brand: "Mercure Canina",
      Type: "Croquette pour chien",
      Description: "Les croquettes pour chien Mercure Canina sont élaborées avec des ingrédients de qualité supérieure pour fournir une alimentation équilibrée à votre animal. Elles contiennent des protéines, des vitamines et des minéraux pour soutenir la santé générale, la peau et le pelage de votre chien, ainsi que sa digestion. Leur texture croquante favorise également l'hygiène dentaire. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chien en fonction de son poids et de son âge.",
      Price: 45.90,
      Info: "1200 g",
      Tags: ["Nourriture", "Chien", "Premium", "Eco-responsable"]
    },
    {
      Id: 4,
      Brand: "Purina One",
      Type: "Croquettes pour chat adulte",
      Description: "Les croquettes pour chat adulte Purina One sont élaborées à partir d'ingrédients de haute qualité pour répondre aux besoins nutritionnels spécifiques des chats adultes. Elles sont riches en protéines pour maintenir la masse musculaire, en antioxydants pour soutenir le système immunitaire et en acides gras essentiels pour favoriser une peau saine et un pelage brillant. Leur texture croquante aide à maintenir une bonne hygiène dentaire en réduisant la formation de tartre. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chat en fonction de son poids et de son âge.",
      Price: 22.90,
      Info: "1,5 kg",
      Tags: ["Nourriture", "Chat", "Premium"]
    },
    {
      Id: 5,
      Brand: "Hill's Science Plan",
      Type: "Croquettes pour chat stérilisé",
      Description: "Les croquettes pour chat stérilisé Hill's Science Plan sont spécialement formulées pour répondre aux besoins nutritionnels des chats stérilisés ou castrés. Elles sont riches en protéines pour maintenir la masse musculaire et en fibres pour favoriser la digestion. Elles contiennent également des antioxydants pour soutenir le système immunitaire et des acides gras essentiels pour favoriser une peau saine et un pelage brillant. Leur texture croquante aide à maintenir une bonne hygiène dentaire en réduisant la formation de tartre. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chat en fonction de son poids et de son âge.",
      Price: 27.90,
      Info: "1,5 kg",
      Tags: ["Nourriture", "Chat", "Premium", "Stérilisé"]
    },
    {
      Id: 6,
      Brand: "Hill's Science Diet",
      Type: "Croquettes pour chien adulte",
      Description: "Les croquettes pour chien adulte Hill's Science Diet sont formulées pour répondre aux besoins nutritionnels spécifiques des chiens adultes. Elles sont riches en protéines de haute qualité pour maintenir la masse musculaire, en antioxydants pour soutenir le système immunitaire, en acides gras essentiels pour favoriser une peau saine et un pelage brillant, et en fibres pour favoriser une digestion saine. Leur texture croquante aide à maintenir une bonne hygiène dentaire en réduisant la formation de tartre. Le paquet est refermable pour conserver la fraîcheur des croquettes et les instructions d'alimentation sont clairement indiquées pour vous aider à nourrir votre chien en fonction de son poids et de son âge. Offrez à votre chien une alimentation saine et délicieuse avec les croquettes Hill's Science Diet !",
      Price: 39.90,
      Info: "7 kg",
      Tags: ["Nourriture", "Chien", "Premium"]
    },
    {
      Id: 7,
      Brand: "Cat's Best",
      Type: "Litière pour chat",
      Description: "La litière pour chat Cat's Best est faite à partir de granulés de bois naturels, sans additifs chimiques ni parfums artificiels. Elle est très absorbante et retient les odeurs efficacement, pour offrir un environnement de vie sain et agréable à votre chat. Elle est également très économique, car elle ne nécessite pas de changement complet de litière aussi souvent que les autres litières pour chat. Le paquet est refermable pour conserver la fraîcheur de la litière et faciliter son transport. La litière est facile à nettoyer et peut être jetée dans les toilettes pour plus de praticité. Offrez à votre chat une litière naturelle et efficace avec Cat's Best !",
      Price: 14.90,
      Info: "8 L",
      Tags: ["Litière", "Chat", "Naturelle"]
    },
    {
      Id: 8,
      Brand: "Ruffwear",
      Type: "Harnais pour chien",
      Description: "Le harnais pour chien Ruffwear est un équipement de haute qualité, conçu pour offrir un maximum de confort et de sécurité à votre chien lors de ses sorties. Il est fabriqué à partir de matériaux résistants et durables, et dispose de sangles rembourrées pour éviter les frottements et les irritations. Le harnais est facile à mettre et à enlever, grâce à sa boucle à dégagement rapide. Il est également doté d'un point d'attache pour la laisse sur le dos, afin de répartir la pression de manière équilibrée sur tout le corps du chien. Le harnais est disponible en plusieurs tailles pour convenir à tous les chiens. Offrez à votre chien un équipement de qualité pour des sorties en toute sécurité avec le harnais Ruffwear !",
      Price: 49.95,
      Info: "M",
      Tags: ["Accessoires", "Chien", "Harnais"]
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
