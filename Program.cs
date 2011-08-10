using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernateCourse.QuickStart.Model;

namespace NHibernateCourse.QuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = new Configuration()
                .Configure("Hibernate.cfg.xml")
                .BuildSessionFactory();

            using(var session = sessionFactory.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                session.Save(new Student
                                 {
                                     Name = "John Adams"
                                 });

                tx.Commit();
            }

        }
    }
}
