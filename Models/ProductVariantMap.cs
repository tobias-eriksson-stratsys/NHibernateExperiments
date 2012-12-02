namespace NHibernateExperiments.Models
{
    public class ProductVariantMap : EntityMapping<ProductVariant>
    {
        public ProductVariantMap()
        {
            Property(p => p.Name);
            ManyToOne(p => p.Product);
            Bag(p => p.Values, m => m.Inverse(true), m => m.OneToMany());
        }
    }
}