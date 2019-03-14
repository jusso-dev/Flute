using System;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Flute.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Trainer.Service.Controllers
{
	[Route("api/[controller]")]
    public class TrainerController : Controller
    {

		/// <summary>
		/// API endpoint that will trigger the training of the last model
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route(nameof(TrainModel))]
		public async Task<IActionResult> TrainModel()
		{
			try
			{
				return new OkObjectResult("Accepted. Training commencing..");
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}
	}
}