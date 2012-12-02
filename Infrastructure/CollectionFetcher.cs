using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Persister.Collection;

namespace NHibernateExperiments.Infrastructure
{
    public class CollectionFetcher
    {
        private SessionImpl _session;

        public CollectionFetcher(ISession session)
        {
            _session = (SessionImpl)session;
        }

        public void Fetch<T>(string collectionProperty, IEnumerable<int> parentIds)
        {            
            string roleName = typeof(T).Name + "." + collectionProperty;
            var sessionFactory = (SessionFactoryImpl)_session.SessionFactory;
            var sessionFactoryHelper = new SessionFactoryHelper(sessionFactory);

            var persister = sessionFactoryHelper.GetCollectionPersister(roleName);            
            var loader = new OneToManyLoader(persister, sessionFactory, new Dictionary<string, IFilter>());

            loader.LoadCollectionBatch(_session, parentIds.Cast<object>().ToArray(), NHibernateUtil.Int32); 
        }
    }
}
