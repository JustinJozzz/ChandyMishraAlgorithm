using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Common.Classes;
using static Common.Classes.Node;
using System.Net.Http;
using Newtonsoft.Json;
using Common.Models;
using System.IO;

namespace Controller1
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
            HttpClient client = new HttpClient();
            Controller controller = new Controller((Locations)Enum.Parse(typeof(Locations), "controller1"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/data")
                {
                    await controller.GetData(client, (Locations)Enum.Parse(typeof(Locations), "database"), context.Request.Query["value"].ToString());
                }
                else if (context.Request.Path == "/return")
                {
                    await controller.ReturnData(client, (Locations)Enum.Parse(typeof(Locations), "database"), context.Request.Query["value"].ToString());
                }
                else if (context.Request.Path == "/probe")
                {
                    await controller.ReceiveProbe(JsonConvert.DeserializeObject<Probe>(await new StreamReader(context.Request.Body).ReadToEndAsync()));
                }
                else
                {
                    await context.Response.WriteAsync("Controller3 Up!\n");
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(controller));
                }
            });
        }
    }
}
