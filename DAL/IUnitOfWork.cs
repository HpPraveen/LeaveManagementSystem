using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.DAL
{
    public interface IUnitOfWork
    {
        GenericRepository<EmployeeMaster> EmployeeMasterRepository { get; }
        GenericRepository<LeaveAllocation> LeaveAllocationRepository { get; }
        GenericRepository<LeaveEntry> LeaveEntryRepository { get; }
        GenericRepository<LeaveTypes> LeaveTypesRepository { get; }

        void SaveAsync();
    }
}