namespace NHibernateExperiments.Models
{
    public class VariantValueMap : EntityMapping<VariantValue>
    {
        public VariantValueMap()
        {
            Property(p => p.Name);
            Property(p => p.Value);
            ManyToOne(p => p.Variant);
        }
    }
}