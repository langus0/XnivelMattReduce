using System;
using System.Collections.Generic;

namespace ApiMaperReducer
{
	public abstract class ApiGlobal
	{
		public List<string> listOfNodes {get;set;}
		public void setListOfNodes(List<string> list){
			listOfNodes = list;
		}
	}
}

