import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'ulink'
})
export class LinkPipe implements PipeTransform {
  transform(value: string): string {
    if (typeof value === 'string') {
      return value;
    }

    return '/';
  }
}
