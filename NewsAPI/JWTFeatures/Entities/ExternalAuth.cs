using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAPI.JWTFeatures.Entities
{
	public class ExternalAuth
	{
		public string Provider { get; set; }
		public string IdToken { get; set; }
	}
}
