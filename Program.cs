using System;
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

        }

        private static void Action(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var student = new Student
                                  {
                                      Name = "John",
                                  };
                session.Save(student);

                var emergencyPhone = new EmergencyPhone
                                         {
                                             StudentId = student.Id,
                                             Position = student.EmergencyPhones.Count-1,
                                             NameOfCallee = "Mom",
                                             Phone = "19231",
                                             SpokenLanguage = "Hindu"
                                         };

                session.Save(emergencyPhone);
                student.EmergencyPhones.Add(emergencyPhone);

                tx.Commit();
            }
        }
    }
}
