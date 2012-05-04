using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Claims;

namespace Claim_Providers
{
	class WorksUserInfo
	{
		public static List<Claim> GetClaimsForUser(string username)
		{
			List<Claim> userClaims = new List<Claim>();
			foreach (string userInfo in userDB)
			{
				string[] claims = userInfo.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
				if (username == claims[0])
				{
					userClaims.Add(new Claim(GetClaimTypeForRole(claims[1]), claims[2], Microsoft.IdentityModel.Claims.ClaimValueTypes.String));
				}
			}

			return userClaims;
		}

		public static List<string> GetAllUsers()
		{
			List<string> allUsers = new List<string>();
			return allUsers;
		}
	}
}
