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
                var penalty = new Penalty
                                  {
                                      AttachedToMany = new List<object>()
                                  };

                for (int i = 0; i < 15; i++)
                {
                    var student = new Student();
                    var test = new Test();

                    session.Save(student);
                    session.Save(test);

                    penalty.AttachedToMany.Add(student);
                    penalty.AttachedToMany.Add(test);
                }

                session.Save(penalty);

                tx.Commit();
            }


            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var penalty = session.Get<Penalty>(1);

                foreach (var o in penalty.AttachedToMany)
                {
                    Console.WriteLine(o.ToString());
                }


                tx.Commit();
            }
        }
    }
}
