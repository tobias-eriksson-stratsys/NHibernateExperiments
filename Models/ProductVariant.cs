using System.Collections.Generic;

namespace NHibernateExperiments.Models
{
    public class ProductVariant : Entity
    {
        public ProductVariant()
        {
            Values = new List<VariantValue>();
        }

        public virtual Product Product { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<VariantValue> Values { get; set; }
    }
}