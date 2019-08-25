using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RNC.API.Data;

namespace RNC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ProdutoContexto>(opt =>
                opt.UseInMemoryDatabase(databaseName: "produtoInMemory")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddTransient<IProdutoRepository, ProdutoRepository>();

            services.AddApiVersioning();

            services.AddResponseCaching();

            services.AddResponseCompression(opt=>
            {
                //opt.Providers.Add<GzipCompressionProvider>();
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.EnableForHttps = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
