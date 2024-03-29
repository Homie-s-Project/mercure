import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from '@angular/core';
import {IPaginationProductModel} from "../../models/IPaginationProductModel";
import {environment} from "../../../environments/environment";
import {SearchService} from "../../services/search/search.service";
import {ActivatedRoute, Router} from "@angular/router";
import {catchError, finalize, interval, Subscription} from "rxjs";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, OnDestroy, OnChanges {
  subscriptions: Subscription[] = []

  isLoading = true;

  hasProductsError: boolean = false;
  productsPaginated?: IPaginationProductModel

  @Input() pageSize?: number = 8;
  pageIndex: number = 0;

  isLastPage = false;
  isFirstPage = true;
  search: string = "";
  messageError: string = "";
  subscriptionErrorTimer!: Subscription;
  subscriptionSearch!: Subscription;
  noProductFound: boolean = false

  // in seconds
  private TIME_BEFORE_REDIRECT: number = 10;
  secondBeforeRedirect: number = this.TIME_BEFORE_REDIRECT;

  constructor(private searchService: SearchService, private route: ActivatedRoute, private router: Router) {
    this.subscriptions.push(
      this.route.queryParams
        .subscribe(params => {
          this.search = params['q'].trim();

          if (!this.search) {
            this.router.navigate(['/']);
          }

          if (!this.isLoading) {
            this.searchProduct();
          }
        })
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.search) {
      this.router.navigate(['/']);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
    if (this.hasProductsError) {
      this.subscriptionErrorTimer.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.searchProduct();
  }

  searchProduct() {
    // évite de faire plusieurs requêtes en même temps
    this.subscriptionSearch?.unsubscribe();

    this.subscriptionSearch = this.searchService.search(this.search, this.pageIndex)
      .pipe(
        // @ts-ignore
        catchError((err) => {
          if (!environment.production) {
            console.error(err)
          }

          if (err.status === 404) {
            this.noProductFound = true;
            this.productsPaginated = undefined;
          }


          if (err.status !== 404) {
            this.hasProductsError = true;

            this.secondBeforeRedirect = 10;

            this.subscriptionErrorTimer = interval(1000)
              .subscribe(x => {
                this.secondBeforeRedirect -= 1;
              })

            setTimeout(() => {
              this.router.navigate(["/"]);
            }, this.TIME_BEFORE_REDIRECT * 1_000);

            console.log(err)
            this.messageError = err.error.message;

            return new Error(err.error.message)
          }
        }),
        finalize(() => {
          this.isLoading = false;
          this.checkIfFirstPage();
          this.checkIfLastPage();
        })
      )
      .subscribe((data) => {
        this.productsPaginated = data;
        this.noProductFound = false;
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
    if (this.productsPaginated && (this.pageIndex + 1) === this.productsPaginated?.totalPages) {
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
