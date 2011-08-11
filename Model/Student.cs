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
        public virtual Address Address { get; set; }
        public Student()
        {
            Tests = new HashSet<Test>();
            EmergencyPhones = new List<EmergencyPhone>();
        }
    }

    public class Address
    {
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class EmergencyPhone
    {
        public virtual string Phone { get; set; }
        public virtual string NameOfCallee { get; set; }
        public virtual string SpokenLanguage { get; set; }
    }

    public class User
    {
        public virtual string Username { get; set; }
        public virtual ICollection<Group> Groups { get; set; } 
    }
    public class Group
    {
        public virtual string Name { get; set; }
    }

}