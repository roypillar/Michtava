namespace Frontend.Areas.Students.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    public class StudentRegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} חייב להיות קצר מ{1} תווים.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "מספר הטלפון שהוזן אינו חוקי.")]
        public string PhoneNumber { get; set; }


        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} חייב להיות באורך {1} תווים לפחות.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        [Compare("Password", ErrorMessage = "הסיסמא ואימותה לא תואמות.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} חייב להיות קצר מ{1} תווים.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

       
    }
}