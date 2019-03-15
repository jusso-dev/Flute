﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared;
using Flute.Shared.Interfaces;
using Flute.Shared.Repoistory;
using Flute.Shared.Services;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flute.Trainer.Service
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
			services.AddSingleton<IMLTrainerService, MLTrainerService>();
			services.AddSingleton<IBlobStorageService, BlobStorageService>();
			services.AddSingleton<ITrainerRepoistroy, TrainerRepoistroy>();
			services.AddSingleton<Shared.IConfigurationReader, ConfigurationReader>();
			services.AddSingleton<ITrainerService, TrainerService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
