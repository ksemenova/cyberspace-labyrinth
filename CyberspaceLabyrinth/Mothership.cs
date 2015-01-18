using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CyberspaceLabyrinth
{
	public class Mothership: API
	{

		public Mothership (string uri, string email): base(uri, email)
		{

		}

		/// <summary>
		/// Sends report to mothership
		/// </summary>
		public string Report(string [] roomIds, string challengeCode)
		{
			var response = client.PostAsJsonAsync("report", new {roomIds = roomIds, challenge = challengeCode});

			// who really needs to check for error code .. skipping 
//			if (response.Result.IsSuccessStatusCode)
//			{
				var result = response.Result.Content.ReadAsStringAsync().Result;
				return result;
//			}  
		}
	}
}

