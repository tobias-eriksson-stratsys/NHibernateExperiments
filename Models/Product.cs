using System.Collections.Generic;

namespace NHibernateExperiments.Models
{
    public class Product : Entity
    {
        public Product()
        {
            Variants = new List<ProductVariant>();
        }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<ProductVariant> Variants { get; set; }
    }
}