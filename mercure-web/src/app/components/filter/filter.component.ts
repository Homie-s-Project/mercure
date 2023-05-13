import {Component, EventEmitter, Input, Output} from '@angular/core';
import {faChevronUp} from "@fortawesome/free-solid-svg-icons";
import {IFilterValueModel} from "../../models/IFilterValueModel";
import {FilterService} from "../../services/filter/filter.service";

@Component({
  selector: 'app-filter[category][filterValue]',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Output() refresh: EventEmitter<any> = new EventEmitter<any>();
  @Input() category!: string;
  @Input() filterValue!: IFilterValueModel[];

  hasChanged: boolean = false;
  isFilterOpen: boolean = false;

  protected readonly faChevronUp = faChevronUp;

  constructor(private filterService: FilterService) {
  }

  public toggleFilter(): void {
    this.isFilterOpen = !this.isFilterOpen;
  }

  public setFilterValue(event: Event, category: string, value: IFilterValueModel): void {
    let isChecked = (event.target as HTMLInputElement).checked;
    this.filterService.setFilterValue(category, value.name, value.value, isChecked);
    this.hasChanged = true;
  }

  closeOrSave() {
    this.toggleFilter();
    this.refresh.emit();
  }
}
