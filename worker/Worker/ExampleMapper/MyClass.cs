using System;

namespace ExampleMapper
{
	public class Mapper : ApiMaperReducer.ApiMapper
	{
		public void Run(string line) 
		{
			string[] words = line.Split(' ');
			foreach (string word in words)
			{
				send (word, word);
			}
		}
	}
}

