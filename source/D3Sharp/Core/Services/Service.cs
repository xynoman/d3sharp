﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public uint ServiceID { get; private set; }
        public uint Hash { get; private set; }

        public ServiceAttribute(uint serviceID, uint serviceHash)
        {
            this.ServiceID = serviceID;
            this.Hash = serviceHash;
        }

        public ServiceAttribute(uint serviceID, string serviceName)
            : this(serviceID, StringHashHelper.HashString(serviceName))
        {
        }
    }

    public static class Service
    {
        private static uint _notImplementedServiceCounter = 99;
        private readonly static Dictionary<Type, ServiceAttribute> ProvidedServices = new Dictionary<Type, ServiceAttribute>();
        private readonly static Dictionary<Type, IService> Services = new Dictionary<Type, IService>();
       
        static Service()
        {
            foreach (var type in Assembly.GetEntryAssembly().GetTypes().Where(type => type.GetInterface("IServerService") != null))
            {
                object[] attributes = type.GetCustomAttributes(typeof(ServiceAttribute), true); // get the attributes of the packet.
                if (attributes.Length == 0) return;

                ProvidedServices.Add(type, (ServiceAttribute)attributes[0]);
                Services.Add(type, (IService)Activator.CreateInstance(type));
            }
        }

        public static IService GetByID(uint serviceID)
        {
            return (from pair in ProvidedServices let serviceInfo = pair.Value where serviceInfo.ServiceID == serviceID select Services[pair.Key]).FirstOrDefault();
        }

        public static uint GetByHash(uint serviceHash)
        {
            foreach (var serviceInfo in ProvidedServices.Select(pair => pair.Value).Where(serviceInfo => serviceInfo.Hash == serviceHash))
            {
                return serviceInfo.ServiceID;
            }

            return _notImplementedServiceCounter++;
        }
    }
}
