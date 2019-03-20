using Flute.Client.Interfaces;
using Flute.Client.Models;
using Flute.Client.Services;
using Flute.Shared;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Shared.Repoistory;
using Flute.Shared.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flute.Client
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var _config = new ConfigurationReader();

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.HttpOnly = HttpOnlyPolicy.Always;
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddSingleton<IBlobStorageService, BlobStorageService>();
			services.AddSingleton<IConfigurationReader, ConfigurationReader>();
			services.AddSingleton<ITrainerService, TrainerService>();

			services.AddScoped<IUserRepoistory, UserRepoistory>();
			services.AddScoped<ITrainedModelRepoistory, TrainedModelRepoistory>();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})

			.AddCookie(options =>
			{
				options.LoginPath = "/account/login";
				options.LogoutPath = "/account/logout";
			})
			.AddGoogle(googleOptions => 
			{
				googleOptions.ClientId = _config.ReadConfigurationAsync(ConfigurationReader.GoogleClientId, readFromKeyVault:false).Result;
				googleOptions.ClientSecret = _config.ReadConfigurationAsync(ConfigurationReader.GoogleClintSecret, readFromKeyVault:true).Result;
				googleOptions.SaveTokens = true;
			});

			services.AddHttpClient();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			string connectionString = string.Empty;
			connectionString = @"Server=(localdb)\mssqllocaldb;Database=FluteUsers;Trusted_Connection=True;ConnectRetryCount=0";
			services.AddDbContext<UserDbContextContext>
				(options => options.UseSqlServer(
				connectionString,
				x => x.MigrationsAssembly("Flute.Shared")));
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
