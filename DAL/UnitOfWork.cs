using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GenericRepository<EmployeeMaster> EmployeeMasterRepository
        {
            get
            {
                return new GenericRepository<EmployeeMaster>(_dbContext);
            }
        }

        public GenericRepository<LeaveAllocation> LeaveAllocationRepository
        {
            get
            {
                return new GenericRepository<LeaveAllocation>(_dbContext);
            }
        }

        public GenericRepository<LeaveEntry> LeaveEntryRepository
        {
            get
            {
                return new GenericRepository<LeaveEntry>(_dbContext);
            }
        }

        public GenericRepository<LeaveTypes> LeaveTypesRepository
        {
            get
            {
                return new GenericRepository<LeaveTypes>(_dbContext);
            }
        }

        public void SaveAsync()
        {
            _dbContext.SaveChanges();
        }
    }
}