using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        const string connectionString = "";

        private async Task TestHello(HttpContext context)
        {
            //BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            ////Create a unique name for the container
            //string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            //// Create the container and return a container client object
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            //await GetList(containerClient, context);

            await context.Response.WriteAsync("Hello World!~~~~~~~");
        }

        private async Task GetList(BlobContainerClient containerClient, HttpContext context)
        {
            List<string> items = new List<string>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
                Console.WriteLine("\t" + blobItem.Name);
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(items));
        }

        private async Task Download(BlobContainerClient containerClient, HttpContext context)
        { }

        private async Task Upload(BlobContainerClient containerClient, HttpContext context)
        { }
    }
}
