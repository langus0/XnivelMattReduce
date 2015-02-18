using System;
using System.Collections.Generic;

namespace ApiMaperReducer
{
	public abstract class ApiGlobal
	{
		public string[] listOfNodes {get;set;}
		public void setListOfNodes(string[] list){
			listOfNodes = list;
		}
	}
}

