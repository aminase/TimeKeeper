﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repository;

namespace TimeKeeper.API.Helper
{
    public class TimeKeeperAuthAttribute:AuthorizationFilterAttribute
    {
        private string _roles;
        public TimeKeeperAuthAttribute(string Roles = "")
        {
            _roles = Roles == "" ? "Admin,Lead,User" : Roles;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            UnitOfWork timeUnit = new UnitOfWork();
            ModelFactory factory = new ModelFactory();
            UserModel CurrentUser = new UserModel();
            Dictionary<string, string> token = new Dictionary<string, string>();
            string provider = "iserver";

            try
            {
                string id_token = actionContext.Request.Headers.Authorization.Parameter;
                if (HttpContext.Current.Request.Headers["Provider"] != null) provider = actionContext.Request.Headers.GetValues("Provider").FirstOrDefault();
                token = TokenUtility.GenToken(id_token);
                Employee emp = new Employee();
                string field = provider == "iserver" ? "sub" : "email";
                emp = timeUnit.Employees.Get(x => x.Email == token[field]).FirstOrDefault();
                if (emp != null) CurrentUser = factory.Create(emp, provider);
                if (_roles.Contains(CurrentUser.Role)) return;
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}