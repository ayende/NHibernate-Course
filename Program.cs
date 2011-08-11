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

                session.Get<Graphic>(1);
                session.Get<Final>(1);

                session.Save(new Rough
                                 {
                                     Name = "a",
                                     Height = 480,
                                     Width = 680,
                                     Type = "png"
                                 });

                session.Query<File>()
                    .Where(x => x.Name == "b")
                    .ToList();

                session.Query<Final>()
                    .Where(x => x.ColorDepth > 16)
                    .ToList();

                tx.Commit();
            }
        }
    }
}
