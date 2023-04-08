import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
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
export class ProductComponent implements OnInit, OnDestroy {
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
    this.subscriptions.push(
      this.route.paramMap.subscribe((map: ParamMap) => {
        this.currentId = parseInt(map.get('productId') || '');
      })
    );

    console.log(this.currentId);

    if (this.currentId == undefined) {
      this.currentProduct = undefined;
      return;
    }

    this.currentProduct = this.product.getProductById(this.currentId);

    console.log(this.currentProduct)
  }

  ngOnDestroy(): void {
    for (let subscription of this.subscriptions) {
      subscription.unsubscribe();
    }
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
