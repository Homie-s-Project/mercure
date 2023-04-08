import { Component, Input, OnInit } from '@angular/core';
import { faTag, faCartPlus } from '@fortawesome/free-solid-svg-icons';
import { IProductModel } from 'src/app/models/IProductModel';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {
  faTag = faTag;
  faCartPlus = faCartPlus;

  @Input() product?: IProductModel;

  constructor() { }

  ngOnInit(): void {
  }

}
