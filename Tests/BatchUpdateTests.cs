using NHibernateExperiments.Models;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    public class BatchUpdateTests : BaseTest
    {
        private const int NumberOfEntities = 5000;

        protected override void OnFixtureSetup()
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
        public void OrdinaryUpdate()
        {
            Time(() =>
            {
                var products = Session.QueryOver<Product>().List();
                foreach (var product in products)
                {
                    product.Description = "Ändrad OrdinaryUpdate";
                }
                FlushAndClear();
            });
        }

        [Test]
        public void UpdateWithStatelessSession()
        {
            var statelessSession = SessionFactory.OpenStatelessSession();
            Time(() =>
            {
                var products = statelessSession.QueryOver<Product>().List();
                foreach (var product in products)
                {
                    product.Description = "Ändrad UpdateWithStatelessSession";
                    statelessSession.Update(product);                    
                }                
                FlushAndClear();
            });
        }

        [Test]
        public void UpdateWithSql()
        {
            Time(() =>
            {
                Session.CreateSQLQuery("update Product set Description = :description")
                    .SetParameter("description", "Ändrad UpdateWithSql")
                    .ExecuteUpdate();
                FlushAndClear();
            });
        }

        [Test]
        public void UpdateWithDml()
        {
            Time(() =>
            {
                Session.CreateQuery("update Product p set p.Description = :description")
                    .SetParameter("description", "Ändrad UpdateWithDml")
                    .ExecuteUpdate();
                FlushAndClear();
            });
        }
    }
}