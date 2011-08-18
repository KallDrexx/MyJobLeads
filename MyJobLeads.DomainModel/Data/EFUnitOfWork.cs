using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Entities.EF;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Transactions;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.Entities.History;
using System.Data.Entity.Validation;
using MyJobLeads.DomainModel.Entities.Configuration;

namespace MyJobLeads.DomainModel.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        protected MyJobLeadsDbContext _context;
        protected TransactionScope _transaction;

        public EFUnitOfWork()
        {
            _context = new MyJobLeadsDbContext();
        }

        #region Unit Of Work Methods

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            // Open the transaction scope
            _transaction = new TransactionScope();
        }

        public void EndTransaction(bool commit)
        {
            if (_transaction == null)
                return;

            if (commit)
                _transaction.Complete();
            else
                _transaction.Dispose();

            _transaction = null;
        }

        #endregion

        #region Repositories

        private IRepository<Company> _companies;
        public IRepository<Company> Companies
        {
            get
            {
                if (_companies == null)
                    _companies = new EFRepository<Company>(_context);
                return _companies;
            }
        }

        private IRepository<CompanyHistory> _companyHistory;
        public IRepository<CompanyHistory> CompanyHistory
        {
            get
            {
                if (_companyHistory == null)
                    _companyHistory = new EFRepository<CompanyHistory>(_context);
                return _companyHistory;
            }
        }

        private IRepository<Contact> _contacts;
        public IRepository<Contact> Contacts
        {
            get
            {
                if (_contacts == null)
                    _contacts = new EFRepository<Contact>(_context);
                return _contacts;
            }
        }

        private IRepository<ContactHistory> _contactHistory;
        public IRepository<ContactHistory> ContactHistory
        {
            get
            {
                if (_contactHistory == null)
                    _contactHistory = new EFRepository<ContactHistory>(_context);
                return _contactHistory;
            }
        }

        private IRepository<JobSearch> _jobSearches;
        public IRepository<JobSearch> JobSearches
        {
            get
            {
                if (_jobSearches == null)
                    _jobSearches = new EFRepository<JobSearch>(_context);
                return _jobSearches;
            }
        }

        private IRepository<JobSearchHistory> _jobsearchHistory;
        public IRepository<JobSearchHistory> JobSearchHistory
        {
            get
            {
                if (_jobsearchHistory == null)
                    _jobsearchHistory = new EFRepository<JobSearchHistory>(_context);
                return _jobsearchHistory;
            }
        }

        private IRepository<Organization> _organizations;
        public IRepository<Organization> Organizations
        {
            get
            {
                if (_organizations == null)
                    _organizations = new EFRepository<Organization>(_context);
                return _organizations;
            }
        }

        private IRepository<Position> _position;
        public IRepository<Position> Positions
        {
            get
            {
                if (_position == null)
                    _position = new EFRepository<Position>(_context);
                return _position;
            }
        }

        private IRepository<Task> _tasks;
        public IRepository<Task> Tasks
        {
            get
            {
                if (_tasks == null)
                    _tasks = new EFRepository<Task>(_context);
                return _tasks;
            }
        }

        private IRepository<TaskHistory> _taskHistory;
        public IRepository<TaskHistory> TaskHistory
        {
            get
            {
                if (_taskHistory == null)
                    _taskHistory = new EFRepository<TaskHistory>(_context);
                return _taskHistory;
            }
        }

        private IRepository<UnitTestEntity> _unitTestEntities;
        public IRepository<UnitTestEntity> UnitTestEntities
        {
            get
            {
                if (_unitTestEntities == null)
                    _unitTestEntities = new EFRepository<UnitTestEntity>(_context);
                return _unitTestEntities;
            }
        }

        private IRepository<User> _users;
        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new EFRepository<User>(_context);
                return _users;
            }
        }

        private IRepository<MilestoneConfig> _milestoneConfigs;
        public IRepository<MilestoneConfig> MilestoneConfigs
        {
            get
            {
                if (_milestoneConfigs == null)
                    _milestoneConfigs = new EFRepository<MilestoneConfig>(_context);
                return _milestoneConfigs;
            }
        }

        #endregion
    }
}
