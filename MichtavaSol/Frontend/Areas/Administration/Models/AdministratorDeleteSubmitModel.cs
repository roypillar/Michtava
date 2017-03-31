namespace Frontend.Areas.Administration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AdministratorDeleteSubmitModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AccountDetailsEditModel AccountDetailsEditModel { get; set; }

        [Display(Name = "מחיקה מוחלטת (מחיקת כל היסטוריית הפעולות של המשתמש מהמערכת)")]
        public bool DeletePermanent { get; set; }
    }
}