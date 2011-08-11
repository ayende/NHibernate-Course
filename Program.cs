using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernateCourse.QuickStart.Model;
using NHibernate.Linq;

namespace NHibernateCourse.QuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            try
            {
                var sessionFactory = new Configuration()
                    .Configure("Hibernate.cfg.xml")
                    .BuildSessionFactory();

                Action(sessionFactory);
            }
            finally
            {
                HibernatingRhinos.Profiler.Appender.ProfilerInfrastructure.FlushAllMessages();
            }

            Console.ReadLine();

        }

        private static void Action(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(new Student
                                 {
                                     Address = new Address
                                                   {
                                                       City = "Redding",
                                                       HouseNumber = "3474",
                                                       Street = "Electro Way"
                                                   },
                                     Age = 15,
                                     Name = "Frank",
                                     Attributes = new Hashtable
                                                      {
                                                          {"GPA4", "abc"},
                                                          {"GPA5", 32}
                                                      }
                                 });



                tx.Commit();
            }

            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.CreateCriteria<Student>()
                    .Add(Restrictions.Eq("Attributes.GPA4", "abc"))
                    .List();

                tx.Commit();
            }
        }
    }
}
