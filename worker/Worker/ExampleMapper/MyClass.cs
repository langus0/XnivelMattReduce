using System;
using System.Collections.Generic;

namespace ExampleMapper
{
	public class Mapper : ApiMaperReducer.ApiMapper
	{

		public override void map(string line) 
		{
			string[] words = line.Split(' ');
			foreach (string word in words)
			{
				System.Console.WriteLine ("zaraz wysle");
				send (word, word);
			}
		}
	}

	public class Reducer:ApiMaperReducer.ApiReducer
	{
		public override void reduce(string key, List<string> values){
			send (key, values.Count.ToString());
		}
	}
}

