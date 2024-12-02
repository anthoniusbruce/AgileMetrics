using System;
namespace AgileMetricsServer.Models
{
    public class SettingsModel
    {
        public const string adoTokenLocalStoreId = "client-guid";
        public string? AdoToken { get; set; }

        static public string ReverseToken(string token)
        {
            var charArray = token.ToCharArray();
            Array.Reverse(charArray);
            var reversed = new string(charArray);
            return reversed;
        }

    }
}

