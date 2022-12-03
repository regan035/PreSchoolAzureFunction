using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreSchoolAzureFunction.Models;
using System.Diagnostics;

namespace PreSchoolAzureFunction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static readonly HttpClient client= new HttpClient();
        private readonly BlobServiceClient _blobServiceClient;

        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        //http://localhost:7268/api/OnCheckUploadToQueue

        [HttpPost]
        public async Task<IActionResult> Index(CheckinRequest checkinRequest,IFormFile file)
        {
            //checkinRequest.Id = Guid.NewGuid().ToString();
            
            using (var content = new StringContent(JsonConvert.SerializeObject(checkinRequest),System.Text.Encoding.UTF8,"application/json"))
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:7212/api/RequestUploadToQueue", content);
                HttpResponseMessage httpresponse = await client.PostAsync("https://prod-15.centralus.logic.azure.com:443/workflows/0d6b862ab4624f57b5fce7c11ab1c398/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=y9tNeDlteGZnAYL-eugtxsP20iRwByxNVTMp23sfOZw", content);
                string returnValue = response.Content.ReadAsStringAsync().Result;
            }
            if(file != null)
            {
                var fileName = checkinRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("imagefunction");
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                var httpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
                return View();
            }
            return RedirectToAction(nameof(FinishRequest));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult FinishRequest()
        {
            return View();
        }
    }
}