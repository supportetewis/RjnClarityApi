﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace Clarity.Utilities
{
	public class ApiCaller
	{
		const string base_uri = "https://rjn-clarity-api.com/v1/clarity";

		public static T GetResponseJson<T>(Authorizer auth, string path, Dictionary<string, string> query = null)
		{

			//Generate the URL

			//Create the web request
			var request = System.Net.HttpWebRequest.Create(CreateUri(path, query));
			request.Method = "GET";

			//Add the token
			var token = auth.GetToken();
			request.Headers.Add("Authorization", $"Bearer {token}");

			//Call the api
			using (System.Net.WebResponse response = request.GetResponse())
			{
				var reader = new StreamReader(response.GetResponseStream());
				var responseJson = reader.ReadToEnd();
				var responseObj = JsonConvert.DeserializeObject<T>(responseJson);

				return responseObj;
			}
		}

		static string CreateUri(string path, Dictionary<string, string> query)
		{
			//Add the query parameters if provided
			string q = null;
			if (query != null)
			{
				q = "";
				foreach (var param in query)
				{
					q += q.Length > 0 ? "," : "?" + param.Key + "=" + System.Web.HttpUtility.UrlEncode( param.Value);
				}
			}

			//Concatenate the path
			return base_uri + path + q;
		}
	}
}
