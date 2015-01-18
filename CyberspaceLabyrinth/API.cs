using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CyberspaceLabyrinth
{
	public class API: IDisposable
	{

		protected HttpClient client;

		public API (string uri, string email)
		{
			client = new HttpClient ();
			client.BaseAddress = new Uri (uri);
			client.DefaultRequestHeaders.Accept.Clear ();
			client.DefaultRequestHeaders.Add ("X-Labyrinth-Email", email);
		}

		public void Dispose()
		{
			client.Dispose ();
		}
	}
}

