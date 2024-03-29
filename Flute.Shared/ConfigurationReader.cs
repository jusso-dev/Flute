﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Flute.Shared
{
	/// <summary>
	/// Read configuration values from either Azure Key Vault or Environment Variables
	/// </summary>
	public class ConfigurationReader : IConfigurationReader
	{
		private static string keyVaultUri = Environment.GetEnvironmentVariable("FLUTE_KEYVAULT_ENDPOINT");


		/// <summary>
		/// Store all environment variables in central location
		/// If the const is marked KEYVAULT=True, then it's value is retrieved from Azure Key Vault, otherwise it's retrieved from env variables.
		/// </summary>

		public const string TrainerBaseApiUrl = "TrainerBaseApiUrl";
		// KEYVAULT=True
		public const string StorageConnectionString = "StorageConnectionString";
		public const string GoogleClientId = "GoogleClientId";
		// KEYVAULT=True
		public const string GoogleClintSecret = "GoogleClintSecret";

		public ConfigurationReader()
		{
		}

		/// <summary>
		/// Default configuration reader method, returns values from either Azure Key Vault or from
		/// Environment Variables on the machine.
		/// </summary>
		/// <param name="keyName">The key identifier to retrieve</param>
		/// <param name="readFromKeyVault">Whether or not to read from Azure Key Vault or from env settings.</param>
		/// <returns>string from the Configuration value</returns>
		public async Task<string> ReadConfigurationAsync(string keyName, bool readFromKeyVault)
		{
			try
			{
				if (string.IsNullOrEmpty(keyName))
					return null;

				if (readFromKeyVault)
				{
					AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
					var keyVaultClient = new KeyVaultClient(
						new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

					var secret = await keyVaultClient.GetSecretAsync(vaultBaseUrl: keyVaultUri, secretName: keyName);

					return secret.Value;
				}
				else
				{
					var key = Environment.GetEnvironmentVariable(keyName) ?? null;
					return key;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
