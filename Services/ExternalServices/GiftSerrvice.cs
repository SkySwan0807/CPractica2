using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Services.Models;
using static System.Net.WebRequestMethods;

namespace Services.ExternalServices
{
	public class GiftSerrvice
	{
		public GiftSerrvice() { }

		public async Task<List<Electronic>> GetAllGifts()
		{
			string url = "https://api.restful-api.dev/objects";

			Log.Information("Me voy a conectar al siguiente HOST: " + url);

			HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync(url);

			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();

			List<Electronic> giftlist = JsonConvert.DeserializeObject<List<Electronic>>(responseBody);

			return giftlist;
		}
	}
}
