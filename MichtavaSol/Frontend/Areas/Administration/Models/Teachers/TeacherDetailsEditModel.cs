namespace Frontend.Areas.Administration.Models.Teachers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Web;
    using Frontend.Areas.Administration.Models.Account;
    public class TeacherDetailsEditModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name must not be more than 20 characters.")]
        public string Name { get; set; }

        public AccountDetailsEditModel AccountDetailsEditModel { get; set; }

     
    }
}