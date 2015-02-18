using System;
using ServiceStack;
using System.IO;
using Common;


namespace Worker
{
	public class Reducer: Service
	{

		public object Put (SendMappedData request)
		{
			System.Console.WriteLine ("NOWA WIADOMOSC DLA REDUCE " + request.value);
			if (ReducerUtils.correctid (request.chunk + request.key, request.id)) {
				System.IO.File.AppendAllText(request.key+".txt", request.value + Environment.NewLine);
			}
			return true;
		}
		public object Put (SendMapperEndWork request)
		{
			System.Console.WriteLine ("WIADOMOSC ENDWORK ");
			ReducerUtils.newEndMapper (request.chunk);
			if (ReducerUtils.recivedFromAllEndMapper (StatusConfigContainer.numberOfNodes)) {
				//dzialaj
			}
			/*
			if (ReducerUtils.newEndMapper (request.chunk)) {
				//sendtoall
			} else {
				if (ReducerUtils.recivedAllFromOneEndMapper (request.chunk, 1)) {
					//wywal z listy zadan tego mapera i jesli jakis watek na nim dziala to go zabij
				}
			}*/
			return true;
		}
	}
}

