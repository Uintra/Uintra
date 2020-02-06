import { Pipe, PipeTransform } from "@angular/core";
import { UmbracoFlatPropertyModel } from "@ubaseline/next";

@Pipe({
  name: "ulink"
})
export class LinkPipe implements PipeTransform {
  transform(value: UmbracoFlatPropertyModel | string): string {
    const isUFP = value instanceof UmbracoFlatPropertyModel;
debugger
    if (isUFP) {
      return (value as UmbracoFlatPropertyModel).get();
    }
    if (typeof value === "string") {
      return value;
    }

    return "/";
  }
}
