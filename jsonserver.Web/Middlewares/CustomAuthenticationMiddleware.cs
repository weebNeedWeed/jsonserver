﻿using Microsoft.AspNetCore.Http;
using jsonserver.Web.Extensions;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace jsonserver.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            HttpClient httpClient = new HttpClient();

            string userName = context.Session.Get<string>("UserName");
            string accessToken = context.Session.Get<string>("AccessToken");

            // User has logged in
            if(userName != null || accessToken != null)
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("jsonserver OAuth WebApp");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");

                var response = await httpClient.GetAsync("https://api.github.com/user");

                // Failed to auth
                if (!response.IsSuccessStatusCode)
                {
                    context.Session.Clear();
                }

                dynamic userDataJson = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                string newUserName = userDataJson.login.ToString();

                if(newUserName != userName)
                {
                    context.Session.Clear();
                }
            }

            await _next(context);
        }
    }
}
