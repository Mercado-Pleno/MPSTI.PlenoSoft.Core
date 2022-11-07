using Microsoft.AspNetCore.Mvc.Rendering;
using MPSTI.PlenoSoft.Core.CrossProject.Utils;
using System;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.MVC
{
	public static class MvcEnum<TEnum> where TEnum : Enum
	{
		private static SelectListItem[] _all;
		public static SelectListItem[] ListAll() => _all ??= Enumerado.GetAll<TEnum>().Select(GetListItem).ToArray();

		private static SelectListItem GetListItem(Enumerado enumerado)
		{
			return new SelectListItem
			{
				Value = enumerado.Value.ToString(),
				Text = enumerado.Description,
				Group = GetSelectListGroup(enumerado.GroupName)
			};
		}

		private static SelectListGroup GetSelectListGroup(string groupName)
		{
			return !string.IsNullOrWhiteSpace(groupName) ? new SelectListGroup { Name = groupName } : null;
		}
	}
}