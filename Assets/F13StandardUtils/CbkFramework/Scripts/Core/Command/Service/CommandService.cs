using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using F13StandardUtils.CbkFramework.Scripts.Core.Command.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Attribute;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Command.Service
{
    [Service]
    public class CommandService : BaseService, ICommandService
    {
        
        public void Initialize()
        {
            BindEventToCommand();

        }
        private void BindEventToCommand()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = assemblies.Select(a => a.GetTypes()).SelectMany(b => b).Where(c => c.GetCustomAttributes(typeof(BindEventAttribute), false).Length > 0).ToList();

            foreach (Type type in types)
            {
                if(!typeof(ICommand).IsAssignableFrom(type)) throw new Exception(nameof(ServiceLocator)+": "+type.Name + " have to implement "+nameof(ICommand)+".");
                BindEventAttribute attribute = type.GetCustomAttributes(typeof(BindEventAttribute), false).First() as BindEventAttribute;
                Subscribe(attribute?.EventName, e =>
                {
                    ( Activator.CreateInstance(type) as ICommand)?.Execute(e);
                });
            }
        }
    }
}