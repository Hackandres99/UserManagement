export class User {
    constructor(
        public id: number,
        public firstname: string,
        public lastname: string,
        public username: string,
        public email: string,
        public accountPassword: string,
        public avatarImagePath: string,
        public creationDate: Date,
        public updateDate: Date
    ){}
}
