﻿using System;
#if NET_40
using System.ComponentModel.DataAnnotations;
#else 
using Vmgr.CustomAttributes;
#endif
using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr.Data.Generic.HelperClasses;
using System.Collections.Generic;

namespace Vmgr.Data.Biz
{
	public static class EntityExtensions
    {
        internal static void SetForeignKey<P, F>(this IService<F> foreign, IService<P> primary, object value) where P : IEntity2 where F : IEntity2 
        {
            //  Retrieves the ID of the primaryEntity
            string keyName = primary.GetKeyName();

            PropertyInfo[] properties = foreign.MetaData.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
                if (p.Name == keyName)
                {
                    p.SetValue(foreign.MetaData, value, null);
                    break;
                }
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type.BaseType == null) return type.GetInterfaces();

            return Enumerable.Repeat(type.BaseType, 1)
                             .Concat(type.GetInterfaces())
                             .Concat(type.GetInterfaces().SelectMany<Type, Type>(GetBaseTypes))
                             .Concat(type.BaseType.GetBaseTypes());
        }

        internal static string GetKeyName<T>(this IService<T> service) where T : IEntity2
        {
            PropertyInfo[] properties = service.MetaData.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
                if (p.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() != null)
                {
                    return p.Name;
                }

            return string.Empty;
        }

        internal static void RemoveEntities<T>(this EntityCollection<T> entityCollection) where T : EntityBase2, IEntity2
        {
            entityCollection.RemovedEntitiesTracker = new EntityCollection<T>();

            EntityCollection<T> entitiesToRemove = new EntityCollection<T>();
            entitiesToRemove.AddRange(entityCollection);

            foreach (T e2r in entitiesToRemove)
                entityCollection.Remove(e2r);  // This automatically pushes removed entity to RemovedEntitiesTracker.

        }
    }
}