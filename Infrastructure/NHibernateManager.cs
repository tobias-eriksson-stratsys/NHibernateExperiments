using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace NHibernateExperiments.Infrastructure
{
    public class NHibernateManager
    {
        private const string ConnStr = "Data Source=:memory:;Version=3;New=True;";
        private static Configuration _configuration;

        public static ISessionFactory BuildSessionFactory()
        {
            var mapper = new ModelMapper();
            mapper.BeforeMapBag += MapperOnBeforeMapBag;
            mapper.BeforeMapManyToOne += MapperOnBeforeMapManyToOne;
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var config = CreateConfiguration(domainMapping);
            return config.BuildSessionFactory();
        }

        protected static Configuration CreateConfiguration(HbmMapping domainMapping)
        {
            var config = new Configuration();
            config.DataBaseIntegration(db =>
                                       {
                                           db.Dialect<SQLiteDialect>();
                                           db.Driver<SQLite20Driver>();
                                           db.ConnectionProvider<TestConnectionProvider>();
                                           db.ConnectionString = ConnStr;
                                           db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                           db.BatchSize = 20;
                                       })
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .SetProperty("show_sql", "false")
                .SetProperty("generate_statistics", "false")
                .AddMapping(domainMapping);

            _configuration = config;
            return _configuration;
        }

        private static void MapperOnBeforeMapManyToOne(IModelInspector insp, PropertyPath prop, IManyToOneMapper map)
        {
            map.Column(prop.LocalMember.Name + "Id");
        }

        private static void MapperOnBeforeMapBag(IModelInspector insp, PropertyPath prop, IBagPropertiesMapper map)
        {
            map.Key(km => km.Column(prop.GetContainerEntity(insp).Name + "Id"));
            map.Cascade(Cascade.All);
        }

        public static Configuration Configuration
        {
            get { return _configuration; }
        }
    }
}