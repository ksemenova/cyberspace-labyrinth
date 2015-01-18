using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CyberspaceLabyrinth
{
	public interface ILabyrinth
	{
		string GetStartRoomId ();
		Room GetRoom(string roomId);
	}

	public class Labyrinth: API, ILabyrinth
	{

		public Labyrinth (string uri, string email): base(uri, email)
		{
			
		}

		/// <summary>
		/// Gets the start room identifier.
		/// </summary>
		/// <returns>The start room identifier.</returns>
		public string GetStartRoomId()
		{
			var response = client.GetStringAsync ("start").Result;
			return JObject.Parse (response).Value<string> ("roomId");
		}

		/// <summary>
		/// Gets all info about a room
		/// </summary>
		/// <returns>The room.</returns>
		/// <param name="roomId">Room identifier.</param>
		public Room GetRoom(string roomId)
		{
			//Get all exits of the room
			var exitsResponse = client.GetStringAsync ("exits?roomId=" + HttpUtility.UrlEncode (roomId));

			//Get writing and order on the wall
			var wallResponse = client.GetStringAsync ("wall?roomId=" + HttpUtility.UrlEncode (roomId));

			//Add all info to the room
			var room = new Room ();

			//Add id to the room
			room.RoomId = roomId;


			//Get all rooms' IDs for exits
			var jsonval = JObject.Parse (exitsResponse.Result).Value<JArray> ("exits");


			if (jsonval != null) {
				var exits = jsonval.ToObject<string[]> ();

				var idsResponses = new Dictionary <string, Task<string>> (); 
				foreach (var exit in exits) {
					var exitId = client.GetStringAsync ("move?roomId=" + HttpUtility.UrlEncode (roomId)
					            + "&exit=" + HttpUtility.UrlEncode (exit));
					idsResponses.Add (exit, exitId);
				}


				//Add exits with ids to the room
				foreach (var exit in exits) {
					room.Exits.Add (exit, JObject.Parse (idsResponses [exit].Result).Value<string> ("roomId"));
				}
			}

			//Add writing and order to the room
			var result = JObject.Parse (wallResponse.Result);
			room.Writing = result.Value<string> ("writing");
			room.Order = result.Value<int> ("order");

			return room;
		}
	}
}

