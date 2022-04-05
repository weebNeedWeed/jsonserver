using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace jsonserver.Utility.Auth
{
    public static class Verify
    {
        public static async Task<bool> VerifyUser(string userName, string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("jsonserver OAuth WebApp");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");

            var response = await httpClient.GetAsync("https://api.github.com/user");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            string _userName = data.login.ToString();

            if(_userName != userName)
            {
                return false;
            }

            return true;
        }
    }
}
