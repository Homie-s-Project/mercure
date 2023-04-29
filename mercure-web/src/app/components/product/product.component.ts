import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  faCartPlus,
  faCaretRight,
  faCaretDown,
  faChevronUp,
  faChevronDown,
  faFaceFrown,
  faCaretLeft
} from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { IProductModel } from 'src/app/models/IProductModel';
import { ProductService } from 'src/app/services/product/product.service';
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  animations: [
    trigger('collapse', [
      state('false', style({ height: AUTO_STYLE, visibility: AUTO_STYLE })),
      state('true', style({ height: '0', visibility: 'hidden' })),
      transition('false => true', animate(250 + 'ms ease-in')),
      transition('true => false', animate(250 + 'ms ease-out'))
    ])
  ]
})
export class ProductComponent implements OnInit {
  // Variable Fontawesome
  faCartPlus = faCartPlus;
  faCaret = faCaretDown;
  faChevronUp = faChevronUp;
  faCaretLeft = faCaretLeft;
  faCaretRight = faCaretRight;
  faChevronDown = faChevronDown;
  faFaceFrown = faFaceFrown;

  collapsed: boolean = false;
  currentId?: number;
  currentProduct?: IProductModel;

  notFound: boolean = false;

  constructor (public productService: ProductService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit(): Promise<void> {
    var productRoute = this.route.snapshot.paramMap.get('productId');

    if (productRoute == undefined || null) {
      this.currentProduct = undefined;
      return;
    } else {
      this.currentId = +productRoute;
    }

    this.productService.getProductById(this.currentId)
      .then((product) => {
        this.currentProduct = product;
      })
      .catch((error) => {
        if (error.status == 404) {
          this.notFound = true;
        } else {
          this.router.navigate(['/']);
        }

        if (!environment.production) {
          console.error(error);
        }
      });
  }

  toggleCollapse() {
    this.collapsed = !this.collapsed;

    console.log(this.collapsed);

    if (this.collapsed)
      this.faCaret = faCaretRight;
    else
      this.faCaret = faCaretDown;
  }
}
