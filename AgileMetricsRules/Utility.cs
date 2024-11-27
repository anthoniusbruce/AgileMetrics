namespace AgileMetricsRules
{
    public static class Utility
	{
		public static DateTime DateSkToDate(int dateSk)
		{
            int year = dateSk / 10000;
            int month = (dateSk - year * 10000) / 100;
            int day = dateSk - year * 10000 - month * 100;

            var date = new DateTime(year, month, day);
            return date;
        }

        public static int DateToDateSk(DateTime date)
        {
            var ret = date.Year * 10000 + date.Month * 100 + date.Day;
            return ret;
        }

        public static string DateToJsonDate(DateTime date)
        {
            var ret = date.ToString("u").Replace(' ', 'T');
            return ret;
        }
    }
}

