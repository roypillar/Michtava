namespace Frontend.Areas.Administration.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AccountDetailsEditModel
    {
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "אימייל")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "מספר טלפון")]
        public string PhoneNumber { get; set; }
    }
}