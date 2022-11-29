using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreSchoolFunct.Models;

namespace PreSchoolFunct
{
    public static class CheckinRequestWriteToQueue
    {
        [FunctionName("CheckinRequestWriteToQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("CheckinRequestInBound",Connection ="AzureWebJobsStorage")] IAsyncCollector<CheckinRequest> checkinRequestQueue,
            ILogger log)
        {
            log.LogInformation("Check in/out request received by CheckinRequestWriteToQueue function");

           // string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CheckinRequest data = JsonConvert.DeserializeObject<CheckinRequest>(requestBody);

            await checkinRequestQueue.AddAsync(data);

            string responseMessage = "Check in/out request has been received for "+data.Name;

            return new OkObjectResult(responseMessage);
        }
    }
}
