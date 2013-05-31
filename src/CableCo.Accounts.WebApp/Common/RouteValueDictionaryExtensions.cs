using System.Web.Routing;

namespace CableCo.Accounts.WebApp.Common
{
	public static class RouteValueDictionaryExtensions
	{
		 public static void MergeFrom(this RouteValueDictionary routeValues, object additionalValues)
		 {
			 if (additionalValues != null)
			 {
				 routeValues.MergeFrom(new RouteValueDictionary(additionalValues));
			 }
		 }

		 public static void MergeFrom(this RouteValueDictionary routeValues, RouteValueDictionary additionalValues)
		 {
			 if (additionalValues != null)
			 {
				 foreach (var item in additionalValues)
				 {
					 routeValues[item.Key] = item.Value;
				 }
			 }
		 }
	}
}