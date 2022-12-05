using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreSchoolAzureFunction.Models;

namespace PreSchoolAzureFunc
{
    public static class RequestUploadToQueue
    {
        [FunctionName("RequestUploadToQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("CheckinRequestInBound",Connection ="AzureWebJobsStorage")] IAsyncCollector<CheckinRequest> checkinRequestQueue,
            ILogger log)
        {
            log.LogInformation("Checkin/out request recieved by RequestUploadToQueue function.");

           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CheckinRequest data = JsonConvert.DeserializeObject<CheckinRequest>(requestBody);
            
            await checkinRequestQueue.AddAsync(data);
            string responseMessage = "Checkin/out request has been received for" +data.Name;

            return new OkObjectResult(responseMessage);
        }
    }
}
