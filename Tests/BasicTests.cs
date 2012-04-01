using NHibernateExperiments.Models;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    public class BasicTests : BaseTest
    {
        [Test]
        public void CanSaveEntity()
        {
            var product = new Product()
            {
                Name = "Strumpa",
                Description = "Strumpa i hög kvalité."
            };

            Save(product);            
            FlushAndClear();            
            var dbProduct = Session.Get<Product>(product.Id);
            Assert.That(dbProduct, Is.Not.Null);
            Assert.That(dbProduct.Name, Is.EqualTo("Strumpa"));
            Assert.That(dbProduct.Description, Is.EqualTo("Strumpa i hög kvalité."));
        }
    }
}