namespace NHibernateExperiments.Models
{
    public class ProductVariantMap : EntityMapping<ProductVariant>
    {
        public ProductVariantMap()
        {
            Property(p => p.Name);
        }
    }
}