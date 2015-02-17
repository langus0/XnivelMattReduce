using System;
using System.Collections.Generic;

namespace Worker
{
	public class ReducerUtils
	{
		private static Dictionary<string, int> keyList = new Dictionary<string, int> ();
		public static bool correctid(string chunk_and_key,int id)
		{
			if (keyList.ContainsKey (chunk_and_key)) {
				keyList.Add (chunk_and_key, 1);
				return true;
			}
			if (keyList [chunk_and_key] == id - 1) {
				keyList [chunk_and_key] = id;
				return true;
			} else {
				return false;
			}
		}
	}
}

