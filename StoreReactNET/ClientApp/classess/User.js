export class User
{
    constructor(respondString)
    {
        let respond = JSON.parse(respondString);

        this.userID = respond.id;
        this.email = respond.email;
        this.firstName = respond.firstName;
        this.lastName = respond.lastName;
    }
}