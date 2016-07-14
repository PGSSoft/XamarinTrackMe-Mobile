using System.Globalization;
using System.Reflection;
using System.Resources;
using MvvmCross.Platform;

namespace TrackMe.Core.Localization
{
	public class Localize
	{
		static readonly CultureInfo ci;

		static Localize () 
		{
			ci = Mvx.Resolve<ILocalize>().GetCurrentCultureInfo();
		}

		public static string GetString(string key, string comment)
		{
			ResourceManager temp = new ResourceManager ("UsingResxLocalization.Resx.AppResources", typeof(Localize).GetTypeInfo ().Assembly);
			string result = temp.GetString (key, ci);
			return result; 
		}
	}
}

