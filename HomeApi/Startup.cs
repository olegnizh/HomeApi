using FluentValidation.AspNetCore;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HomeApi.Data;
using Microsoft.EntityFrameworkCore;


namespace HomeApi
{
    public class Startup
    {

        /// <summary>
        /// Загрузка конфигурации из файла Json
        /// </summary>
        private IConfiguration Configuration { get; } = new ConfigurationBuilder()
          .AddJsonFile("HomeOptions.json")
          .Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Подключаем автомаппинг
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            services.AddAutoMapper(assembly);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);


            // Подключаем валидацию
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());
            

            // Добавляем новый сервис
            services.Configure<HomeOptions>(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HomeApi",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
