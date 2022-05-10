using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using F13StandardUtils.CbkFramework.Scripts.Core.Async.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator
{
    public class ServiceLocator: MonoBehaviour , IServiceLayer
    {
        public static ServiceLocator Instance;
        private readonly Dictionary<Type,Type> _allServices=new Dictionary<Type,Type>();
        private readonly Dictionary<Type,IService> _runningServices=new Dictionary<Type, IService>();

        private void Awake()
        {
            Instance = this;
            MapServices();
            InitNotLazyServices();
            GetService<IAsyncService>().WaitForSecond(0f, BootGame);
        }

        private void BootGame()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.Select(a => a.GetTypes()).SelectMany(b => b).Where(c => c.GetCustomAttributes(typeof(BootGameAttribute), false).Length > 0).ToList();
            if(types.Count < 1) 
                throw new Exception(nameof(ServiceLocator)+": "+"Add "+nameof(BootGameAttribute)+"!");
            if(types.Count > 1) 
                throw new Exception(nameof(ServiceLocator)+": "+"Add "+nameof(BootGameAttribute)+" once!");
            Activator.CreateInstance(types.First());
        }

        private void MapServices()
        {
            _allServices.Clear();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies
                .Select(a => a.GetTypes())
                .SelectMany(t => t)
                .Where(s => s.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0)
                .ToList();

            foreach (var type in types)
            {
                var serviceInterfaceTypes = type.GetInterfaces();
                var st = serviceInterfaceTypes.Where(t => t.GetInterface(nameof(IService)) != null).ToList();
                if(!st.Any()) throw new Exception(nameof(ServiceLocator)+": "+type.Name + " have to derived from "+nameof(IService)+".");
                if(st.Count>1)throw new Exception(nameof(ServiceLocator)+": "+type.Name + " have to contains "+nameof(IService)+" only once.");
                _allServices.Add(st.First(),type);
            }
        }
        
        private void InitNotLazyServices()
        {
            var notLazyServiceList = _allServices.Where(s=>s.Value.GetCustomAttribute(typeof(ServiceAttribute)) is ServiceAttribute a && !a.IsLazy);
            foreach (var type in notLazyServiceList)
            {
                InitService(type.Key);
            }
        }
        
        private void InitService(Type serviceType)
        {
            if (_runningServices.ContainsKey(serviceType))
            {
                return;
            }

            if (!_allServices.ContainsKey(serviceType))
            {
                throw new Exception("There is no registered service as " + serviceType.Name + ". Please make sure using "+typeof(ServiceAttribute)+" on your service OR implement some interface which derived from "+nameof(IService));
            }

            var actualType = _allServices[serviceType];
            IService service;
            if (typeof(MonoBehaviour).IsAssignableFrom(actualType))
            {
                var serviceGO = new GameObject {name = actualType.Name};
                service = serviceGO.AddComponent(actualType) as IService;
                DontDestroyOnLoad(serviceGO);
            }
            else
            {
                service = (IService) Activator.CreateInstance(actualType);
            }
            _runningServices.Add(serviceType, service);
            service?.Initialize();
        }
        
        public T GetService<T>() where T : IService
        {
            var serviceType = typeof(T);
            if(!serviceType.IsInterface) throw new Exception(nameof(ServiceLocator)+": "+nameof(GetService)+"<T>() T have to be interface. " + serviceType.Name+" is NOT interface");
            if(!_runningServices.ContainsKey(serviceType)) InitService(serviceType);
            return (T) _runningServices[serviceType];
        }

    }
}

