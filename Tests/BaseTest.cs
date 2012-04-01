using System;
using System.Diagnostics;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NHibernateExperiments.Infrastructure;
using NUnit.Framework;

namespace NHibernateExperiments.Tests
{
    [TestFixture]
    public class BaseTest
    {
        static BaseTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected virtual void OnFixtureSetup() { }
        protected virtual void OnSetup() { }
        protected virtual void OnTeardown() { }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ConfigureFixture();
            OnFixtureSetup();
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            OnFixtureTeardown();
        }

        [SetUp]
        public void Setup()
        {
            OnSetup();
        }

        [TearDown]
        public void Teardown()
        {
            OnTeardown();
        } 


         private ISessionFactory _sessionFactory;
        private static Stopwatch _timer;


        protected ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = NHibernateManager.BuildSessionFactory()); }
        }

        protected ISession Session
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
        }


        protected void ConfigureFixture()
        {
            SetupNHibernateSession();
        }

        protected void OnFixtureTeardown()
        {
            TearDownNHibernateSession();
            OnTeardown();
        }

        protected void SetupNHibernateSession()
        {
            TestConnectionProvider.ExplicitlyDestroyConnection();
            SetupContextualSession();
            BuildSchema();
        }
        protected void TearDownNHibernateSession()
        {
            TearDownContextualSession();
            TestConnectionProvider.ExplicitlyDestroyConnection();
        }
        private void SetupContextualSession()
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }
        private void TearDownContextualSession()
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);
            session.Close();
        }
        private void BuildSchema()
        {
            var cfg = NHibernateManager.Configuration;
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Create(false, true);
        }

        protected virtual void Save(params object[] entities)
        {
            foreach (var entity in entities)
            {
                Session.Save(entity);
            }
        }

        protected void FlushAndClear()
        {
            Session.Flush();
            Session.Clear();
        }

        protected static void StartTimer()
        {
            _timer = Stopwatch.StartNew();
        }
        
        protected static void Time(Action action)
        {
            Agdur.Benchmark.This(action)
                .Times(3).Average().InMilliseconds().ToConsole();
        }

        protected static void StopTimer()
        {
            Debug.WriteLine("Elapsed: " + _timer.ElapsedMilliseconds);
        }
    }
}