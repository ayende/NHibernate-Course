using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernateCourse.QuickStart.Model;

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
                session.Save(new Test {Score = 10});


                tx.Commit();
            }

            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var test = session.Get<Test>(1);
                session.Lock(test, LockMode.Upgrade);
                test.Score += 10;

                tx.Commit();
            }
        }
    }
}
