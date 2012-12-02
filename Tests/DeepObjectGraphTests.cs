using System.Collections.Generic;
using System.Diagnostics;
using NHibernateExperiments.Infrastructure;
using NHibernateExperiments.Models;
using NUnit.Framework;
using System.Linq;

namespace NHibernateExperiments.Tests
{
    public class DeepObjectGraphTests : BaseTest
    {
        protected override void OnFixtureSetup()
        {            
            var session = SessionFactory.OpenStatelessSession();
            for (int i = 0; i < 2; i++)
            {
                var product = new Product
                {
                    Description = "Beskrivning " + i,
                    Name = "Namn " + i,
                };

                var variant1 = new ProductVariant
                    {
                        Name = i + " Variant 1",
                        Product = product
                    };
                var value = new VariantValue
                    {
                        Name = i + " Val",
                        Variant = variant1
                    };
                variant1.Values.Add(value);


                var variant2 = new ProductVariant
                    {
                        Name = i + " Variant 2",
                        Product = product
                    };

                product.Variants.Add(variant1);
                product.Variants.Add(variant2);

                session.Insert(product);
                session.Insert(variant1);
                session.Insert(variant2);
                session.Insert(value);                
            }
            FlushAndClear();
            UseNHibernateProfiler();            
        }

        protected override void OnSetup()
        {
            FlushAndClear();
        }

        protected override void OnTeardown()
        {
            FlushNHibernateProfiler();
        }

        [Test]
        public void OrdinaryQuery()
        {
            var products = Session.QueryOver<Product>().List();

            EnumerateGraph(products);
            
        }

        [Test]
        public void OptimizedQueries()
        {
            var products = Session.QueryOver<Product>().Fetch(p => p.Variants).Eager.List();

            Session.QueryOver<ProductVariant>().Fetch(v => v.Values).Eager
                   .WhereRestrictionOn(v => v.Product).IsInG(products).List();

            EnumerateGraph(products);
        }

        [Test]
        public void CollectionFetcher()
        {
            var products = Session.QueryOver<Product>().List();
            var fetcher = new CollectionFetcher(Session);
            fetcher.Fetch<Product>("Variants", products.Select(p => p.Id));

            //Session.QueryOver<ProductVariant>().Fetch(v => v.Values).Eager
            //       .WhereRestrictionOn(v => v.Product).IsInG(products).List();

            EnumerateGraph(products);

        }

        private void EnumerateGraph(IList<Product> products)
        {
            foreach (var product in products)
            {
                Debug.WriteLine(product.Name);
                foreach (var variant in product.Variants)
                {
                    Debug.WriteLine(variant.Name);
                    foreach (var value in variant.Values)
                    {
                        Debug.WriteLine(value.Name);    
                    }
                }
            }
        }
    }
}