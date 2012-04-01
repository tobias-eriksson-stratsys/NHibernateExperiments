namespace NHibernateExperiments.Models
{
    public class ProductMap : EntityMapping<Product>
    {
        public ProductMap()
        {
            Property(p => p.Name);
            Property(p => p.Description);
            Bag(p => p.Variants, m => m.Inverse(true), m => m.OneToMany());
        }
    }
}