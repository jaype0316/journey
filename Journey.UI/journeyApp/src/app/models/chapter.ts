import { Tag } from "./tag";

export class Chapter {
    pk:string;
    id:number;
    sequence:number;
    title:string;
    body:string;
    createdAt:Date;
    tags:Tag[];

    constructor() {
        this.createdAt = this.createdAt || new Date();
        this.tags = new Array();
    }
}
