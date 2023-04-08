import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faCartPlus, faCaretRight, faCaretDown, faChevronUp, faChevronDown, faFaceFrown } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { IProductModel } from 'src/app/models/IProductModel';
import { ProductService } from 'src/app/services/product/product.service';

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
  faChevronDown = faChevronDown;
  faFaceFrown = faFaceFrown;

  collapsed: Boolean = false;
  currentId?: number; 
  currentProduct?: IProductModel;
  subscriptions: Subscription[] = []

  constructor (public product: ProductService, private route: ActivatedRoute) { }
  
  ngOnInit(): void {
    // this.subscriptions.push(
    //   this.route.paramMap.subscribe((map: ParamMap) => {
    //     this.currentId = parseInt(map.get('productId') || '');
    //   })
    // );

    var productRoute = this.route.snapshot.paramMap.get('productId');

    if (productRoute == undefined || null) {
      this.currentProduct = undefined;
      return;
    } else {
      this.currentId = +productRoute;
    }

    this.currentProduct = this.product.getProductById(this.currentId);
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
