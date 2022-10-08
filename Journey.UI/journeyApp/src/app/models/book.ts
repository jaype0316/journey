export class Book {
    pk:string;
    about:string;
    title:string;
    createdAt:Date;
    logoKey:string;

    constructor() {
        this.createdAt = this.createdAt || new Date();
    }
}
