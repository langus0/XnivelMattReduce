using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Mono.Math;

namespace Common
{
	public class HashUtils
	{
		private static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		private string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
		public string makeHashKey (string key)
		{
			return CalculateMD5Hash (key);
		}
		//zastanawiam sie jeszcze ktory wybrac
		public List<int> makeListOfIdsFromKey(string key,List<String> ListOfNodes)
		{
			int tmp;
			List<int> returnList = new List<int> ();
			tmp =(int) ((new BigInteger (GetBytes (key)))%ListOfNodes.Count);
			returnList.Add (tmp);
			return returnList;
		}
		public string[] makeListOfAddressesFromKey(string key,string[] ListOfNodes)
		{
			int tmp;
			List<String> returnList = new List<String> ();
			tmp = (int) ((new BigInteger (GetBytes (key)))%ListOfNodes.Length);
			returnList.Add (ListOfNodes[tmp]);
			return returnList.ToArray();
		}

		public static int getReducerIDFromKey(string key, string[] ListOfNodes)
		{
			return  (int) ((new BigInteger (GetBytes (key)))%ListOfNodes.Length);
		}
	}
}

