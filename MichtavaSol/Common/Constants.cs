using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Constants
    {
        public const string SuperAdministratorRoleName = "SuperAdministrator";

        public const string AdministratorRoleName = "Administrator";

        public const string TeacherRoleName = "Teacher";

        public const string StudentRoleName = "Student";

        public const string DefaultProfileImageUrl =
            "/Content/Images/Profile_Images/Default/default-profile-image.png";//TODO actually add the picture

        public const string TeachersProfileImagesUploadDirectory = "/Content/Images/Profile_Images/Teachers";//TODO

        public const string StudentsProfileImagesUploadDirectory = "/Content/Images/Profile_Images/Students";//TODO
    }
}
