namespace Services.Interfaces
{
    using System;
    using Entities.Models;
    using Common;
    using System.Collections.Generic;

    public interface ISchoolClassService : IRepositoryService<SchoolClass>
    {
        SchoolClass GetById(Guid id);

        SchoolClass GetByDetails(int gradeYear, string letter);

        MichtavaResult addStudentToSchoolClass(Student s, SchoolClass c);

        MichtavaResult HardDelete(SchoolClass schoolClass);

        ICollection<Student> GetStudents(SchoolClass sc);

        ICollection<Teacher> GetTeachers(SchoolClass sc);

        MichtavaResult AddStudentToClass(Student student, SchoolClass targetSc);

        MichtavaResult RemoveStudentFromClass(Student student, SchoolClass sc);

        MichtavaResult AddTeacherToClass(Teacher teacher, SchoolClass targetSc);

        MichtavaResult RemoveTeacherFromClass(Teacher teacher, SchoolClass targetSc);

    }
}
