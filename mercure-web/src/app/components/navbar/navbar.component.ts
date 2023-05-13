import {AfterViewInit, Component, ElementRef, EventEmitter, Output, ViewChild} from '@angular/core';
import {faCartShopping, faMagnifyingGlass, faUser, faXmark} from '@fortawesome/free-solid-svg-icons';
import {SearchService} from "../../services/search/search.service";
import {debounceTime, Subject, Subscription} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements AfterViewInit {
  subscriptions: Subscription[] = []

  faCartShopping = faCartShopping
  faUser = faUser;
  faXmark = faXmark;
  actualIcone = this.faCartShopping;
  faMagnifyingGlass = faMagnifyingGlass;
  @Output() toggleHide = new EventEmitter<boolean>();
  hideCart: boolean = true
  isRedirecting: boolean = false;

  searchValueChanged: Subject<string> = new Subject<string>();
  autocompleteSuggestions: string[] = [];
  searchValue: string = '';
  active: boolean = false;
  @ViewChild('autocomplete') autocompleteElement: ElementRef | undefined;
  private searchValueSubscription?: Subscription;
  private autocompleteDelay: number = 750;
  private autocompleteSubscription: any;

  constructor(private searchService: SearchService, private router: Router, private route: ActivatedRoute) {
    router.events.subscribe((val) => {
      // Permet de cacher le panier lorsqu'on change de page
      this.hideCart = true;
      this.toggleHide.emit(this.hideCart);
      this.actualIcone = this.faCartShopping;

      // Efface les suggestions de recherche
      this.autocompleteSuggestions = [];
    });
  }

  ngAfterViewInit(): void {
    this.isRedirecting = false;
    this.searchValueSubscription = this.searchValueChanged
      .pipe(
        debounceTime(this.autocompleteDelay),
      )
      .subscribe((value) => {
        this.searchValue = value;
        if (this.searchValue.length > 0) {
          this.autocomplete();
        } else {
          this.autocompleteSuggestions = [];
        }
      });

    this.subscriptions.push(
      this.route.queryParams
        .subscribe(params => {
          let search = params['q'];

          if (search && this.searchValue.length === 0) {
            this.searchValue = search;
          }
        })
    );
  }

  onChangeInput(text: any) {
    this.searchValueChanged.next(text.value);
  }

  autocomplete() {
    this.autocompleteSubscription = this.searchService.autocomplete(this.searchValue)
      .subscribe((res) => {
        this.autocompleteSuggestions = res;
      });
  }

  toggleShowHide() {
    this.actualIcone = !this.hideCart ? this.faCartShopping : this.faXmark;
    this.hideCart = !this.hideCart;
    this.toggleHide.emit(this.hideCart);
  }

  goToSearch() {
    // Permet de unfocus l'input
    this.autocompleteElement?.nativeElement.blur();

    // Prevent multiple redirections and avoid page deplucation
    if (!this.isRedirecting) {
      this.isRedirecting = true;
      this.router.navigate(['/search'], {queryParams: {q: this.searchValue.trim()}});
    }
  }
}
