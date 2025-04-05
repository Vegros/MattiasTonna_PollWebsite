using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {


        private readonly string _filePath = "polls.json";

        public IQueryable<Poll> GetPolls()
        {
            var json = File.ReadAllText(_filePath);
            var polls = JsonSerializer.Deserialize<List<Poll>>(json) ?? new List<Poll>();
            return polls.AsQueryable();
        }

        public void CreatePoll(Poll poll)
        {
            var polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            var json = JsonSerializer.Serialize(polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }




    }
}
