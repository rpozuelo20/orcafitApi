using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using orcafitApi.Data;
using orcafitApi.Helpers;
using orcafitApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafitApi
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
            //  Cadenas de conexion:
            string cadenasqlcasa = this.Configuration.GetConnectionString("cadenasqlcasa");
            string cadenasqlazure = this.Configuration.GetConnectionString("cadenasqlazure");
            //  Acceso a datos SQL:
            services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();
            services.AddTransient<IRepositoryRutinas, RepositoryRutinas>();
            services.AddDbContext<orcafitContext>(options => options.UseSqlServer(cadenasqlazure));
            //  Habilitacion de la seguridad JwtOAuth:
            HelperOAuthToken helper = new HelperOAuthToken(this.Configuration);
            services.AddAuthentication(helper.GetAuthenticationOptions()).AddJwtBearer(helper.GetJwtOptions());
            //  Inyeccion del Helper:
            services.AddTransient<HelperOAuthToken>(x => helper);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "orcafitApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "orcafitApi v1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication(); // Realizamos el uso de Authentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
