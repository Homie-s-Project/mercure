import { Component, Input } from '@angular/core';
import { faTag } from '@fortawesome/free-solid-svg-icons';
import {ICategoriesModel} from "../../models/ICategoriesModel";

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent {
 // Variable fontawesome
 faTag = faTag;

 @Input() categories?: ICategoriesModel[];
}
