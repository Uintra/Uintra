import { IUProperty } from 'src/app/shared/interface/umbraco-property';

export interface IDefaultModel {
    contentTypeAlias: string;
    isDefaultModel: boolean;
    properties: IDefaultModelProperty[];
}
export interface IDefaultModelProperty {
    alias: string;
    propertyEditorAlias: string;
    value: any;
}

export function isDefaultModel(data: any): data is IDefaultModel {
    return data && data.isDefaultModel === true;
}

export function defaultModel2UProperty(data: IDefaultModel)
{
    let res: {[key: string]: IUProperty<any>} = {};

    data.properties.forEach(prop => {
        res[prop.alias] = {
          value: prop.value,
          propertyEditorAlias: prop.propertyEditorAlias,
          alias: prop.alias
        }
    });

    return res;
}