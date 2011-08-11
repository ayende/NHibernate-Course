using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual int Age { get; set; }

        public virtual bool IsBuly { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public virtual IList<EmergencyPhone> EmergencyPhones { get; set; } 

        public Student()
        {
            Tests = new HashSet<Test>();
            EmergencyPhones = new List<EmergencyPhone>();
        }
    }

    public class EmergencyPhone
    {
        public virtual int StudentId { get; set; }
        public virtual int Position { get; set; }
        public virtual string Phone { get; set; }
        public virtual string NameOfCallee { get; set; }
        public virtual string SpokenLanguage { get; set; }

        public virtual  bool Equals(EmergencyPhone other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.StudentId == StudentId && other.Position == Position;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EmergencyPhone)) return false;
            return Equals((EmergencyPhone) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StudentId*397) ^ Position;
            }
        }
    }
}