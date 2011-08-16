using System;
using System.Media;
using System.Threading;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Event;
using NHibernate.SqlCommand;
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
                var configuration = new Configuration()
                    .Configure("Hibernate.cfg.xml");
                configuration
                    .SetListener(ListenerType.PreUpdate, new ValidationListener());
                configuration
                    .SetListener(ListenerType.PreInsert, new ValidationListener());

                var sessionFactory = configuration
                    .BuildSessionFactory();

                Action(sessionFactory);
            }
            finally
            {
                HibernatingRhinos.Profiler.Appender.ProfilerInfrastructure.FlushAllMessages();
            }

            Thread.Sleep(1000);

        }

        private static void Action(ISessionFactory sessionFactory)
        {
            HttpContext.Current = new HttpContext(new HttpRequest("a", "http://localhost/", "a"), new HttpResponse(Console.Out));

            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var sumOfDues = Projections.Sum<TaxRecord>(x=>x.TotalDue);
                var detachedCriteria = DetachedCriteria.For<TaxRecord>()
                    .AddOrder(Order.Desc(sumOfDues))
                    .SetProjection(
                        Projections.ProjectionList()
                            .Add(sumOfDues)
                            .Add(Projections.GroupProperty("Account.Id"))
                    )
                    .Add(Property.ForName("Id").EqProperty("x.Id"))
                    .Add(Property.ForName("Juresdiction").EqProperty("j.Id"))
                    .SetMaxResults(5);

                var top5AccountsInEachJuresdiction = DetachedCriteria.For<Juresdiction>("j")
                    .CreateAlias("TaxRecords", "x", JoinType.InnerJoin,
                                 Subqueries.Exists(detachedCriteria))
                    .SetProjection(Projections.Property("x.Account.Id"));

                session.CreateCriteria<Owner>()
                    .CreateAlias("Accounts", "acc")
                    .Add(Restrictions.Eq("acc.CurrentlySuing", false))
                    .Add(Subqueries.PropertyIn("acc.Id", top5AccountsInEachJuresdiction))
                    .List();


                tx.Commit();
            }
        }

    }
}
