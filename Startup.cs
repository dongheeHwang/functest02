using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace WebAppTest01
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await TestHello(context);
                });
            });
        }

        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=storagefuncapp001;AccountKey=uH+8gFH31xEQaVAnVIl6Oj42J/hkyWmkpN04h6d2ols1nsdB8HjyUTqvKuP3ST9k3xtWy8H2QJwHHWLJbhacOA==;EndpointSuffix=core.windows.net";
        const string endpoint = "https://storagefuncapp001.privatelink.blob.core.windows.net";
        private async Task TestHello(HttpContext context)
        {
            try
            {
                string accountName = "storagefuncapp001";
                string accountKey = "uH+8gFH31xEQaVAnVIl6Oj42J/hkyWmkpN04h6d2ols1nsdB8HjyUTqvKuP3ST9k3xtWy8H2QJwHHWLJbhacOA==";
                //BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                //BlobClient blobClient = new BlobClient()
                await context.Response.WriteAsync(endpoint);
                BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(endpoint), storageSharedKeyCredential);
                //Create a unique name for the container
                string containerName = "container01";

                // Create the container and return a container client object
                //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                await GetList(containerClient, context);
            }
            catch(Exception e)
            {
                await context.Response.WriteAsync(e.ToString());
            }


            //try
            //{
            //    HttpClient client = new HttpClient();
            //    var respone = await client.GetByteArrayAsync("http://www.fss.or.kr/fss/kr/bbs/list.jsp?bbsid=1207404857348&url=/fss/kr/1207404857348");
            //    var responeseString = Encoding.UTF8.GetString(respone);
            //    await context.Response.WriteAsync(responeseString);
            //}
            //catch(Exception e)
            //{
            //    await context.Response.WriteAsync(e.ToString());
            //}
        }

        private async Task GetList(BlobContainerClient containerClient, HttpContext context)
        {
            try
            { 
                List<string> items = new List<string>();
                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    items.Add(blobItem.Name);
                    //Console.WriteLine("\t" + blobItem.Name);
                }
                await context.Response.WriteAsync(JsonConvert.SerializeObject(items));
            }
            catch(Exception e)
            {
                await context.Response.WriteAsync(e.ToString());
            }

        }

        private async Task Download(BlobContainerClient containerClient, HttpContext context)
        {
            //containerClient.
        }

        private async Task Upload(BlobContainerClient containerClient, HttpContext context)
        { }
    }
}
