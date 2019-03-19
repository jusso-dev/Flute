using Flute.Shared;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Shared.Repoistory;
using Flute.Shared.Services;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Model;
using Flute.Trainer.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
			services.AddScoped<IUserRepoistory, UserRepoistory>();
			services.AddScoped<ITrainedModelRepoistory, TrainedModelRepoistory>();

			services.AddSingleton<IMLTrainerService, MLTrainerService>();
			services.AddSingleton<IBlobStorageService, BlobStorageService>();
			services.AddSingleton<Shared.IConfigurationReader, ConfigurationReader>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			string connectionString = string.Empty;
			connectionString = @"Server=(localdb)\mssqllocaldb;Database=FluteUsers;Trusted_Connection=True;ConnectRetryCount=0";
			services.AddDbContext<UserDbContextContext>
				(options => options.UseSqlServer(
				connectionString,
				x => x.MigrationsAssembly("Flute.Shared")));
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
