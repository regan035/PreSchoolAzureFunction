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
            return RedirectToAction(nameof(Index));
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
    }
}