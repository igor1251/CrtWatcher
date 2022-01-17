using System;
using Unity;
using System.Collections.Generic;
using CrtLoader.Model.Interfaces;
using CrtLoader.Model.Classes;

namespace CrtLoader
{
    internal class Program
    {
        static IUnityContainer _container;
        static ILocalStore _localStore;
        static IDbStore _dbStore;

        static void PrepareServices(IUnityContainer container)
        {
            container.RegisterType<IDbContext, DbContext>(TypeLifetime.Singleton);
            container.RegisterType<IQueryStore, QueryStore>(TypeLifetime.Singleton);
            container.RegisterType<IDbStore, DbStore>(TypeLifetime.Singleton);
            container.RegisterType<ILocalStore, LocalStore>(TypeLifetime.Singleton);
        }

        static void ResolveServices(IUnityContainer container)
        {
            _localStore = _container.Resolve<ILocalStore>();
            _dbStore = _container.Resolve<IDbStore>();
        }

        static void Main(string[] args)
        {
            _container = new UnityContainer();
            PrepareServices(_container);
            ResolveServices(_container);

            List<CertificateSubject> subjects = _localStore.LoadCertificateSubjectsAndCertificates().GetAwaiter().GetResult();
            Console.WriteLine("{0, -40}{1, -10}{2, -20}{3, -45}{4, -25}{5, -25}{6, -25}", "Subject name", "Phone", "Comment", "Cert hash", "Algorithm", "Start date", "End date");
            foreach (var item in subjects)
            {
                Console.WriteLine("{0, -40}{1, -10}{2, -20}{3, -45}{4, -25}{5, -25}{6, -25}", item.SubjectName, item.SubjectPhone, item.SubjectComment, "", "", "", "");
                foreach (var certificate in item.CertificateList)
                {
                    Console.WriteLine("{0, -40}{1, -10}{2, -20}{3, -45}{4, -25}{5, -25}{6, -25}", "", "", "", certificate.CertificateHash, certificate.Algorithm, certificate.StartDate, certificate.EndDate);
                }
            }
        }
    }
}
