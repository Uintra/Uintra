import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "limitTo"
})
export class TruncatePipe implements PipeTransform{
  transform(value: string, args: string): string {
    value = value.replace(/(<\/?(?:a)[^>]*>)|<[^>]+>/gi, "$1");
    const limit = args ? parseInt(args, 10) : 10;
    const trail = "...";

    return value.length > limit ? value.substring(0, limit) + trail : value;
  }
}
