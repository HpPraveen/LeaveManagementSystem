using LeaveManagementSystem.DAL;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Services
{
    public class LeaveStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object GetPendingLeaveConfirmationsBySupervisor(string supervisorCode)
        {
            var pendingLeaveList = _unitOfWork.LeaveEntryRepository.Get(e => e.EmployeesSupervisorCode == supervisorCode && e.LeaveStatus == "Requested").ToList();
            List<LeaveConfirmationViewModel> leaveConfirmationDtoList = new List<LeaveConfirmationViewModel>();
            foreach (var pendingLeave in pendingLeaveList)
            {
                var employeeName = _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeCode == pendingLeave.EmployeeCode).FirstOrDefault().EmployeeName;
                var leaveType = _unitOfWork.LeaveTypesRepository.Get(e => e.LeaveTypeCode == pendingLeave.LeaveTypeCode).FirstOrDefault().LeaveTypeDescription;

                var leaveConfirmationViewModel = new LeaveConfirmationViewModel
                {
                    EmployeeName = employeeName,
                    LeaveType = leaveType,
                    EmployeeCode = pendingLeave.EmployeeCode,
                    LeaveTypeCode = pendingLeave.LeaveTypeCode,
                    LeaveRequestedFromDate = pendingLeave.LeaveRequestedFromDate.ToString("dd-MM-yyyy"),
                    LeaveRequestedToDate = pendingLeave.LeaveRequestedToDate.ToString("dd-MM-yyyy"),
                    NumberOfLeaves = pendingLeave.NumberOfLeaves,
                    EmployeeComment = pendingLeave.EmployeeComment
                };
                leaveConfirmationDtoList.Add(leaveConfirmationViewModel);
            }

            return leaveConfirmationDtoList;
        }

        public object GetLeaveConfirmationsBySupervisor(string supervisorCode)
        {
            var approvedRejectedLeaveList = _unitOfWork.LeaveEntryRepository.Get(e => e.EmployeesSupervisorCode == supervisorCode && e.LeaveStatus != "Requested").ToList();
            List<LeaveConfirmationViewModel> leaveConfirmationDtoList = new List<LeaveConfirmationViewModel>();
            foreach (var approvedRejectedLeave in approvedRejectedLeaveList)
            {
                var employeeName = _unitOfWork.EmployeeMasterRepository.Get(e => e.EmployeeCode == approvedRejectedLeave.EmployeeCode).FirstOrDefault().EmployeeName;
                var leaveType = _unitOfWork.LeaveTypesRepository.Get(e => e.LeaveTypeCode == approvedRejectedLeave.LeaveTypeCode).FirstOrDefault().LeaveTypeDescription;

                var leaveConfirmationViewModel = new LeaveConfirmationViewModel
                {
                    EmployeeName = employeeName,
                    LeaveType = leaveType,
                    EmployeeCode = approvedRejectedLeave.EmployeeCode,
                    LeaveTypeCode = approvedRejectedLeave.LeaveTypeCode,
                    LeaveRequestedFromDate = approvedRejectedLeave.LeaveRequestedFromDate.ToString("dd-MM-yyyy"),
                    LeaveRequestedToDate = approvedRejectedLeave.LeaveRequestedToDate.ToString("dd-MM-yyyy"),
                    NumberOfLeaves = approvedRejectedLeave.NumberOfLeaves,
                    EmployeeComment = approvedRejectedLeave.EmployeeComment,
                    LeaveStatusUpdatedDate = approvedRejectedLeave.LeaveStatusUpdatedDate,
                    ApproverComment = approvedRejectedLeave.ApproverComment,
                    LeaveStatus = approvedRejectedLeave.LeaveStatus
                };
                leaveConfirmationDtoList.Add(leaveConfirmationViewModel);
            }

            return leaveConfirmationDtoList;
        }

        public bool UpdateEmployeeLeaveEntry(LeaveConfirmationViewModel leaveConfirmationViewModel)
        {
            try
            {
                var requestedDate = Convert.ToDateTime(leaveConfirmationViewModel.LeaveRequestedFromDate);
                var employeeLeaveEntry = _unitOfWork.LeaveEntryRepository.Get(e => e.EmployeeCode == leaveConfirmationViewModel.EmployeeCode
                && e.LeaveTypeCode == leaveConfirmationViewModel.LeaveTypeCode
                && e.LeaveRequestedFromDate == requestedDate && e.LeaveStatus == "Requested").ToList();

                if (employeeLeaveEntry.Count > 0)
                {
                    employeeLeaveEntry.FirstOrDefault().LeaveStatus = leaveConfirmationViewModel.LeaveStatus;
                    employeeLeaveEntry.FirstOrDefault().LeaveStatusUpdatedBy = leaveConfirmationViewModel.LeaveStatusUpdatedBy;
                    employeeLeaveEntry.FirstOrDefault().LeaveStatusUpdatedDate = leaveConfirmationViewModel.LeaveStatusUpdatedDate;
                    employeeLeaveEntry.FirstOrDefault().ApproverComment = leaveConfirmationViewModel.ApproverComment;

                    if (leaveConfirmationViewModel.LeaveStatus == "Rejected")
                    {
                        var result = UpdateEmployeeLeaveAllocation(leaveConfirmationViewModel);

                        if (result == false)
                        {
                            return false;
                        }
                    }

                    _unitOfWork.LeaveEntryRepository.Update(employeeLeaveEntry.FirstOrDefault());
                    _unitOfWork.SaveAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateEmployeeLeaveAllocation(LeaveConfirmationViewModel leaveConfirmationViewModel)
        {
            try
            {
                var requestedDate = Convert.ToDateTime(leaveConfirmationViewModel.LeaveRequestedFromDate);
                var employeeLeaveAllocation = _unitOfWork.LeaveAllocationRepository.Get(e => e.EmployeeCode == leaveConfirmationViewModel.EmployeeCode &&
                e.LeaveTypeCode == leaveConfirmationViewModel.LeaveTypeCode && e.Year == requestedDate.Year.ToString()).ToList();

                if (employeeLeaveAllocation.Count > 0)
                {
                    if (leaveConfirmationViewModel.LeaveStatus == "Rejected")
                    {
                        employeeLeaveAllocation.FirstOrDefault().UtilizedLeaveAmount -= leaveConfirmationViewModel.NumberOfLeaves;
                        employeeLeaveAllocation.FirstOrDefault().RemainingLeaveAmount = employeeLeaveAllocation.FirstOrDefault().EntitledLeaveAmount - employeeLeaveAllocation.FirstOrDefault().UtilizedLeaveAmount;
                    }
                    else if (leaveConfirmationViewModel.LeaveStatus == "Requested")
                    {
                        employeeLeaveAllocation.FirstOrDefault().UtilizedLeaveAmount += leaveConfirmationViewModel.NumberOfLeaves;
                        employeeLeaveAllocation.FirstOrDefault().RemainingLeaveAmount = employeeLeaveAllocation.FirstOrDefault().EntitledLeaveAmount - employeeLeaveAllocation.FirstOrDefault().UtilizedLeaveAmount;
                    }
                    _unitOfWork.LeaveAllocationRepository.Update(employeeLeaveAllocation.FirstOrDefault());
                    _unitOfWork.SaveAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}