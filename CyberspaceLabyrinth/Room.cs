using System;
using System.Collections.Generic;

namespace CyberspaceLabyrinth
{
	public class Room
	{
		public string RoomId { get; set;}
		public int Order { get; set;}
		public string Writing { get; set;}
		public Dictionary <string, string> Exits { get; private set;}

		public bool IsLightOn {
			get 
			{
				return Order != -1;
			}
		}

		public Room ()
		{
			Exits = new Dictionary <string, string> ();
		}
	}
}

