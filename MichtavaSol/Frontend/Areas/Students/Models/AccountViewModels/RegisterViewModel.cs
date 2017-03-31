namespace Frontend.Areas.Students.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} חייב להיות קצר מ{1} תווים.")]
        [Display(Name = "שם המשתמש")]
        public string UserName { get; set; }

        [Phone]
        [Display(Name = "מספט הטלפון")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "מספר הטלפון שהוזן אינו חוקי.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} חייב להיות קצר מ{1} תווים.")]
        [Display(Name = "שם (פרטי+משפחה)")]
        public string Name { get; set; }



        [Required]
        [StringLength(100, ErrorMessage = "{0} חייב להיות באורך {1} תווים לפחות.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "אימות סיסמא")]
        [Compare("Password", ErrorMessage = "הסיסמא ואימותה לא תואמות.")]
        public string ConfirmPassword { get; set; }

    }
}