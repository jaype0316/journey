

export class Tag {
    id:string;
    name:string;
    isDefault:boolean;

    constructor(id:string, name:string, isDefault:boolean){
        this.name = name;
        this.isDefault = isDefault;
        this.id = id;
    }
}


