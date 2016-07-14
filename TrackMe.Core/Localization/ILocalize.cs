using System.Globalization;

namespace TrackMe.Core.Localization
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCultureInfo();
		void SetLocale ();
	}
}

