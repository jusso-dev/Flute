using System;
using System.Linq;
using System.Threading.Tasks;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Model;

namespace Flute.Trainer.Service.Repoistory
{
	public class UserRepoistory : IUserRepoistory
	{
		private readonly UserDbContextContext _context;
		public UserRepoistory(UserDbContextContext context)
		{
			_context = context;
		}

		public Task<AuthenticatedUserModel> GetSingleUser(string emailAddress)
		{
			try
			{
				if(string.IsNullOrEmpty(emailAddress))
					return null;

				var singleUser = _context.Users
					.Where(e => e.EmailAddress == emailAddress)
					.Take(1)
					.FirstOrDefault();

				return Task.FromResult(singleUser);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Task<AuthenticatedUserModel> GetSingleUserByApiKey(string apiKey)
		{
			try
			{
				if (string.IsNullOrEmpty(apiKey))
					return null;

				var singleUser = _context.Users
					.Where(e => e.ApiKey == apiKey)
					.Take(1)
					.FirstOrDefault();

				return Task.FromResult(singleUser);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Task<bool> CheckForExistingUser(string emailAddress)
		{
			try
			{
				if(string.IsNullOrEmpty(emailAddress))
					return null;

				var userRecord = _context.Users
					.Take(1)
					.FirstOrDefault(a => a.EmailAddress == emailAddress);

				if (!string.IsNullOrEmpty(userRecord?.EmailAddress))
				{
					return Task.FromResult(true);
				}
				else
				{
					return Task.FromResult(false);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<bool> AddUser(string emailAddress)
		{
			try
			{
				if(string.IsNullOrEmpty(emailAddress))
					return false;

				if(this.CheckForExistingUser(emailAddress).Result)
					return false;

				await _context.Users.AddAsync(new AuthenticatedUserModel()
				{	EmailAddress = emailAddress,
					LastLogin = DateTime.UtcNow,
					ApiKey = $"FL.{Guid.NewGuid()}"
				});

				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
