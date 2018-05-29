﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using resourceEdge.Domain.Entities;
using resourceEdge.Domain.UnitofWork;
using resourceEdge.webUi.Infrastructure.Core;
using resourceEdge.webUi.Models;
using resourceEdge.webUi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace resourceEdge.webUi.Infrastructure
{
    public static class UserManagement
    {
        private static ApplicationDbContext context = new ApplicationDbContext();
        static UnitOfWork unitOfWork = new UnitOfWork();
        public static ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public static async Task<Tuple<ApplicationUser, string>> CreateUser(
            string email, string empRole, string userstatus, string fname, string lname, string phoneNo,
            string empId, string jobId, string Comments, string createdBy, string modifiedBy, string modeofEntry, DateTime? selectedDate, string candidateReferredBy, bool? isactive
            , string DeptId, string BUnitId, int groupId, int locationId)
        {
            try
            {
                var user = new ApplicationUser()
                {

                    UserName = email,
                    Email = email,
                    EmpRole = empRole,
                    UserStatus = userstatus,
                    FirstName = fname,
                    LastName = lname,
                    UserfullName = fname + "" + lname,
                    BusinessunitId = BUnitId,
                    DepartmentId = DeptId,
                    PhoneNumber = phoneNo,
                    CreatedBy = createdBy,
                    ModifiedBy = modifiedBy,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    EmployeeId = empId,
                    ModeofEntry = modeofEntry,
                    EntryComments = Comments,
                    SelectedDate = selectedDate,
                    Candidatereferredby = candidateReferredBy,
                    JobtitleId = int.Parse(jobId),
                    Isactive = isactive,
                    GroupId = groupId,
                    LocationId = locationId
                };
                //Enable this later
                //IdentityResult validEmail = await userManager.UserValidator.ValidateAsync(user);
                //if (!validEmail.Succeeded)
                //{
                //    return "Not a valid Email Address";
                //}
                //else
                //{
                Generators Generator = new Generators();
                var password = Generator.RandomPassword();
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return null;
                }
                else
                {
                    var role = context.Roles.Find(empRole.ToString());
                    if (role == null)
                    {
                        return null;
                    }
                    else
                    {
                        var userRole = await userManager.AddToRoleAsync(user.Id.ToString(), role.Name);
                        if (!userRole.Succeeded)
                        {
                            return null;
                        }
                        else
                        {
                            //activate this later
                            //var message = $"Welcome to Tence Your Username is {user.UserName} and Password is {password}. \n Please ensure to keep this details safe.";
                            //NotificationManager manager = new NotificationManager();
                            //await manager.sendEmailNotification("Tenece", "noreply@gmail.com", message, user.Email);
                            return Tuple.Create(user, password);

                        }

                    }
                    // }
                    //}

                    //}

                }

                //Create AddManager, RemoveManager
                //Add departmental head
                //Add businessUnit head
                //

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    

        public static Employee CreateEmployee(EmployeeViewModel employees)
        {
            var UserId = HttpContext.Current.User.Identity.GetUserId();
            Employee realEmployee = new Employee();
            var UserFromSession = (SessionModel)HttpContext.Current.Session["_ResourceEdgeTeneceIdentity"];
            var unitDetail = unitOfWork.BusinessUnit.GetByID(employees.businessunitId);
            realEmployee.businessunitId = employees.businessunitId;
            realEmployee.createdby = UserId;
            realEmployee.dateOfJoining = employees.dateOfJoining;
            realEmployee.dateOfLeaving = employees.dateOfLeaving;
            realEmployee.DepartmentId = employees.departmentId;
            realEmployee.empEmail = employees.empEmail;
            realEmployee.FullName = employees.FirstName + " " + employees.lastName;
            realEmployee.empStatusId = employees.empStatusId;
            realEmployee.isactive = true;
            realEmployee.jobtitleId = employees.jobtitleId;
            realEmployee.modeofEmployement = employees.modeofEmployement;
            realEmployee.modifiedby = UserId;
            realEmployee.officeNumber = employees.officeNumber;
            realEmployee.positionId = employees.positionId;
            realEmployee.prefixId = employees.prefixId;
            realEmployee.yearsExp = employees.yearsExp;
            realEmployee.LevelId = employees.Level;
            realEmployee.LocationId = unitDetail.LocationId.Value;
            realEmployee.GroupId = UserFromSession.GroupId;
            realEmployee.isactive = true;
            var CreatedDate = realEmployee.createddate = DateTime.Now;
            var modifiedDate = realEmployee.modifieddate = DateTime.Now;
            return realEmployee;
        }
        public static bool checkEmployeeId(string id)
        {
            var user = context.Users.Where(x => x.EmployeeId == id).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public static bool checkEmail(string email)
        {
            var user = context.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public static ApplicationUser getEmployeeIdFromUserTable(string userid)
        {
            var result = context.Users.Find(userid);
            return result ?? null;
        }

    }
}