using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernateExperiments.Models
{
    public class EntityMapping<T> : ClassMapping<T> where T : Entity
    {
        public EntityMapping()
        {
            Id(c => c.Id, m => m.Generator(new HighLowGeneratorDef()));
        }
    }
}