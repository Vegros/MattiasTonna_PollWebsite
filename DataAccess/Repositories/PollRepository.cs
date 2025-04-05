using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private  PollDbContext _dbContext;  

        public PollRepository(PollDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public IQueryable<Poll> GetPolls()
        {
            return _dbContext.Polls; 
        }
        public void CreatePoll(Poll poll)
        {
            _dbContext.Polls.Add(poll);  
            _dbContext.SaveChanges();
        }
    }
}
