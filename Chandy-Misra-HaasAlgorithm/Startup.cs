using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Common.Classes;
using System.Net.Http;
using static Common.Classes.Node;
using System.IO;

namespace Chandy_Misra_HaasAlgorithm
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Database database = new Database((Locations)Enum.Parse(typeof(Locations), "database"));
            HttpClient client = new HttpClient();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/data")
                {
                    string data = await database.GetData(context.Request.Query["value"].ToString());
                    await context.Response.WriteAsync(data);

                }
                else if (context.Request.Path == "/return")
                {
                    await database.ReturnTable(await new StreamReader(context.Request.Body).ReadToEndAsync());
                }
                else
                {
                    await context.Response.WriteAsync("Database Up!\n");
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(database.Tables));
                }
            });
        }
    }
}
