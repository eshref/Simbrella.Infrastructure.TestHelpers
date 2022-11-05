using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;


namespace Simbrella.LoanManagement.TestHelpers.Utils
{
    public static class UriUtils
    {
        public static string PrepareRelativeUri(Dictionary<string, string> uriParts)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);

            foreach ((string key, string value) in uriParts)
            {
                query[key] = value;
            }

            return query.ToString();
        }
    }
}