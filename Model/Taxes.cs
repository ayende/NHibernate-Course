using System.Collections;
using System.Collections.Generic;

namespace NHibernateCourse.QuickStart.Model
{
    public class Owner
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class Juresdiction
    {
        public  virtual string Id { get; set; }
        public virtual ICollection<TaxRecord> TaxRecords { get; set; }
    }

    public class Account
    {
        public virtual int Id { get; set; }
        public virtual bool CurrentlySuing { get; set; }
        public virtual Owner Owner { get; set; }
    }

    public class TaxRecord
    {

        public virtual int Id { get; set; }
        public virtual string Juresdiction { get; set; }
        public virtual decimal TotalDue { get; set; }
        public virtual Account Account { get; set; }
   
    }
}