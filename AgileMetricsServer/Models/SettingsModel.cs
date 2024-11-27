using System;
namespace AgileMetricsServer.Models
{
    public class SettingsModel
    {
        public const string adoTrTaxTokenLocalStoreId = "client-guid";
        public const string adoTrTaxDefaultTokenLocalStoreId = "user-guid";
        public const string adoOrganizationStoreId = "client-id";
        public string? AdoTrTaxToken { get; set; }
        public string? AdoTrTaxDefaultToken { get; set; }

        static public string ReverseToken(string token)
        {
            var charArray = token.ToCharArray();
            Array.Reverse(charArray);
            var reversed = new string(charArray);
            return reversed;
        }

    }
}

