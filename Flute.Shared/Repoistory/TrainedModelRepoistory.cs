using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;

namespace Flute.Shared.Repoistory
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
		
		public async Task<bool> AddNewModel(string modelId, string usersEmail, string modelFriendlyName)
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
					UploadedModelCount = this.ReturnUserModels(usersEmail).Result.Count,
					ModelFriendlyName = modelFriendlyName
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
		/// Returns all trained models for a given user
		/// </summary>
		/// <param name="usersEmail"></param>
		/// <returns></returns>
		public Task<List<UsersUploadedModels>> ReturnUserModels(string usersEmail)
		{
			try
			{

				if(string.IsNullOrEmpty(usersEmail))
				{
					return null;
				}

				var usersModels = _context.UploadedModels
					.Where(e => e.EmailAddress == usersEmail)
					.OrderByDescending(a => a.ModelUploadDateTime)
					.ToList();

				return Task.FromResult(usersModels); 
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
		public Task<UsersUploadedModels> CountModelsUploaded(string usersEmail)
		{
			try
			{
				var countOfModels = _context.UploadedModels
					.Where(e => e.EmailAddress == usersEmail)
					?.FirstOrDefault();

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
