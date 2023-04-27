import {Component, Input, OnInit} from '@angular/core';
import {IPaginationProductModel} from "../../models/IPaginationProductModel";
import {environment} from "../../../environments/environment";
import {ProductService} from "../../services/product/product.service";

@Component({
  selector: 'app-products-pagination',
  templateUrl: './products-pagination.component.html',
  styleUrls: ['./products-pagination.component.scss']
})
export class ProductsPaginationComponent implements OnInit {

  isLoading = true;

  hasProductsError: boolean = false;
  productsPaginated?: IPaginationProductModel

  @Input() pageSize?: number = 8;
  pageIndex: number = 0;

  isLastPage = false;
  isFirstPage = true;

  constructor(private productService: ProductService) {
  }

  ngOnInit(): void {
    this.isLoading = true;

    this.productService.getProductsPagination(this.pageIndex, this.pageSize)
      .then(data => {
        this.productsPaginated = data;
      })
      .catch((error) => {
        this.hasProductsError = true;

        if (!environment.production) {
          console.error(error)
        }

        // Afficher une erreur avec un modal
        // this.modalService.open(error)
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

  nextPage() {
    if (this.productsPaginated && this.pageIndex < this.productsPaginated?.totalPages) {
      this.pageIndex++;
      this.ngOnInit();
    }

    if (this.pageIndex > 0) {
      this.isFirstPage = false;
    }

    if (this.pageIndex === this.productsPaginated?.totalPages) {
      this.isLastPage = true;
    }
  }

  previousPage() {
    if (this.productsPaginated && this.pageIndex <= this.productsPaginated?.totalPages && this.pageIndex >= 0) {
      this.pageIndex--;
      this.ngOnInit();
    }

    if (this.pageIndex === 0) {
      this.isFirstPage = true;
    }

    if (this.pageIndex === this.productsPaginated?.totalPages) {
      this.isLastPage = false;
    }

    if (this.productsPaginated && this.pageIndex < this.productsPaginated?.totalPages) {
      this.isLastPage = false;
    }
  }
}
