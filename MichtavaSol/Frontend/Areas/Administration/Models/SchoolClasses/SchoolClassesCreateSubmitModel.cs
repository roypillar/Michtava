namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Entities.Models;
    using Services.Interfaces;

    public class SchoolClassCreateSubmitModel : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Class Letter")]
        [MaxLength(1,
            ErrorMessage = "The selected class letter should be a single alphabet character")]
        public string ClassLetter { get; set; }
        
        [Range(1,50)]
        public int ClassNumber { get; set; }


        public IEnumerable<SelectListItem> Subjects
        {
            get
            {
                ISubjectService subjectServ = DependencyResolver.Current.GetService<ISubjectService>();

                var schoolSubs = subjectServ
                    .All()
                    .Select(st => new
                    {
                        st.Id,
                        st.Name
                    }
                    ).ToList();

                IEnumerable<SelectListItem> schoolSubsList = schoolSubs.Select(
                    s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name.ToString()
                    });

                return schoolSubsList;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ISchoolClassService schoolClassService = DependencyResolver.Current.GetService<ISchoolClassService>();

            bool letterExists = schoolClassService
                .All()
                .Any(
                    sc => sc.ClassLetter == this.ClassLetter &&
                        sc.ClassNumber == this.ClassNumber);

            if (letterExists)
            {
                yield return new ValidationResult(
                    "There is already another class with the same letter in this grade.");
            }
        }
    }
}