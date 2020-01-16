import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'limitTo'
})
export class TruncatePipe {
  transform(value: string, args: string): string {
    value = value.replace(/(<\/?(?:a)[^>]*>)|<[^>]+>/ig, '$1');
    const limit = args ? parseInt(args, 10) : 10;
    const trail = '...';

    return value.length > limit ? value.substring(0, limit) + trail : value;
  }
}
