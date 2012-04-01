namespace NHibernateExperiments.Models
{
    public class ProductVariant : Entity
    {
        public virtual Product Product { get; set; }
        public virtual string Name { get; set; }
    }
}