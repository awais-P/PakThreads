import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(tags: string[], searchText: string): string[] {
    if (!searchText) return tags;
    return tags.filter(tag => tag.toLowerCase().includes(searchText.toLowerCase()));
  }
}
