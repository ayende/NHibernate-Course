using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
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
                var blind = new Penalty
                                  {
                                      Score = 15
                                  };
                var deaf = new Penalty
                               {
                                   Score = 10
                               };
                var thunder = new Penalty
                                  {
                                      Score = -5
                                  };
                session.Save(blind);
                session.Save(deaf);
                session.Save(thunder);
                session.Save(new Test
                                 {
                                     Score = 10,
                                     Penalties = new Dictionary<string, Penalty>
                                                     {
                                                         {"Blind", blind},
                                                         {"Deaf", deaf},
                                                         {"Thunder", thunder}
                                                     }
                                 });


                tx.Commit();
            }

            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var load = session.Load<Test>(1);

                foreach (var penalty in load.Penalties)
                {
                    Console.WriteLine("{0}: {1}", penalty.Key, penalty.Value.Score);
                }

                tx.Commit();
            }
        }
    }
}
