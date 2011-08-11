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
                var student = new Student();
                var test = new Test();

                session.Save(student);
                session.Save(test);

                session.Save(new Penalty
                                 {
                                     Score = 5,
                                     AttachedTo = test
                                 });
                session.Save(new Penalty
                                 {
                                     Score = 43,
                                     AttachedTo = student
                                 });

                
                tx.Commit();
            }


            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var q = from student in session.Query<Student>()
                        from penalty in student.Penalties
                        where student.Name == "John"
                        select penalty;

                q.ToList();
                
               
                tx.Commit();
            }
        }
    }
}
