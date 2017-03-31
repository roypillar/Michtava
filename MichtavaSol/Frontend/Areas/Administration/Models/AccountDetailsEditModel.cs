namespace Frontend.Areas.Administration.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AccountDetailsEditModel
    {
        [Required]
        [Display(Name = "שם מתשמש")]
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