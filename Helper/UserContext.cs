﻿using System.IdentityModel.Tokens.Jwt;
using System;

namespace ProcureHub_ASP.NET_Core.Helper
{
    public class UserContext : IUserContext
    {

        private string name;
        public string email;
        private bool isSupplier = false;
        private string partnerCode;
        private bool isAdmin = false;
        private int userId = 0;

        public UserContext()
        {
            //Http
            //string authHeader = context.Request.Headers.Authorization;
            //authHeader = authHeader.Replace("Bearer ", string.Empty);
            //if (authHeader != null)
            //{
            //    var handler = new JwtSecurityTokenHandler();
            //    JwtSecurityToken token = handler.ReadJwtToken(authHeader);
            //    if (token != null)
            //    {
            //        this.SetEmail(token.Claims.First(c => c.Type == "email").Value);
            //    }
            //}

        }   

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetEmail(string email)
        {
            this.email = email;
        }

        public void SetPartnerCode(string partnerCode)
        {
            this.partnerCode = partnerCode;
        }

        public void SetIsSupplier(bool isSupplier)
        {
            this.isSupplier = isSupplier;
        }

        public void SetIsAdmin(bool isAdmin)
        {
            this.isAdmin = isAdmin;
        }

        public void SetUserId(int userId)
        {
            this.userId = userId;
        }

        public object GetUserContext()
        {
            return new
            {
                name = ""
            };
        }

        public string GetEmail()
        {
            return email;
        }

        public bool GetIsSupplier()
        {
            return isSupplier;
        }

        public int GetUserId()
        {
            return userId;
        }

        public void ExtractInfo(string jwtToken)
        {
            var authHeader = jwtToken.Replace("Bearer ", string.Empty);
            if (authHeader != null)
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadJwtToken(authHeader);
                if (token != null)
                {
                    this.email = token.Claims.First(c => c.Type == "email").Value;
                    this.isSupplier = token.Claims.First(c => c.Type == "isSupplier").Value == "True" ? true : false;
                    this.userId = Int32.Parse(token.Claims.First(c => c.Type == "userId").Value);
                }
            }
        }
    }
}
