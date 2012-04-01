using NHibernate.Transform;
using NHibernateExperiments.Models;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    public class ReadManyEntitiesTests : BaseTest
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
        }

        [Test]
        public void OrdninaryQuery()
        {
            Time(() =>
            {
                var products = Session.QueryOver<Product>().List<Product>();
                FlushAndClear();         
            });            
        }

        [Test]
        public void ReadonlySession()
        {
            Session.DefaultReadOnly = true;
            Time(() =>
            {
                var products = Session.QueryOver<Product>().ReadOnly().List<Product>();
                FlushAndClear();
            });
        }

        [Test]
        public void StatelessSession()
        {
            var session = SessionFactory.OpenStatelessSession();
            Time(() =>
            {
                var products = session.QueryOver<Product>().List<Product>();
                FlushAndClear();
            });
        }

        [Test]
        public void ReportingQuery()
        {
            Time(() =>
            {                     
                var products = Session.CreateQuery("select p.Name as Name, p.Description as Description from Product p")
                    .SetResultTransformer(Transformers.AliasToBean<ProductDto>())
                    .List<ProductDto>();
                FlushAndClear();
            });
        }

        private class ProductDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}