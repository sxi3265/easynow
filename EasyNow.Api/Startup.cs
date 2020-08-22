using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper.Internal;
using EasyNow.Api.Filters;
using EasyNow.Api.Services;
using EasyNow.Bo;
using EasyNow.Dal;
using EasyNow.Utility.Extensions;
using EasyNow.Utility.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EasyNow.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.SaveToken = true;
                opts.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuerSigningKey  = true,
                    IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890")),
                    ValidIssuer = "easynow.me",
                    ValidAudience = "api",
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //services.AddControllers(opts =>
            //{
            //    opts.Filters.Add<GlobalExceptionFilter>();
            //    opts.Filters.Add<ResultActionFilterAttribute>();
            //}).AddControllersAsServices().AddNewtonsoftJson(opts =>
            //{
            //    JsonTool.JsonSerializerSettings.CopyTo(opts.SerializerSettings);
            //});
            //services.AddApiVersioning();
            services.AddDbContext<EasyNowContext>(builder =>
            {
                builder.UseMySql("Server=mysql.20666666.xyz;Port=30000;Database=EasyNow;Uid=root;Pwd=sbxaialhj;");
            });
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddGrpc();
            services.AddAuthorization();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AssignableTo<ControllerBase>()
                .Where(e => !e.IsAbstract && !e.IsDynamic()).AsSelf().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterModule<DalModule>();
            builder.RegisterModule<BoModule>();
            builder.RegisterAutoMapper(this.GetType().Assembly, typeof(DalModule).Assembly, typeof(BoModule).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ScriptService>();
                //endpoints.MapControllers();   
            });
        }
    }
}
