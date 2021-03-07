using LeaveManagementSystem.DAL;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Services
{
    public class LeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LeaveStatusService _leaveStatusService = new LeaveStatusService(new UnitOfWork(new ApplicationDbContext()));

        public LeaveRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object GetAllLeaveTypes()
        {
            return _unitOfWork.LeaveTypesRepository.Get().ToList();
        }

        public string GetLeavePackageByEmployee(string employeeCode)
        {
            return _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeCode == employeeCode).FirstOrDefault().EmployeeLeavePacakge.ToString();
        }

        public string GetSupervisorByEmployee(string employeeCode)
        {
            var supervisorCode = _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeCode == employeeCode).FirstOrDefault().EmployeeSupervisorCode;
            return _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeSupervisorCode == supervisorCode).FirstOrDefault().EmployeeName;
        }

        public string GetSupervisorCodeByEmployee(string employeeCode)
        {
            return _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeCode == employeeCode).FirstOrDefault().EmployeeSupervisorCode;
        }

        public object GetAllLeaveByEmployee(string employeeCode, int? year)
        {
            var leaveList = _unitOfWork.LeaveEntryRepository.Get(e => e.EmployeeCode == employeeCode && e.LeaveRequestedFromDate.Year == year).ToList();
            List<LeaveConfirmationViewModel> leaveConfirmationDtoList = new List<LeaveConfirmationViewModel>();
            foreach (var leave in leaveList)
            {
                var leaveType = _unitOfWork.LeaveTypesRepository.Get(e => e.LeaveTypeCode == leave.LeaveTypeCode).FirstOrDefault().LeaveTypeDescription;

                var leaveConfirmationViewModel = new LeaveConfirmationViewModel
                {
                    Year = leave.LeaveRequestedFromDate.Year.ToString(),
                    LeaveType = leaveType,
                    EmployeeCode = leave.EmployeeCode,
                    LeaveTypeCode = leave.LeaveTypeCode,
                    LeaveRequestedFromDate = leave.LeaveRequestedFromDate.ToString("dd-MM-yyyy"),
                    LeaveRequestedToDate = leave.LeaveRequestedToDate.ToString("dd-MM-yyyy"),
                    NumberOfLeaves = leave.NumberOfLeaves,
                    EmployeeComment = leave.EmployeeComment,
                    ApproverComment = leave.ApproverComment,
                    LeaveStatus = leave.LeaveStatus
                };
                leaveConfirmationDtoList.Add(leaveConfirmationViewModel);
            }

            return leaveConfirmationDtoList;
        }

        public int GetRemainingLeaveAmountByEmployee(string employeeCode, int? year, string leaveTypeCode)
        {
            int amount = 0;
            var result = _unitOfWork.LeaveAllocationRepository.Get(e => e.EmployeeCode == employeeCode && e.Year == year.ToString() && e.LeaveTypeCode == leaveTypeCode).FirstOrDefault();

            if (result != null)
            {
                return result.RemainingLeaveAmount;
            }
            return amount;
        }

        public bool AddEmployeeLeaveEntry(LeaveEntry leaveEntry)
        {
            try
            {
                var LeaveConfirmationViewModel = new LeaveConfirmationViewModel
                {
                    EmployeeCode = leaveEntry.EmployeeCode,
                    LeaveTypeCode = leaveEntry.LeaveTypeCode,
                    LeaveRequestedFromDate = leaveEntry.LeaveRequestedFromDate.ToString(),
                    LeaveStatus = leaveEntry.LeaveStatus,
                    NumberOfLeaves = leaveEntry.NumberOfLeaves
                };
                var result = _leaveStatusService.UpdateEmployeeLeaveAllocation(LeaveConfirmationViewModel);

                if (result == false)
                {
                    return false;
                }

                _unitOfWork.LeaveEntryRepository.Insert(leaveEntry);
                _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}