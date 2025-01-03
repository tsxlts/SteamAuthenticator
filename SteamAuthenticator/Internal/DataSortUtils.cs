
using static Steam_Authenticator.Model.EcoUser;

namespace Steam_Authenticator.Internal
{
    internal class DataSortUtils
    {
        public static int Compare(Type valueType, object value1, object value2)
        {
            if (valueType == typeof(int) || valueType == typeof(long) || valueType == typeof(decimal) || valueType == typeof(double) || valueType == typeof(float))
            {
                decimal.TryParse(value1?.ToString(), out decimal v1);
                decimal.TryParse(value2?.ToString(), out decimal v2);

                if (v1 > v2)
                {
                    return 1;
                }
                else if (v1 < v2)
                {
                    return -1;
                }

                return 0;
            }

            if (valueType == typeof(DateTime))
            {
                DateTime.TryParse(value1?.ToString(), out DateTime v1);
                DateTime.TryParse(value2?.ToString(), out DateTime v2);

                if (v1 > v2)
                {
                    return 1;
                }
                else if (v1 < v2)
                {
                    return -1;
                }

                return 0;
            }

            if (valueType == typeof(TimeOnly))
            {
                TimeOnly.TryParse(value1?.ToString(), out TimeOnly v1);
                TimeOnly.TryParse(value2?.ToString(), out TimeOnly v2);

                if (v1 > v2)
                {
                    return 1;
                }
                else if (v1 < v2)
                {
                    return -1;
                }

                return 0;
            }

            if (valueType == typeof(TimeSpan))
            {
                TimeSpan.TryParse(value1?.ToString(), out TimeSpan v1);
                TimeSpan.TryParse(value2?.ToString(), out TimeSpan v2);

                if (v1 > v2)
                {
                    return 1;
                }
                else if (v1 < v2)
                {
                    return -1;
                }

                return 0;
            }

            if (valueType == typeof(TimeRange))
            {
                TimeRange v1 = value1 as TimeRange;
                TimeRange v2 = value2 as TimeRange;

                if (v1?.Start > v2?.Start)
                {
                    return 1;
                }
                else if (v1?.Start < v2?.Start)
                {
                    return -1;
                }

                return 0;
            }

            if (typeof(System.Collections.IList).IsAssignableFrom(valueType))
            {
                var value1s = value1 as System.Collections.IList;
                var value2s = value2 as System.Collections.IList;

                value1 = null;
                value2 = null;
                if (value1s.Count > 0)
                {
                    value1 = value1s[0];
                }
                if (value2s.Count > 0)
                {
                    value2 = value2s[0];
                }
                if (value1 == null && value2 == null)
                {
                    return 0;
                }

                var type = value1 != null ? value1.GetType() : value2.GetType();
                return Compare(type, value1, value2);
            }

            return string.Compare(value1?.ToString(), value2?.ToString());
        }
    }
}
