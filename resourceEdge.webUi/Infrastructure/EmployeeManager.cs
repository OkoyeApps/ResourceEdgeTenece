﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using resourceEdge.Domain.Abstracts;
using resourceEdge.Domain.Entities;
using resourceEdge.Domain.UnitofWork;
using resourceEdge.webUi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace resourceEdge.webUi.Infrastructure
{
    /// <summary>
    /// This class is used around the project as a service layer for interfaceing with the employee,
    /// Each method within this class was used to assess distinct informations about the employee and
    /// any related class.
    /// Note. that although some methods are not actually hitting the employee table
    /// the system considers some actions to be employee related.
    /// when using this class, know that it uses the unit of work to do most of its operations and thereby
    /// is serviced by a single DbContext.
    /// For modification i recommend sticking to this approach inorder to avoid exceptions and multiple DbContext
    /// In cases where the exeptions may occurs you can use the <c>GetDbContext()</c> method from the unit of work
    /// but i recommend strongly that when dealing with an entity that has to be updated ADHERE to this approach.
    /// </summary>


    public class EmployeeManager
    {
        UnitOfWork unitofWork = new UnitOfWork();
        private ILeaveManagement LeaveRepo;
        private IFiles FileRepo;
        private IEmployees EmployeeRepo;
        private IReportManager ReportManagerRepo;
        private IPayroll PayrollRepo;
        private IMailDispatcher mailDispatchRepo;
        private ApplicationDbContext db = new ApplicationDbContext();
        private resourceEdge.webUi.ApplicationUserManager userManager;

        /// <summary>
        /// This class initializes Several constructors
        /// this is so because as a result of programming to the interface any implementing 
        /// class can provide only implementation of those related to the class
        /// <example>If the Employee class needs to implement this we only use the constructor that provides implementation
        /// of details related to the employee
        /// </example>
        /// </summary>

        public EmployeeManager()
        {
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            db = new ApplicationDbContext();
        }
        public EmployeeManager(IEmployees eparam)
        {
            EmployeeRepo = eparam;
        }
        public EmployeeManager(IEmployees EParam, IReportManager RParam, IMailDispatcher mailParam)
        {
            EmployeeRepo = EParam;
            ReportManagerRepo = RParam;
            mailDispatchRepo = mailParam;
        }
        public EmployeeManager(IEmployees eParam, ILeaveManagement Lparam)
        {
            EmployeeRepo = eParam;
            LeaveRepo = Lparam;
        }
        public EmployeeManager(IEmployees EParam, IFiles fParam, ILeaveManagement lParam)
        {
            FileRepo = fParam;
            LeaveRepo = lParam;
            EmployeeRepo = EParam;
        }
        public EmployeeManager(IEmployees EParam, IFiles fParam, ILeaveManagement lParam, IPayroll payParam)
        {
            FileRepo = fParam;
            LeaveRepo = lParam;
            EmployeeRepo = EParam;
            PayrollRepo = payParam;
        }

        public EmployeeManager(IEmployees EParam, IFiles fParam, ILeaveManagement lParam, IReportManager RParam)
        {
            FileRepo = fParam;
            LeaveRepo = lParam;
            EmployeeRepo = EParam;
            ReportManagerRepo = RParam;

        }

        public class EmployeeListItem
        {
            public int empID { get; set; }
            public string userId { get; set; }
            public string empEmail { get; set; }
            public int empRoleId { get; set; }
            public string FullName { get; set; }
            public string reportingManager1 { get; set; }
            public string reportingManager2 { get; set; }
            public string empStatusId { get; set; }
            public int businessunitId { get; set; }
            public int departmentId { get; set; }

        }


        /// <summary>
        /// Every part of this class is divided into sections.
        /// the methods are organized in order of return type.
        /// This approach was used for code readabilty.
        /// <para name="id"></para>
        /// <para name="userId"></para>
        /// </summary>
        /// <returns></returns>
        public List<Employees> GetAllEmployees()
        {
            return EmployeeRepo.Get().ToList();
        }
        public List<Employees> GetEmpByBusinessUnit(int id)
        {
            var employeeByUnit = EmployeeRepo.GetEmpByBusinessUnit(id);
            if (employeeByUnit != null)
            {
                return employeeByUnit;
            }
            return null;
        }
        public string GetGroupName(int id)
        {
            var result = unitofWork.Groups.GetByID(id).GroupName;
            return result ?? null;
        }
        public List<Employees> GetReportManagerBusinessUnit(int id)
        {
            //var context = unitofWork.GetDbContext();
            List<Employees> managers = new List<Employees>();
            //var ReportManager = context.ReportManagers.Where(x => x.BusinessUnitId == id).FirstOrDefault(); //Fix this when the reportmanager is fixed
            var ReportManagers = ReportManagerRepo.GetManagersByBusinessunit(id).FirstOrDefault();
            if (ReportManagers != null)
            {
                var employees = EmployeeRepo.GetUnitHead(ReportManagers.BusinessUnitId);
                if (employees != null)
                {
                    return employees;
                }
                var result =   EmployeeRepo.GetReportManagers(ReportManagers.managerId, ReportManagers.BusinessUnitId);
                return result;
            }
            return null;
        }
        public Employees GetEmployeeByUserId(string userid)
        {
            return EmployeeRepo.GetByUserId(userid);
        }

        public Employees CheckIfEmployeeExistByUserId(string userId)
        {

            return EmployeeRepo.CheckIfEmployeeExistByUserId(userId);
        }

        public List<Employees> GetEmployeeByDept(int dept)
        {
            return EmployeeRepo.GetEmployeeByDepts(dept);
        }

        public List<Employees> GetUnitHeads(int unitId)
        {
            return EmployeeRepo.GetUnitHead(unitId);
        }

        public List<Employees> GetHr()
        {
            return EmployeeRepo.GetHrs();
        }

        public List<Employees> GetEmployeeUnitMembers(int unitId, int locationId)
        {
            var members = unitofWork.employees.Get(filter: x => x.businessunitId == unitId && x.LocationId == locationId).ToList();
            return members ?? null;
        }

        //Although this method originally called by the ApiManager, its still left here just if there is a need to use it
        public List<Employees> GetUnitMembersBySearch(string userId, string searchString)
        {
            var userUnitId = EmployeeRepo.GetByUserId(userId);
            List<Employees> TeamMembers = new List<Employees>();
            if (searchString.ToLower().StartsWith("tenece"))
            {
                var TeamByEmpId = db.Users.Where(x => x.businessunitId == userUnitId.businessunitId.ToString() && x.employeeId == searchString).FirstOrDefault();
                if (TeamByEmpId != null)
                {
                    var TeamMember = EmployeeRepo.GetByUserId(TeamByEmpId.Id);
                    TeamMembers.Add(TeamMember);
                    return TeamMembers;
                }
            }
            TeamMembers = EmployeeRepo.GetEmpByBusinessUnit(userUnitId.businessunitId).Where(x => x.empEmail.Contains(searchString) || x.FullName.Contains(searchString)).ToList();
            if (TeamMembers != null)
            {
                return TeamMembers;
            }

            return null;

        }
        //This code was used to service the employee Creation if manager role was created immediately
        //this was not used in the assign reportmanager action Method becausethe repository was invoked
        public bool AssignReportManager(ReportManagers manager)
        {
            try
            {
                ReportManagerRepo.Insert(manager);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        /// <summary>
        /// This section is the EmployeeLeaves section
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EmployeeLeaveTypes LeaveTypeById(int id)
        {
            var leave = unitofWork.GetDbContext().LeaveType.Where(x => x.id == id).SingleOrDefault();
            if (leave != null)
            {
                return leave;
            }
            return null;
        }

        /// <summary>
        /// This section is the Report Manager section
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>

        public ReportManagers getReportManagerByUserId(string userId)
        {
            var manager = ReportManagerRepo.GetByUserId(userId);
            if (manager != null)
            {
                return manager;
            }
            return null;
        }

        public List<EmployeeListItem> GetReportManagrbyUserId(string userId)
        {
            var employee = UserManagement.getEmployeeIdFromUserTable(userId); //Check if this check actually checks the user table
            if (employee != null)
            {
                int BuintId = int.Parse(employee.businessunitId);
                var Reportmanager = ReportManagerRepo.GetManagersByBusinessunit(BuintId).Select(x => new EmployeeListItem
                {
                    empID = x.employeeId,
                    businessunitId = x.BusinessUnitId,
                    departmentId = x.DepartmentId,
                    userId = x.managerId,
                    FullName = x.FullName
                }).ToList();
                return Reportmanager ?? null;
            }
            return null;
        }
        public List<ReportManagers> ExistingReportManager(string userId, int UnitId)
        {
            var validManager = ReportManagerCount(userId);
            if (validManager != true)
            {
                var manager = ReportManagerRepo.GetManagersByBusinessunit(UnitId);
                if (manager != null)
                {
                    return manager;
                }
            }
            return null;
        }
        public bool ReportManagerCount(string userId)
        {
            var result = ReportManagerRepo.GetReportmanagerCount(userId);
            if (result.Count >= 2)
            {
                return true;
            }
            return false;
        }

        public bool AddEmployeeToMailDispatch(string userName, string password, string sender, string GroupName, string Fullname)
        {
            try
            {
                MailDispatcher mail = new MailDispatcher()
                {
                    Delivered = false,
                    Message = userName + ',' + password,
                    Reciever = userName,
                    Sender = sender,
                    Subject = "Account Detail",
                    Type = Domain.Infrastructures.MailType.Account,
                    GroupName = GroupName,
                    FullName = Fullname
                };
            mailDispatchRepo.Insert(mail);
            return true;
            }catch(Exception ex)
            {
                return false;
                    throw ex;
                
            }
        }

        public class EmployeeEdit : EmployeeManager
        {

            /// <summary>
            /// This class was used to only service the Edit page for the employee. realy for abstractions
            /// This step was used because a lot of unrelated data were needed and instead of having 
            /// This section actually abstracts the code from the controller into this section
            /// </summary>
            /// <param name="model"></param>
            /// <param name="userId"></param>
            /// <param name="currentAvater"></param>
            /// <param name="filefullName"></param>
            /// <param name="File"></param>
            /// <param name="fileRepo"></param>
            /// 
            public EmployeeEdit()
            {
            }
            public EmployeeEdit(IEmployees eParam, IFiles fParam, ILeaveManagement lParam, IPayroll payParam) : base(eParam, fParam, lParam, payParam)
            {

            }
            public void AddOrUpdateAvater(Files model, string userId, Files currentAvater, string filefullName, HttpPostedFileBase File, IFiles fileRepo)
            {
                if (currentAvater == null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(File.FileName);
                    string extenstion = Path.GetExtension(File.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extenstion;
                    model.FilePath = "~/Files/Avatars/" + fileName;
                    fileName = Path.Combine(filefullName + fileName);
                    model.FileType = FileType.Avatar;
                    File.SaveAs(fileName);
                    var file = new Files()
                    {
                        FileName = model.FileName,
                        FilePath = model.FilePath,
                        UserId = userId
                    };
                    fileRepo.Insert(model);
                }
                else
                {

                    string fileName = Path.GetFileNameWithoutExtension(File.FileName);
                    string extenstion = Path.GetExtension(File.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extenstion;
                    model.FilePath = "~/Files/Avatars/" + fileName;
                    fileName = Path.Combine(filefullName + fileName);
                    model.FileType = FileType.Avatar;
                    File.SaveAs(fileName);
                    currentAvater.FileName = model.FileName;
                    currentAvater.FilePath = model.FilePath;
                    fileRepo.update(currentAvater);
                }
            }

            public string getEmpAvatar(string userId)
            {
                var avatar = FileRepo.GetFileByUserId(userId, FileType.Avatar);
                if (avatar != null)
                {
                    return avatar.FilePath.ToString();
                }
                return null;
            }

            public void AddORUpdateSalary(string userId, PayrollViewModel entity, Employees employee, ApplicationUser currentUser, string systemUserId)
            {
                var CurrentPayRoll = unitofWork.GetDbContext().Payroll.Where(x => x.UserId == userId).FirstOrDefault();

                if (CurrentPayRoll != null)
                {
                    CurrentPayRoll.BusinessUnit = entity.BusinessUnit;
                    CurrentPayRoll.Deduction = entity.Deduction;
                    CurrentPayRoll.LeaveAllowance = entity.LeaveAllowance;
                    CurrentPayRoll.Loan = entity.Loan;
                    CurrentPayRoll.Reimbursable = entity.Reimbursable;
                    CurrentPayRoll.Salary = entity.Salary;
                    CurrentPayRoll.Total = entity.Total;
                    PayrollRepo.update(CurrentPayRoll);
                }
                else
                {
                    EmpPayroll payroll = new EmpPayroll();
                    payroll.BusinessUnit = employee.businessunitId.ToString();
                    payroll.Department = employee.departmentId.ToString();
                    payroll.EmpName = employee.FullName;
                    payroll.UserId = currentUser.Id;
                    payroll.EmpStatus = employee.empStatusId;
                    payroll.Deduction = entity.Deduction;
                    payroll.LeaveAllowance = entity.LeaveAllowance;
                    payroll.Reimbursable = entity.Reimbursable;
                    payroll.ResignationDate = employee.dateOfLeaving.Value;
                    payroll.ResumptionDate = employee.dateOfJoining.Value;
                    payroll.Loan = entity.Loan;
                    payroll.Salary = entity.Salary;
                    payroll.Total = entity.Total;
                    payroll.CreatedBy = systemUserId;
                    payroll.ModifiedBy = systemUserId;
                    payroll.CreatedDate = DateTime.Now;
                    payroll.ModifiedDate = DateTime.Now;
                    payroll.Remarks = entity.Remarks;
                    PayrollRepo.Insert(payroll);
                }
            }

        }


        //Modify this class later to use the unit of work pattern later in the code.
        public class EmployeeDetails : EmployeeManager
        {
            /// <summary>
            /// This class was used to only service the Details page for the employee.
            /// This step was used because a lot of unrelated data were needed and instead of having 
            /// to hit the db always and using viewBags, this approach was used.
            /// Although the objecct return by this class is realy verbose therefore you might not want to initialize it always
            /// </summary>
            /// <param name="Id"></param>
            /// <param name="unitId"></param>
            /// <returns></returns>
            /// 


            public Tuple<Employees, ApplicationUser, Files, Jobtitles, Positions, EmpPayroll, List<LeaveRequest>> GetEmpDetails(int Id)
            {

                var employee = unitofWork.GetDbContext().Employee.Find(Id);
                if (employee != null)
                {
                    var empUserDetails = userManager.FindById(employee.userId);
                    var job = unitofWork.jobTitles.GetByID(employee.jobtitleId);
                    var position = unitofWork.positions.GetByID(employee.positionId);
                    var avatar = unitofWork.Files.Get(filter: x => x.UserId == employee.userId && x.FileType == FileType.Avatar).FirstOrDefault();
                    var Salary = unitofWork.PayRoll.Get(filter: x => x.UserId == employee.userId).SingleOrDefault();
                    var leave = unitofWork.LRequest.Get(filter: x => x.UserId == employee.userId).ToList();
                    return Tuple.Create(employee, empUserDetails, avatar, job, position, Salary, leave);
                }
                return null;
            }

            public Tuple<Employees, ApplicationUser, Files> GetAllHrDetails(int unitId)
            {
                var members = unitofWork.employees.Get(filter: x => x.businessunitId == unitId && x.empRoleId == 3).FirstOrDefault();
                ApplicationUser HrUserDetail = new ApplicationUser();
                Files Avatar = new Files();
                if (members != null)
                {
                    Avatar = unitofWork.Files.Get(filter: x => x.UserId == members.userId && x.FileType == FileType.Avatar).FirstOrDefault();
                    HrUserDetail = userManager.FindById(members.userId);
                }
                return Tuple.Create(members, HrUserDetail, Avatar);
            }

            public Tuple<Employees, ApplicationUser, Files> GetAllUnitHeadDetails(int unitId)
            {
                var members = unitofWork.employees.Get(filter: x => x.businessunitId == unitId && x.IsUnithead == true).FirstOrDefault();
                ApplicationUser HrUserDetail = new ApplicationUser();
                Files Avatar = new Files();
                if (members != null)
                {
                    Avatar = unitofWork.Files.Get(filter: x => x.UserId == members.userId && x.FileType == FileType.Avatar).FirstOrDefault();
                    HrUserDetail = userManager.FindById(members.userId);
                }

                return Tuple.Create(members, HrUserDetail, Avatar);
            }
            public Tuple<List<Employees>, List<Files>, List<ApplicationUser>> GetTeamMembersWithAvatars(int unitId)
            {
                List<Files> Images = new List<Files>();
                List<Employees> TeamMembers = new List<Employees>();
                List<ApplicationUser> TeamMemberUserDetail = new List<ApplicationUser>();
                var members = unitofWork.employees.Get(x => x.businessunitId == unitId && x.IsUnithead != true).ToList();
                if (members != null)
                {
                    foreach (var item in members)
                    {
                        var result = unitofWork.Files.Get(filter: x => x.UserId == item.userId && x.FileType == FileType.Avatar).FirstOrDefault();
                        var MemberUserDetail = userManager.FindById(item.userId);
                        Images.Add(result);
                        TeamMembers.Add(item);
                        TeamMemberUserDetail.Add(MemberUserDetail);
                    }
                }

                return Tuple.Create(TeamMembers, Images, TeamMemberUserDetail);
            }
            public List<EmloyeDetailistItem> GetAllEmployeesDetails()
            {
                var employeeQuery = unitofWork.employees.Get();

                List<EmloyeDetailistItem> employeeListItem = new List<EmloyeDetailistItem>();
                EmloyeDetailistItem listItem;
                List<Employees> employees = new List<Employees>();
                foreach (var item in employeeQuery)
                {
                    listItem = new EmloyeDetailistItem();
                    var ImagList = unitofWork.Files.Get(filter: x => x.UserId == item.userId && x.FileType == FileType.Avatar).Select(x=>x.FilePath).FirstOrDefault();
                    var userlist = userManager.FindById(item.userId);
                    bool? loginList = unitofWork.Logins.Get(filter: x => x.userId == item.userId && x.IsLogOut == false).Select(x=>x.IsLogIn).LastOrDefault();
                    var employeeUnit = unitofWork.BusinessUnit.GetByID(item.businessunitId).unitname;
                    var employeeDept = unitofWork.Department.GetByID(item.departmentId).deptname;
                    listItem.BusinessUnitName = employeeUnit;
                    listItem.DepartmentName = employeeDept;
                    listItem.IsUnitHead = item.IsUnithead;
                    listItem.EmployeeId = item.empID;
                    listItem.Login = loginList ?? false;
                    listItem.UserId = userlist.Id;
                    listItem.ImageUrl = ImagList;
                    listItem.FullName = item.FullName;
                    employeeListItem.Add(listItem);
                }
                return employeeListItem;
            }
        }

    }
    public class EmloyeDetailistItem
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string BusinessUnitName { get; set; }
        public string DepartmentName { get; set; }
        public string ImageUrl { get; set; }
        public bool? Login { get; set; }
        public int EmployeeRole { get; set; }
        public int EmployeeId { get; set; }
        public bool? IsUnitHead { get; set; }
    }
}