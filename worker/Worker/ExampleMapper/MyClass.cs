using System;

namespace ExampleMapper
{
	public class Mapper : ApiMaperReducer.ApiMapper
	{
		public void Run() 
		{
			string key="1", value="2";
			//robcos w while a wynik:
			send (key, value);
		}
	}
}

