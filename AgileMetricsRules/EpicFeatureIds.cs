namespace AgileMetricsRules
{
    public class EpicFeatureIds
    {
        public const string BadRequest = "BadRequest";
        public const string NotAuthorized = "NotAuthorized";

        public static string CollapseFeatureIds(EpicFeatureIdsJsonRecord featureIds)
        {
            if (featureIds.BadRequest)
                return BadRequest;
            if (featureIds.NotAuthorized)
                return NotAuthorized;
            if (featureIds.Value == null || featureIds.Value.Count == 0)
                return string.Empty;

            string ret = string.Empty;
            foreach (var item in featureIds.Value)
            {
                ret = ret + string.Format("{0};", item.WorkItemId);
            }
            ret = ret.TrimEnd(';');

            return ret;
        }
    }
}