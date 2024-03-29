﻿using BidProjectsManager.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BidProjectsManager.DataLayer.Common
{
    public interface IUnitOfWork
    {
        ICapexRepository CapexRepository { get; }
        ICountryRepository CountryRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        IEbitRepository EbitRepository { get; }
        IOpexRepository OpexRepository { get; }
        IProjectCommentRepository ProjectCommentRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IUserRepository UserRepository { get; }
        IReportDefinitionRepository ReportDefinitionRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        string GetConnectionString();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProjectRepository ProjectRepository { get; private set; }
        public IEbitRepository EbitRepository { get; private set; }
        public IOpexRepository OpexRepository { get; private set; }
        public ICapexRepository CapexRepository { get; private set; }
        public ICountryRepository CountryRepository { get; private set; }
        public ICurrencyRepository CurrencyRepository { get; private set; }
        public IProjectCommentRepository ProjectCommentRepository { get; private set; }
        public IReportDefinitionRepository ReportDefinitionRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProjectRepository = new ProjectRepository(_context);
            EbitRepository = new EbitRepository(_context);
            OpexRepository = new OpexRepository(_context);
            CapexRepository = new CapexRepository(_context);
            CountryRepository = new CountryRepository(_context);
            CurrencyRepository = new CurrencyRepository(_context);
            ProjectCommentRepository = new ProjectCommentRepository(_context);
            ReportDefinitionRepository= new ReportDefinitionRepository(_context);
            UserRepository= new UserRepository(_context);

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
             _context.SaveChanges();
        }


        public string GetConnectionString()
        {
            return _context.Database.GetConnectionString() ?? string.Empty;
        }
    }
}
