using System;
using System.Collections.Generic;

namespace CyberspaceLabyrinth
{

	class MainClass
	{
		//TODO: read from command arguments
		const string URI = "http://challenge2.airtime.com:7182/";
		const string Email = "K.Semyonova@gmail.com";

		static HashSet <string> visitedRoomIds = new HashSet<string>();
		static List <string> roomsIdsWithBrockenLight = new List<string>();
		static SortedList <int, string> sortedLitRooms = new SortedList<int, string>();


		/// <summary>
		/// Explores the new room. Marks the room as visited and adds to lists: roomsIdsWithBrockenLight or sortedLitRooms.
		/// </summary>
		/// <param name="roomId">Room identifier.</param>
		public static void ExploreNewRoom(string roomId, ILabyrinth labyrinth)
		{

				var room = labyrinth.GetRoom (roomId);
				visitedRoomIds.Add(roomId);
				if (room.IsLightOn)
				{
					sortedLitRooms.Add (room.Order, room.Writing);
				} 
				else
				{
					roomsIdsWithBrockenLight.Add (roomId);
				}

				foreach (var exit in room.Exits) 
				{
					if (!visitedRoomIds.Contains (exit.Value)) 
					{
						ExploreNewRoom (exit.Value, labyrinth);
					}
				}

		}

		public static void Main (string[] args)
		{

			using (var labyrinth = new Labyrinth (URI, Email)) 
			{

				var start = labyrinth.GetStartRoomId ();
				ExploreNewRoom (start, labyrinth);

			}

			var code = String.Join ("", sortedLitRooms.Values);
			var ids = roomsIdsWithBrockenLight.ToArray ();

			using(var mothership = new Mothership(URI, Email))
			{
				var res = mothership.Report (ids, code);
				Console.Write (res);
			}


		}
	}
}
