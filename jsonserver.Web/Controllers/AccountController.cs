using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace jsonserver.Web.Controllers
{
    public class AccountController: Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            string baseUrl = "https://github.com/login/oauth/authorize";
            string client_id = _configuration.GetValue<string>("Github:client_id");

            Dictionary<string, string> paramList = new Dictionary<string, string>
            {
                {"client_id", client_id }
            };

            string newUrl = new Uri(QueryHelpers.AddQueryString(baseUrl, paramList)).ToString();

            ViewData["github_auth_url"] = newUrl;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery]string code)
        {
            string client_secret = _configuration.GetValue<string>("Github:client_secret");
            string client_id = _configuration.GetValue<string>("Github:client_id");

            string url = "https://github.com/login/oauth/access_token";

            List<KeyValuePair<string, string>> requestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("code", code)
            };

            var content = new FormUrlEncodedContent(requestBody);

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                dynamic jsonData = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                
                // No error
                if(jsonData.error == null)
                {
                    return Content(jsonData.access_token.ToString());
                }

                return RedirectToAction(controllerName: "Account", actionName:"Login");
            }

            return RedirectToAction(controllerName: "Account", actionName: "Login");
        }
    }
}
