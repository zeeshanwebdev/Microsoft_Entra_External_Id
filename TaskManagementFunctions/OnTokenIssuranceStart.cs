using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaskManagementFunctions
{
    public static class OnTokenIssuranceStart
    {
        [FunctionName("OnTokenIssuranceStart")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger _logger)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _logger.LogInformation("REQUEST");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Read userId and email from the Microsoft Entra request
            string userId = data?.data?.authenticationContext?.user?.id;
            string email = data?.data?.authenticationContext?.user?.mail;

            _logger.LogInformation($"User ID: {userId}");
            _logger.LogInformation($"User Email: {email}");

            var defaultBehaviour = new ContinueWithDefaultBehavior();

            defaultBehaviour.claims.CorrelationId = "123";
            defaultBehaviour.claims.ApiVersion = "1.0.0";
            defaultBehaviour.claims.DateOfBirth = "01/01/2000";
            defaultBehaviour.claims.CustomRoles.Add("Writer");
            defaultBehaviour.claims.CustomRoles.Add("Editor");

            defaultBehaviour.type = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";

            var actions = new List<ContinueWithDefaultBehavior>();
            actions.Add(defaultBehaviour);

            var dataObject = new Data
            {
                type = "microsoft.graph.onTokenIssuanceStartResponseData",
                actions = actions
            };

            ResponseObject response = new()
            {
                data = dataObject
            };

            _logger.LogInformation("END");



            return new JsonNetActionResult(response);
        }

        public class JsonNetActionResult : ActionResult
        {
            public Object Data { get; private set; }

            public JsonNetActionResult(Object data)
            {
                this.Data = data;
            }

            public override void ExecuteResult(ActionContext context)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Data));
            }
        }

        public class ResponseObject
        {
            public Data data { get; set; }
        }

        [JsonObject]
        public class Data
        {
            [JsonProperty("@odata.type")]
            public string type { get; set; }
            public List<ContinueWithDefaultBehavior> actions { get; set; }
        }

        [JsonObject]
        public class ContinueWithDefaultBehavior
        {
            [JsonProperty("@odata.type")]
            public string type { get; set; }
            public Claims claims { get; set; }
            public ContinueWithDefaultBehavior()
            {
                claims = new Claims();
            }

        }
        public class Claims
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string CorrelationId { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string DateOfBirth { get; set; }
            public string ApiVersion { get; set; }
            public List<string> CustomRoles { get; set; }
            public List<string> Permissions { get; set; }

            public Claims()
            {
                CustomRoles = new List<string>();
                Permissions = new List<string>();
            }
        }
    }
}
