using System.Threading.Tasks;

namespace Flute.Trainer.Service.Interfaces
{
	public interface IConfigurationReader
	{
		Task<string> ReadConfigurationAsync(string keyName, bool readFromKeyVault);
	}
}
