using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.IdentityModel.Claims;
using System.Collections;

namespace Claim_Providers
{
	class WorksClaimTypes
	{
		public static string Role = "http://www.works.com/ws/2009/12/identity/claims/WorksRole";
		public static string Region = "http://www.works.com/ws/2009/12/identity/claims/WorksRegion";
	}
}
