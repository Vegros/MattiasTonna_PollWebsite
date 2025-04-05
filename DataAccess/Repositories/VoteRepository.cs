using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public class VoteRepository
    {
        private readonly PollDbContext _dbContext;

        public VoteRepository(PollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool HasUserVoted(int pollId, string userId)
        {
            return _dbContext.Votes.Any(v => v.PollId == pollId && v.UserId == userId);
        }

        public void AddVote(Vote vote)
        {
            _dbContext.Votes.Add(vote);
            _dbContext.SaveChanges();
        }

        public int GetTotalVotes(int pollId)
        {
            return _dbContext.Votes.Count(v => v.PollId == pollId);
        }
    }
}