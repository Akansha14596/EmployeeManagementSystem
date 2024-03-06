using Microsoft.Extensions.Configuration;
using System.Text;
using EmpManageApp.Application.Repositories;
using EmpManageApp.Infrastructure.Repositories;
using EmpManageApp.Infrastructure.Data;
using System.Data;
using System.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EmpManageApp.Presentation.Service;

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
            var key = Encoding.ASCII.GetBytes("MySecureKey12345678@#$%^&*(@$%)(*&^%");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddTransient<IDbConnection>((sp) =>
            {
                return new SqlConnection(Configuration.GetConnectionString("EmployeeDB"));
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            /*  services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
              });*/

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });

                // Add security definition for Bearer token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add security requirement for Bearer token
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
            });

            services.AddTransient<UserAuthenticationService>();

            services.AddAuthorization();
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
