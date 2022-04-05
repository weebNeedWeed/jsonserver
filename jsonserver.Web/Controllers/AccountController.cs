using jsonserver.Data;
using jsonserver.Data.Entities;
using jsonserver.Data.Repositories.Interfaces;
using jsonserver.Web.Attributes;
using jsonserver.Web.Extensions;
using jsonserver.Web.ViewModels;
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
        private readonly IUserRepository _userRepository;
        private readonly IJsonRepository _jsonRepository;
        private readonly JsonServerContext _context;

        public AccountController(IConfiguration configuration, IUserRepository userRepository, IJsonRepository jsonRepository, JsonServerContext context)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jsonRepository = jsonRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login([FromQuery]string ReturnUrl)
        {
            string baseUrl = "https://github.com/login/oauth/authorize";
            string client_id = _configuration.GetValue<string>("Github:client_id");

            Dictionary<string, string> paramList = new Dictionary<string, string>
            {
                {"client_id", client_id }
            };

            string newUrl = new Uri(QueryHelpers.AddQueryString(baseUrl, paramList)).ToString();

            ViewData["github_auth_url"] = newUrl;

            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery]string code)
        {
            HttpClient httpClient = new HttpClient();

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

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                dynamic jsonData = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                
                // No error
                if(jsonData.error == null)
                {
                    httpClient.DefaultRequestHeaders.Clear();

                    // Exchange user data
                    // All api github required user-agent
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("jsonserver OAuth WebApp");
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"token {jsonData.access_token.ToString()}");

                    dynamic userDataResponse = await httpClient.GetAsync("https://api.github.com/user");

                    if (userDataResponse.IsSuccessStatusCode)
                    {
                        var userDataJson = JsonConvert.DeserializeObject(await userDataResponse.Content.ReadAsStringAsync());

                        string userName = userDataJson.login.ToString();
                        string email = userDataJson.email.ToString();

                        // User Data not already in db
                        if (await _userRepository.GetByEmailAsync(email) == null)
                        {
                            await _userRepository.AddAsync(new User
                            {
                                UserName = userName,
                                Email = email
                            });
                        }

                        HttpContext.Session.Set<string>("UserName", userName);
                        HttpContext.Session.Set<string>("AccessToken", (string)(jsonData.access_token.ToString()));

                        if(TempData["ReturnUrl"] != null)
                        {
                            return Redirect((string)TempData["ReturnUrl"]);
                        }

                        return RedirectToAction(controllerName: "Home", actionName: "Index");
                    }

                    return RedirectToAction(controllerName: "Account", actionName: "Login");
                }

                return RedirectToAction(controllerName: "Account", actionName:"Login");
            }

            return RedirectToAction(controllerName: "Account", actionName: "Login");
        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<IActionResult> Dashboard()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                Jsons = await _jsonRepository.GetAllAsync()
            };
            
            return View(dashboardViewModel);
        }


        [HttpGet]
        [CustomAuthorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreateJsonViewModel createJsonViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(createJsonViewModel);
            }

            Json json = await _jsonRepository.GetByNameAsync(createJsonViewModel.Name);

            // If json already existed, show error
            if (json != null)
            {
                ModelState.AddModelError("", "Json Already Existed");
                return View(createJsonViewModel);
            }

            string userName = HttpContext.Session.Get<string>("UserName");

            User currUser = await _userRepository.GetByUserNameAsync(userName);

            var newJson = new Json()
            {
                Name = createJsonViewModel.Name,
                Content = "[]",
                UserId = currUser.UserId
            };

            await _jsonRepository.AddAsync(newJson);

            TempData["SuccessMessage"] = "Successfully created new json!";

            return RedirectToAction(actionName: "Dashboard", controllerName: "Account");
        }
    }
}
