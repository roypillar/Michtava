namespace Frontend.Areas.Administration.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AccountDetailsEditModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}