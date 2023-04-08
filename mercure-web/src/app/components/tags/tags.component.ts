import { Component, Input } from '@angular/core';
import { faTag } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent {
 // Variable fontawesome
 faTag = faTag;

 @Input() tags?: string[];
}
