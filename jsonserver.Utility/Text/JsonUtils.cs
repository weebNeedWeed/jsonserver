using Newtonsoft.Json;

namespace jsonserver.Utility.Text
{
    public static class JsonUtils
    {
        public static bool IsValidJson(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")))
            {
                try
                {
                    dynamic obj = JsonConvert.DeserializeObject(strInput);
                    return true;
                }
                catch // not valid
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
