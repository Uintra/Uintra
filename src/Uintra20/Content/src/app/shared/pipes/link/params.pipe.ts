import { Pipe, PipeTransform } from "@angular/core";
import { UmbracoFlatPropertyModel } from "@ubaseline/next";

interface IULinkParam {
  name: string;
  value: string;
}

@Pipe({
  name: "uparams"
})
export class ParamsPipe implements PipeTransform {
  transform(value: UmbracoFlatPropertyModel | Array<IULinkParam>): object {
    if (value instanceof UmbracoFlatPropertyModel) {
      return this.reduceUFPParams(value);
    }

    if (Array.isArray(value)) {
      return this.reduceSimpleParams(value);
    }

    return {};
  }

  reduceUFPParams(value): object {
    const paramsArray = Object.values(value.get());

    return paramsArray.reduce((acc, val: { data: IULinkParam }) => {
      acc[val.data.name] = val.data.value;
      return acc;
    }, {}) as object;
  }

  reduceSimpleParams(value): object {
    return value.reduce((acc, val) => {
      acc[val.name] = val.data;
      return acc;
    }, {});
  }
}
