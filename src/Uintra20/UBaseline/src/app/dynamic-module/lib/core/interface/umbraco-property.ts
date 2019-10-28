export interface IUProperty<T> {
    value: T;
    propertyEditorAlias: string;
    alias: string;
}

export class UProperty {
    static extract<S, R>(data: S, extract: Array<keyof R> = [])
    {
        let res = Object.create(null) as R;

        extract.forEach((key: keyof R) => {
            if (data.hasOwnProperty(key))
            {
                res[key as string] = data[key as string] && data[key as string].value;
            }
        });

        return res;
    }
}