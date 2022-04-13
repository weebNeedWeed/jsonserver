using jsonserver.Data;
using jsonserver.Data.Entities;
using jsonserver.Data.Repositories.Interfaces;
using jsonserver.Utility.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jsonserver.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonsController : ControllerBase
    {
        private readonly IJsonRepository _jsonRepository;
        private readonly IUserRepository _userRepository;
        private readonly JsonServerContext _context;

        public JsonsController(IJsonRepository jsonRepository, IUserRepository userRepository, JsonServerContext context)
        {
            _jsonRepository = jsonRepository;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        [Route("[action]/{name:length(4,8):required}")]
        public async Task<IActionResult> Read([FromRoute] string name)
        {
            //authorizationValue match the pattern: ApiKey <key>
            StringValues authorizationValue;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationValue);

            Regex apiKeyPattern = new Regex(@"^ApiKey\s([a-zA-Z0-9^\s]{32})$");

            string _authorizationValue = authorizationValue.ToString();

            var matched = apiKeyPattern.Match(_authorizationValue);

            if (matched.Groups.Count != 2)
            {
                return Unauthorized();
            }

            // the index 1 is the "<key>" of "ApiKey <key>"
            string apiKey = matched.Groups[1].Value.ToString();

            User currUser = await _userRepository.GetByApiKeyAsync(apiKey);

            if (currUser == null)
            {
                return Unauthorized();
            }

            // If no json with name like parameter name, return NotFound
            var json = currUser.Jsons.FirstOrDefault(x => x.Name == name);

            if (json == null)
            {
                return NotFound();
            }

            return Ok(json.Content);
        }

        [HttpPost]
        [Route("[action]/{name:length(4,8):required}")]
        public async Task<IActionResult> Create([FromRoute] string name, [FromForm][Required] string content)
        {
            //authorizationValue match the pattern: ApiKey <key>
            StringValues authorizationValue;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationValue);

            Regex apiKeyPattern = new Regex(@"^ApiKey\s([a-zA-Z0-9^\s]{32})$");

            string _authorizationValue = authorizationValue.ToString();

            var matched = apiKeyPattern.Match(_authorizationValue);

            if (matched.Groups.Count != 2)
            {
                return Unauthorized();
            }

            // the index 1 is the "<key>" of "ApiKey <key>"
            string apiKey = matched.Groups[1].Value.ToString();

            User currUser = await _userRepository.GetByApiKeyAsync(apiKey);

            if (currUser == null)
            {
                return Unauthorized();
            }

            // If no json with name like parameter name, return NotFound
            var json = currUser.Jsons.FirstOrDefault(x => x.Name == name);

            if (json == null)
            {
                return NotFound();
            }

            if (!content.IsValidJson())
            {
                return StatusCode(400);
            }

            dynamic data = JsonConvert.DeserializeObject(content);

            data._id = Guid.NewGuid().ToString();

            dynamic newJsonContent = JsonConvert.DeserializeObject(json.Content);

            newJsonContent.Add(data);

            json.Content = newJsonContent.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("[action]/{name:length(4,8):required}")]
        public async Task<IActionResult> Update([FromRoute] string name, [FromForm][Required] string _id, [FromForm][Required]string content)
        {
            //authorizationValue match the pattern: ApiKey <key>
            StringValues authorizationValue;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationValue);

            Regex apiKeyPattern = new Regex(@"^ApiKey\s([a-zA-Z0-9^\s]{32})$");

            string _authorizationValue = authorizationValue.ToString();

            var matched = apiKeyPattern.Match(_authorizationValue);

            if (matched.Groups.Count != 2)
            {
                return Unauthorized();
            }

            // the index 1 is the "<key>" of "ApiKey <key>"
            string apiKey = matched.Groups[1].Value.ToString();

            User currUser = await _userRepository.GetByApiKeyAsync(apiKey);

            if (currUser == null)
            {
                return Unauthorized();
            }

            // If no json with name like parameter name, return NotFound
            var json = currUser.Jsons.FirstOrDefault(x => x.Name == name);

            if (json == null)
            {
                return NotFound();
            }

            if (!content.IsValidJson())
            {
                return StatusCode(400);
            }

            dynamic newData = JsonConvert.DeserializeObject(content);
            dynamic currContent = JsonConvert.DeserializeObject(json.Content);


            dynamic jsonObj = false;
            foreach(var _obj in currContent)
            {
                if(_obj._id == _id)
                {
                    jsonObj = _obj;
                }
            }

            if(jsonObj == null)
            {
                return NotFound();
            }

            newData._id = jsonObj._id;

            var newJsonContent = new List<dynamic>();

            foreach (var _obj in currContent)
            {
                if (_obj._id != _id)
                {
                    newJsonContent.Add(_obj);
                }
            }

            newJsonContent.Add(newData);

            json.Content = JsonConvert.SerializeObject(newJsonContent);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{name:length(4,8):required}")]
        public async Task<IActionResult> Delete([FromRoute] string name, [FromForm][Required] string _id)
        {
            //authorizationValue match the pattern: ApiKey <key>
            StringValues authorizationValue;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationValue);

            Regex apiKeyPattern = new Regex(@"^ApiKey\s([a-zA-Z0-9^\s]{32})$");

            string _authorizationValue = authorizationValue.ToString();

            var matched = apiKeyPattern.Match(_authorizationValue);

            if (matched.Groups.Count != 2)
            {
                return Unauthorized();
            }

            // the index 1 is the "<key>" of "ApiKey <key>"
            string apiKey = matched.Groups[1].Value.ToString();

            User currUser = await _userRepository.GetByApiKeyAsync(apiKey);

            if (currUser == null)
            {
                return Unauthorized();
            }

            // If no json with name like parameter name, return NotFound
            var json = currUser.Jsons.FirstOrDefault(x => x.Name == name);

            if (json == null)
            {
                return NotFound();
            }

            dynamic currContent = JsonConvert.DeserializeObject(json.Content);

            var newContent = new List<dynamic>();

            foreach(var _obj in currContent)
            {
                if(_obj._id != _id)
                {
                    newContent.Add(_obj);
                }
            }

            json.Content = JsonConvert.SerializeObject(newContent);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
