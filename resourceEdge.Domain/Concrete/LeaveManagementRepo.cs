﻿using resourceEdge.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using resourceEdge.Domain.Entities;

namespace resourceEdge.Domain.Concrete
{
    public class LeaveManagementRepo : ILeaveManagement
    {
        UnitofWork.UnitOfWork unitOfWork = new UnitofWork.UnitOfWork();
        public void AddLeaveManagement(LeaveManagement leave)
        {
            unitOfWork.LeaveManagement.Insert(leave);
            unitOfWork.Save();
        }

        public void AllotEmployeeLeave(EmployeeLeaves empLeave)
        {
            unitOfWork.GetDbContext().EmpLeaves.Add(empLeave);
            unitOfWork.Save();
        }

        public IEnumerable<EmployeeLeaves> GetAllotedLeave()
        {
            return unitOfWork.GetDbContext().EmpLeaves.ToList();
        }

        public void EditLeaveManagement(LeaveManagement leave)
        {
            unitOfWork.LeaveManagement.Update(leave);
            unitOfWork.Save();
        }

        public IEnumerable<LeaveManagement> GetLeave()
        {
            return unitOfWork.LeaveManagement.Get();
        }

        public LeaveManagement GetLeaveById(int? id)
        {
            return unitOfWork.LeaveManagement.GetByID(id);
        }

        public void RemoveLeave(int? id)
        {
            unitOfWork.LeaveManagement.Delete(id);
            unitOfWork.Save();
        }

        public void AddLeaveTypes(EmployeeLeaveTypes leaveType)
        {
            unitOfWork.LeaveType.Insert(leaveType);
            unitOfWork.Save();
        }

        public IEnumerable<EmployeeLeaveTypes> GetLeaveTypes()
        {
            return unitOfWork.LeaveType.Get();
        }

        public void AddLeaveRequest(LeaveRequest request)
        {
            unitOfWork.LRequest.Insert(request);
            unitOfWork.Save();
        }

        public void updateLeaveRequest(LeaveRequest leaveRequest)
        {
            unitOfWork.LRequest.Update(leaveRequest);
            unitOfWork.Save();
        }

        public void UpdateEmployeeLeave(EmployeeLeaves empLeave)
        {
            unitOfWork.EmployeeLeave.Update(empLeave);
            unitOfWork.Save();
        }

        public void EditLeaveRequest(LeaveRequest request)
        {
            unitOfWork.LRequest.Update(request);
            unitOfWork.Save();
        }

        public void DeleteLeaveRequest(int id)
        {
            unitOfWork.LRequest.Delete(id);
            unitOfWork.Save();

        }

        public IEnumerable<LeaveRequest> GetLeaveRequest()
        {
            return unitOfWork.LRequest.Get();
        }

        public EmployeeLeaves GetEmplyeeLeaveByUserId(string id)
        {
            var result = unitOfWork.EmployeeLeave.Get().ToList();
            if (result != null)
            {
              var employeeLeave =   result.Find(x => x.UserId == id);
                if (employeeLeave != null)
                {
                    return employeeLeave;
                }
            }
            return null;
        }
    }
}
