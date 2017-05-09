namespace Frontend.Areas.Administration.Models.Admins
{
    using System;
    using Frontend.Areas.Administration.Models.Account;
    public class AdministratorDetailsEditModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AccountDetailsEditModel AccountDetailsEditModel { get; set; }
    }
}