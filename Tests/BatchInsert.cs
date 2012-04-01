using System.Diagnostics;
using NHibernateExperiments.Models;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    public class BatchInsert : BaseTest
    {
        private const int NumberOfEntities = 2000;

        [Test]
        public void InsertWithOrdinarySession()
        {
            Time(() =>
            {
                for (int i = 0; i < NumberOfEntities; i++)
                {
                    var product = new Product
                                    {
                                        Description = "Beskrivning " + i,
                                        Name = "Namn " + i,
                                    };
                    Session.Save(product);
                }
                FlushAndClear();
            });
        }

        [Test]
        public void InsertWithStatelessSession()
        {
            Time(() =>
            {
                var session = SessionFactory.OpenStatelessSession();
                for (int i = 0; i < NumberOfEntities; i++)
                {
                    var product = new Product
                                    {
                                        Description = "Beskrivning " + i,
                                        Name = "Namn " + i,
                                    };
                    session.Insert(product);
                }
            });
        }
    }
}