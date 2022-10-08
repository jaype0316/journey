export class Chapter {
    pk:string;
    id:number;
    sequence:number;
    title:string;
    body:string;
    createdAt:Date;
    tags:string[];

    constructor() {
        this.createdAt = this.createdAt || new Date();
        this.tags = new Array();
    }
}
