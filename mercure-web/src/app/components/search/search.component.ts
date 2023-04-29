import {Component, Input, OnInit} from '@angular/core';
import {IPaginationProductModel} from "../../models/IPaginationProductModel";
import {environment} from "../../../environments/environment";
import {SearchService} from "../../services/search/search.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  isLoading = true;

  hasProductsError: boolean = false;
  productsPaginated?: IPaginationProductModel

  @Input() pageSize?: number = 8;
  pageIndex: number = 0;

  isLastPage = false;
  isFirstPage = true;
  search: string = "";

  constructor(private searchService: SearchService, private route: ActivatedRoute, private router: Router) {
    this.search = this.route.snapshot.paramMap.get('searchValue') ?? '';

    if (!this.search) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.isLoading = true;

    this.searchService.search(this.search, undefined, this.pageIndex)
      .then((data) => {
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
        this.checkIfFirstPage();
        this.checkIfLastPage();
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

  checkIfLastPage() {
    if (this.productsPaginated && (this.pageIndex+1) === this.productsPaginated?.totalPages) {
      this.isLastPage = true;
    }
  }

  checkIfFirstPage() {
    if (this.pageIndex === 0) {
      this.isFirstPage = true;
    }

    if (this.pageIndex > 0) {
      this.isFirstPage = false;
    }
  }
}
