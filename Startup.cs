using System;
using System.Collections.Generic;
using System.Data.Common;
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
using MySqlConnector;
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
                    //await TestHello(context);
                    await MySQLConnect(context);
                });
            });
        }

        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=storagefuncapp002;AccountKey=dFOp6RF9rLFmsiWYxnY+BYOdV5uLdtnjAEJWsgNYSoD1MPUuNF8Sj8qp+CFdJ68T30GpX5T9jTy6P+f5bZg88w==;EndpointSuffix=core.windows.net";
        private async Task TestHello(HttpContext context)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "container01";

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await GetList(containerClient, context);
            }
            catch(Exception e)
            {
                await context.Response.WriteAsync(e.ToString());
            }
        }

        private async Task MySQLConnect(HttpContext context)
        {
            string serverName = "mysql-funcapp.privatelink.mysql.database.azure.com";
            //string serverName = "10.1.0.6";
            await context.Response.WriteAsync($"MySQLConnect {serverName}" + Environment.NewLine);
            try 
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = serverName,
                    //Server = "10.1.0.6",
                    Database = "test",
                    UserID = "azureadmin@mysql-funcapp",
                    Password = "rkskekfk1234!@#$",
                    SslMode = MySqlSslMode.Required,
                };

                using (var conn = new MySqlConnection(builder.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM inventory";
                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    object obj = await reader.GetFieldValueAsync<object>(i);
                                    await context.Response.WriteAsync(obj.ToString() + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                await context.Response.WriteAsync(e.ToString());
            }
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
