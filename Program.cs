using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Event;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Type;
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

            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {

                QueryDynamically<Test>(session,
                    "Id in (select Id from Students where IsBuly = 0)",
                    new QueryOp {Field = "Score", Operator = "=", Value = 1});

                tx.Commit();
            }
        }

        public class QueryOp
        {
            public string Field;
            public object Value;
            public string Operator;
         
        }

        public class FieldHelper
        {
            public string Field;

            public string JustField
            {
                get
                {
                    var lastIndexOf = Field.LastIndexOf('.');

                    return Field.Substring(lastIndexOf + 1);

                }
            }
            public string Path
            {
                get
                {
                    var lastIndexOf = Field.LastIndexOf('.');
                    if (lastIndexOf == -1)
                        return null;

                    return Field.Substring(0, lastIndexOf);
                }
            }
        }

        private static IList<T> QueryDynamically<T>(ISession session, string studentFilter, params QueryOp[] ops) where T : class
        {
            var query = session.CreateCriteria<T>();

            query.Add(new SQLCriterion(new SqlString(studentFilter), new object[0],
                                                                   new IType[0]));

            foreach (var operationsOnField in ops.GroupBy(x => x.Field))
            {
                var criteria = new List<ICriterion>();
                var fieldHelper = new FieldHelper { Field = operationsOnField.Key };
                foreach (var queryOp in operationsOnField)
                {
                    switch (queryOp.Operator)
                    {
                        case "=":
                            criteria.Add(Restrictions.Eq(fieldHelper.JustField, queryOp.Value));
                            break;
                        case ">":
                            criteria.Add(Restrictions.Gt(fieldHelper.JustField, queryOp.Value));
                            break;
                        case "<":
                            criteria.Add(Restrictions.Lt(fieldHelper.JustField, queryOp.Value));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(queryOp.Operator);
                    }
                }
                var currentQuery = query;
                if(fieldHelper.Path != null)
                {
                    currentQuery = query.GetCriteriaByPath(fieldHelper.Path)
                                   ?? query.CreateCriteria(fieldHelper.Path);
                }
                switch (criteria.Count)
                {
                    case 0:
                        break;
                    case 1:
                        currentQuery.Add(criteria.First());
                        break;
                    default:
                        var dis = new Disjunction();

                        foreach (var criterion in criteria)
                        {
                            dis.Add(criterion);
                        }

                        currentQuery.Add(dis);
                        break;
                }
            }

            return query.List<T>();
        }
    }


    // See the ReadMe.html for additional information
    public class ObjectDumper
    {

        public static void Write(object element)
        {
            Write(element, 0);
        }

        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        public static void Write(object element, int depth, TextWriter log)
        {
            ObjectDumper dumper = new ObjectDumper(depth);
            dumper.writer = log;
            dumper.WriteObject(null, element);
        }

        TextWriter writer;
        int pos;
        int level;
        int depth;

        private ObjectDumper(int depth)
        {
            this.depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < level; i++) writer.Write("  ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (pos % 8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level < depth)
                            {
                                level++;
                                WriteObject(prefix, item);
                                level--;
                            }
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteIndent();
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(m.Name);
                            Write("=");
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if (t.IsValueType || t == typeof(string))
                            {
                                WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    Write("...");
                                }
                                else
                                {
                                    Write("{ }");
                                }
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        level++;
                                        WriteObject(m.Name + ": ", value);
                                        level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }


}
