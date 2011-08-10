using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual int Age { get; set; }

        public virtual bool IsBuly { get; set; }

        public virtual IEnumerable<Test> Tests { get; set; }

        public Student()
        {
            Tests = new HashSet<Test>();
        }
    }
}