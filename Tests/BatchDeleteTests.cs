using NHibernateExperiments.Models;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    public class BatchDeleteTests : BaseTest
    {
        private const int NumberOfEntities = 5000;
        

        private void InitDb()
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
            FlushAndClear();
        }

        [Test]
        public void OrdinaryDelete()
        {            
            for (int i = 0; i < 3; i++)
            {
                InitDb();
                StartTimer();
                var products = Session.QueryOver<Product>().List();

                foreach (var product in products)
                {
                    Session.Delete(product);
                }
                FlushAndClear();
                StopTimer();    
            }
            
        }

        [Test]
        public void DeleteWithStatelessSession()
        {
            var statelessSession = SessionFactory.OpenStatelessSession();
            for (int i = 0; i < 3; i++)
            {
                InitDb();
                StartTimer();
                var products = statelessSession.QueryOver<Product>().List();
                foreach (var product in products)
                {
                    statelessSession.Delete(product);
                }
                FlushAndClear();
                StopTimer();
            }
        }

        [Test]
        public void DeleteWithSql()
        {
            for (int i = 0; i < 3; i++)
            {
                InitDb();
                StartTimer();
                Session.CreateSQLQuery("delete from Product").ExecuteUpdate();
                FlushAndClear();
                StopTimer();
            }
        }

        [Test]
        public void DeleteWithDml()
        {
            for (int i = 0; i < 3; i++)
            {
                InitDb();
                StartTimer();
                Session.CreateQuery("delete Product").ExecuteUpdate();
                FlushAndClear();
                StopTimer();
            }
        }
    }
}