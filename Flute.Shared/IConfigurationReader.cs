using System.Threading.Tasks;

namespace Flute.Shared
{
	public interface IConfigurationReader
	{
		Task<string> ReadConfigurationAsync(string keyName, bool readFromKeyVault)
	}
}