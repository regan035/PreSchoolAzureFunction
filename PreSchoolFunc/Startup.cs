using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PreSchoolFunc;
using PreSchoolFunc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//[assembly:WebJobsStartup(typeof(Startup))]
//namespace PreSchoolFunc
//{
//    public class Startup : IWebJobsStartup
//    {
//        public void Configure(IWebJobsBuilder builder)
//        {
//            string connectionString=Environment.GetEnvironmentVariable("AzureSqlDatabase");

//            builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

//            builder.Services.BuildServiceProvider();
//        }
//    }
//}
