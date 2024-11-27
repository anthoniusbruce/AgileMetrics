namespace AgileMetricsRules
{
    public class FeatureChildren
    {
        public static Dictionary<string, List<FeatureChildPoint>> CalculateChildrenDataPoints(FeatureChildrenJsonRecord featureChildren)
        {
            var ret = new Dictionary<string, List<FeatureChildPoint>>();

            if (featureChildren.Value == null)
                return ret;

            var map = new Dictionary<string, Dictionary<int, int>>();
            foreach (var item in featureChildren.Value)
            {
                var state = "";
                if (item.State != "Closed" && item.State != "Resolved" && item.State != "Active" && item.State != "New")
                    state = "theRest";
                else
                    state = item.State;

                Dictionary<int, int> secondary;
                if (map.ContainsKey(state))
                    secondary = map[state];
                else
                {
                    secondary = new Dictionary<int, int>();
                    map[state] = secondary;
                }

                if (secondary.ContainsKey(item.DateSK))
                    secondary[item.DateSK] = secondary[item.DateSK] + 1;
                else
                    secondary[item.DateSK] = 1;
            }

            foreach (var item in map.Keys)
            {
                var secondary = map[item];

                foreach (var interior in secondary.Keys)
                {
                    DateTime date = Utility.DateSkToDate(interior);

                    if (!ret.ContainsKey(item))
                        ret[item] = new List<FeatureChildPoint>();

                    ret[item].Add(new FeatureChildPoint { Date = date, Count = secondary[interior] });
                }
            }

            return ret;
        }
    }
}

