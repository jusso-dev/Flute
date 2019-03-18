using System;
using System.Linq;
using System.Threading.Tasks;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Model;

namespace Flute.Trainer.Service.Repoistory
{
	public class TrainedModelRepoistory : ITrainedModelRepoistory
	{
		private readonly UserDbContextContext _context;
		public TrainedModelRepoistory(UserDbContextContext context)
		{
			_context = context;
		}


		/// <summary>
		/// Add's new model to the database by the name of the model
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		
		public async Task<bool> AddNewModel(string modelId, string usersEmail)
		{
			try
			{
				if(string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(usersEmail))
				{
					return false;
				}

				// Disallow creation of model if user has exceeded maximum allowed models
				if(this.MaxAllowedModels(usersEmail).Result)
				{
					return false;
				}

				await _context.UploadedModels.AddAsync(new UsersUploadedModels()
				{	EmailAddress = usersEmail,
					ModelId = modelId,
					ModelUploadDateTime = DateTime.UtcNow,
					UploadedModelCount = this.CountModelsUploaded(usersEmail).Result
				});

				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Check how many models the user has uploaded
		/// </summary>
		/// <param name="usersEmail"></param>
		/// <returns></returns>
		public Task<int> CountModelsUploaded(string usersEmail)
		{
			try
			{
				int countOfModels = _context.UploadedModels
					.Take(1)
					.Where(e => e.EmailAddress == usersEmail)
					.FirstOrDefault()
					.UploadedModelCount;

				return Task.FromResult(countOfModels);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// Initially, we will not allow more than 5 models per user
		/// </summary>
		/// <param name="usersEmail"></param>
		/// <returns></returns>
		public Task<bool> MaxAllowedModels(string usersEmail)
		{
			try
			{
				var usersModels = _context.UploadedModels
					.Where(a => a.EmailAddress == usersEmail)
					.ToList();

				if(usersModels.Count >= 5)
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
	}
}
