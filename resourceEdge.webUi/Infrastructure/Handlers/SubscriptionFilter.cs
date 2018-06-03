﻿using Microsoft.AspNet.Identity;
using resourceEdge.Domain.Abstracts;
using resourceEdge.Domain.Concrete;
using resourceEdge.Domain.UnitofWork;
using resourceEdge.webUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace resourceEdge.webUi.Infrastructure.Handlers
{
    public class SubscriptionFilter : FilterAttribute, IActionFilter
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
          
  
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var initializationCode = filterContext.HttpContext.Request.Form["Code"].ToString();
            var user = filterContext.HttpContext.User.Identity.GetUserId();
            var LocationDetail =(SessionModel)filterContext.HttpContext.Session["_ResourceEdgeTeneceIdentity"];
            
            if (LocationDetail != null)
            {
                var Subscribtion = unitOfWork.SubscribedAppraisal.Get(filter: x => x.GroupId == LocationDetail.GroupId && x.LocationId == LocationDetail.LocationId && x.IsActive == true).LastOrDefault();
                if (user != null && Subscribtion != null)
                {
                    var SubScribedAppraisal = unitOfWork.GetDbContext().AppraisalInitialization.Where(x => x.Id == Subscribtion.AppraisalInitializationId && x.InitilizationCode == Subscribtion.Code && x.IsActive == true).FirstOrDefault();
                    if (SubScribedAppraisal != null)
                    {
                     
                        filterContext.HttpContext.Response.StatusCode = 200;
                    }
                    else
                    {
                        var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                        filterContext.Result = new RedirectResult("~/subcription?returnUrl=" + returnUrl);
                    }
                }
               else
                {
                    
                    var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                    filterContext.Result = new RedirectResult("~/subcription?returnUrl=" + returnUrl);

                }
            }
        }
    }

}