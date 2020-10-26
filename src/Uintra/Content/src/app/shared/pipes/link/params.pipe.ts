import { Pipe, PipeTransform } from '@angular/core';

interface IULinkParam {
  name: string;
  value: string;
}

@Pipe({
  name: 'uparams'
})
export class ParamsPipe implements PipeTransform {
  transform(value: Array<IULinkParam>): object {

    if (Array.isArray(value)) {
      return this.reduceSimpleParams(value);
    }

    return {};
  }

  reduceSimpleParams(value): object {
    return value.reduce((acc, val) => {
      acc[val.name] = val.data;
      return acc;
    }, {});
  }
}
