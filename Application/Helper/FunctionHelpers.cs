using System.Globalization;

namespace Application.Helper
{
	public static class FunctionHelpers
	{
		public static string FormatCurrency(double amount)
		{
			return amount.ToString("#,##0", CultureInfo.CreateSpecificCulture("vi-VN"));
		}
	}
}
