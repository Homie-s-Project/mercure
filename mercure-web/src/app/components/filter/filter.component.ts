import {Component, Input} from '@angular/core';
import {faChevronUp} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Input() public filterName?: string;

  protected readonly faChevronUp = faChevronUp;

  public isFilterOpen: boolean = false;

  public toggleFilter(): void {
    this.isFilterOpen = !this.isFilterOpen;
  }
}
