using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Services.Models;
using static System.Net.WebRequestMethods;

namespace Services.ExternalServices
{
	public class GiftSerrvice
	{
		//private IConfiguration _configuration;

		public GiftSerrvice()
		{
			//IConfiguration _configuration = new ConfigurationBuilder().Build();
		}

		public async Task<List<Electronic>> GetAllGifts()
		{
			try
			{
				string url = "https://api.restful-api.dev/objects";

				Log.Information("Trying to connect to this URL: " + url);

				HttpClient client = new HttpClient();

				HttpResponseMessage response = await client.GetAsync(url);

				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();

				List<Electronic> giftlist = JsonConvert.DeserializeObject<List<Electronic>>(responseBody);

				Log.Information("Connection Successful!!!");

				return giftlist;

			} catch (Exception ex)
			{
				Log.Error("Failed Conecction: " + ex.Message);
				throw ex;
			}
		}
	}
}
