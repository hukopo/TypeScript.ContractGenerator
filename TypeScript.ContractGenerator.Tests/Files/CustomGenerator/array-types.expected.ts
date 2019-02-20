
export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | string;
    nullableByteArray?: null | Array<null | Byte>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | string[];
    customTypes?: null | AnotherCustomType[];
    stringsList?: null | string[];
    customTypesDict?: null | {
        [key in string]?: AnotherCustomType;
    };
};
export type Byte = {
};
export type AnotherEnum = 'B' | 'C';
export const AnotherEnums = {
    ['B']: ('B') as AnotherEnum,
    ['C']: ('C') as AnotherEnum,
};
export type AnotherCustomType = {
    d: number;
};
