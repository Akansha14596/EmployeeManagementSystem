using Microsoft.Extensions.Configuration;
using System.Text;
using EmpManageApp.Application.Repositories;
using EmpManageApp.Infrastructure.Repositories;
using EmpManageApp.Infrastructure.Data;
using System.Data;
using System.Data.SqlClient;
using Microsoft.OpenApi.Models;

namespace EmpManageApp.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
           /* services.AddSingleton<DatabaseSettings>(new DatabaseSettings
            {
                ConnectionString = Configuration.GetConnectionString("EmployeeDB")
            });*/
            services.AddTransient<IDbConnection>((sp) =>
            {
                return new SqlConnection(Configuration.GetConnectionString("EmployeeDB"));
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
            });

            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc();
        }

        //Use Configure method to configure the HTTP request pipeline. used to set up middlewares, routing rules, etc
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management System");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
