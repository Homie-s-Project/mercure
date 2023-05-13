import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {IFilterModel} from "../../models/IFilterModel";
import {FilterService} from "../../services/filter/filter.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-filter-group',
  templateUrl: './filter-group.component.html',
  styleUrls: ['./filter-group.component.scss']
})
export class FilterGroupComponent implements OnInit {

  @Output() refresh: EventEmitter<any> = new EventEmitter<any>();

  filters?: IFilterModel[];
  isLoading: boolean = true;

  constructor(private filterService: FilterService) {
    this.filters = this.filterService.filter;
  }

  async ngOnInit(): Promise<void> {
    this.filterService.getFilter()
      .then((data) => {
        if (!environment.production) {
          console.log("Filter loaded");
          console.log(data);
        }
      })
      .catch((err) => {
        if (!environment.production) {
          console.log(err);
        }
      })
      .finally(() => {
        this.isLoading = false;
      });
  }

  refreshSearch() {
    this.refresh.emit();
  }
}
