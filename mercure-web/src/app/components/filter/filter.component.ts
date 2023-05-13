import {Component, EventEmitter, Input, Output} from '@angular/core';
import {faChevronUp} from "@fortawesome/free-solid-svg-icons";
import {IFilterValueModel} from "../../models/IFilterValueModel";
import {FilterService} from "../../services/filter/filter.service";
import {IFilterModel} from "../../models/IFilterModel";
import {Options} from "@angular-slider/ngx-slider";

@Component({
  selector: 'app-filter[filter]',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent {
  @Output() refresh: EventEmitter<any> = new EventEmitter<any>();

  @Input() filter!: IFilterModel;

  hasChanged: boolean = false;
  isFilterOpen: boolean = false;
  sliderOption: Options = {
    floor: 10,
    ceil: 1000,
    step: 5,
    animate: false,
  }

  sliderMinValue: number = 10;
  sliderMaxValue: number = 1000;


  protected readonly faChevronUp = faChevronUp;

  constructor(private filterService: FilterService) {
  }

  public toggleFilter(): void {
    this.isFilterOpen = !this.isFilterOpen;

    if (this.filter.filterType === 'range') {
      this.sliderOption.floor = this.filter.filterRangeMin;
      this.sliderOption.ceil = this.filter.filterRangeMin;
    }
  }

  public setFilterValue(event: Event, category: string, value: IFilterValueModel): void {
    let isChecked = (event.target as HTMLInputElement).checked;
    this.filterService.setFilterValue(category, value.name, value.value, isChecked);
    this.hasChanged = true;
  }

  countSelectedFilterValue(category: string) {
    return this.filterService.getSelectedCountFilterValue(category);
  }

  closeOrSave() {
    this.toggleFilter();
    this.refresh.emit();
  }

  changeRange(category: string) {

    this.filterService.setRangeValue(category, this.sliderMinValue, this.sliderMaxValue);
    this.hasChanged = true;
  }
}
