﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Basics.Transformer
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

          var hasFriendClaim = principal.Claims.Any(x => x.Type == "Friend");
            if(!hasFriendClaim)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("Friend", "Bad"));
            }
            return Task.FromResult(principal);
        }
    }
}
