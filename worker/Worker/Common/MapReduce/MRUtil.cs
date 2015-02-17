using System;

namespace Common
{
	public class MRUtil
	{
		public static string normalizeIP (string ip)
		{
			if (ip == null)
				throw new ArgumentException ("Can't normalize ip = null");
			ip = ip.Trim ();
			if (!ip.EndsWith ("/"))
				ip += "/";

			return ip;
		}
	}
}

