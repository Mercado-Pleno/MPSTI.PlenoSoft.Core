using MPSTI.PlenoSoft.Core.CrossProject.Domains;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public static class ReflectionUtil
	{
		private const BindingFlags flags = BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly static Dictionary<Type, Dictionary<string, PropertyInfo>> caches = new Dictionary<Type, Dictionary<string, PropertyInfo>>();


		public static Dictionary<string, Type> GetTypes<TEntidade>() where TEntidade : IEntidade, new()
		{
			var type = typeof(TEntidade);
			var cache = GetCache(type);
			return cache.ToDictionary(c => c.Key, c => c.Value.PropertyType);
		}


		public static TEntidade Fill<TEntidade>(IDataRecord dataRecord) where TEntidade : IEntidade, new()
		{
			var entidade = new TEntidade();
			Fill(entidade, dataRecord);
			return entidade;
		}

		public static void Fill<TEntidade>(TEntidade entidade, IDataRecord dataRecord) where TEntidade : IEntidade, new()
		{
			var cache = GetCache(entidade.GetType());
			for (int i = 0; i < dataRecord.FieldCount; i++)
			{
				var key = dataRecord.GetName(i).ToUpper();
				if (cache.TryGetValue(key, out var property))
				{
					var value = dataRecord.GetValue(i);
					try
					{
						property.SetValue(entidade, value);
					}
					catch (Exception)
					{
						var enumerado = Enum.Parse(property.PropertyType, value.ToString());
						property.SetValue(entidade, enumerado);
					}
				}
			}
		}

		private static Dictionary<string, PropertyInfo> GetCache(Type type)
		{
			if (!caches.TryGetValue(type, out var cache))
				caches[type] = cache = type.GetProperties(flags).ToDictionary(pi => pi.Name.ToUpper());

			return cache;
		}

		public static Type ObterTipo(PropertyInfo propriedade)
		{
			Type baseType = propriedade.PropertyType;
			if (propriedade.PropertyType.IsGenericType && propriedade.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				baseType = propriedade.PropertyType.GetGenericArguments()[0];
			}
			return baseType;
		}
	}

}