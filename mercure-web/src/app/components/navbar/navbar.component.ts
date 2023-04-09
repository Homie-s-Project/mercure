import {Component, EventEmitter, AfterViewInit, Output} from '@angular/core';
import {faCartShopping, faGlobe, faMagnifyingGlass, faUser} from '@fortawesome/free-solid-svg-icons';
import {SearchService} from "../../services/search/search.service";
import {debounceTime, Subject, Subscription} from "rxjs";
import {Router} from "@angular/router";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements AfterViewInit {
  faCartShopping = faCartShopping
  faGlobe = faGlobe;
  faUser = faUser;
  faMagnifyingGlass = faMagnifyingGlass;
  @Output() hideShow = new EventEmitter<{ showHide: boolean }>();
  showHide = true;

  searchValueChanged: Subject<string> = new Subject<string>();
  autocompleteSuggestions: string[] = [];
  searchValue: string = '';
  active: boolean = false;
  private searchValueSubscription?: Subscription;
  private autocompleteDelay: number = 500;
  private autocompleteSubscription: any;

  constructor(private searchService: SearchService, private router: Router) {
  }

  ngAfterViewInit(): void {
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
      })
  }

  onChangeInput(text: any) {
    this.searchValueChanged.next(text.value);
  }

  search() {
    this.searchService.search(this.searchValue)
      .then((res) => {
        if (!environment.production) {
          console.log(res);
        }
      })
      .catch((err) => {
        if (!environment.production) {
          console.error(err);
        }
      });
  }

  autocomplete() {
    this.autocompleteSubscription = this.searchService.autocomplete(this.searchValue)
      .subscribe((res) => {
        this.autocompleteSuggestions = res;
      });
  }

  toggleshowHide() {
  }

  goToSearch() {
    this.router.navigate(['/search'], {queryParams: {q: this.searchValue.trim()}});
  }
}
