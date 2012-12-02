namespace NHibernateExperiments.Models
{
    public class VariantValue : Entity
    {
        public virtual ProductVariant Variant { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}