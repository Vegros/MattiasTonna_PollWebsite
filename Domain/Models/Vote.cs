using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }

        public string UserId { get; set; }
        public int SelectedOption { get; set; }

        public DateTime DateVoted { get; set; } = DateTime.UtcNow;
    }
}
