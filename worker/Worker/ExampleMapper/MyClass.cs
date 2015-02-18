using System;

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
}

