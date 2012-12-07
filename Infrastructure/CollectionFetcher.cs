using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Persister.Collection;
using NHibernateExperiments.Infrastructure.Ryc.Utilities;
using NHibernateExperiments.Models;

namespace NHibernateExperiments.Infrastructure
{
    public class CollectionFetcher
    {
        private readonly SessionImpl m_session;
        public int MaximumParameters = 1000;

        public CollectionFetcher(ISession session)
        {
            m_session = (SessionImpl)session;
        }

        private void Fetch<T>(string collectionProperty, IList<int> parentIds)
        {
            string roleName = typeof(T).FullName + "." + collectionProperty;
            var sessionFactory = (SessionFactoryImpl)m_session.SessionFactory;
            var sessionFactoryHelper = new SessionFactoryHelper(sessionFactory);

            var persister = sessionFactoryHelper.GetCollectionPersister(roleName);
            foreach (var chunk in ListUtil.Split(parentIds, MaximumParameters))
            {
                var ids = chunk.Cast<object>().ToArray();
                var loader = new OneToManyLoader(persister, ids.Length, sessionFactory, new Dictionary<string, IFilter>());
                loader.LoadCollectionBatch(m_session, ids, NHibernateUtil.Int32);
            }
        }


        /// <summary>
        /// Fetches collection for specified entities. Collections will not be fetched for entities where the collection already is initialized.
        /// </summary>
        public void Fetch<T>(IEnumerable<T> parents, Expression<Func<T, object>> collection) where T : Entity
        {
            var col = collection.Compile();
            var notInitialized = parents.Where(p => !NHibernateUtil.IsInitialized(col(p)));
            if (notInitialized.Any())
            {

                var propertyName = ExpressionProcessor.FindMemberExpression(collection.Body);
                Fetch<T>(propertyName, notInitialized.Select(p => p.Id).ToList());
            }
        }
    }
}
