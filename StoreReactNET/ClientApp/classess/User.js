export class User
{
    constructor(respondString)
    {
        let respond = JSON.parse(respondString);

        this.userID = respond.ID;
        this.email = respond.Email;
        this.firstName = respond.FirstName;
        this.lastName = respond.LastName;
    }
}