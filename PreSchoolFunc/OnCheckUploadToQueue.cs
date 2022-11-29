using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PreSchoolFunc
{
    public static class OnCheckUploadToQueue
    {
        [FunctionName("OnCheckUploadToQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            //[Queue("CheckinRequestInBound", Connection ="AzureWebJobsStorage")] IAsyncCollector<CheckinRequest> checkinRequestQueue,
            ILogger log)
        {
            log.LogInformation("Check in/out request recived by OnCheckUploadToQueue.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            //await checkinRequestQueue.AddAsync(data);

            string responseMessage = "Checkin/out request received for"+data.Name;
            name = name ?? data?.Name;

            return new OkObjectResult(responseMessage);
        }
    }
}
