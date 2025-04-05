using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        
        private VoteRepository _voteRepository;
        private  IPollRepository _pollRepository;
        private  UserManager<IdentityUser> _userManager;
        public PollController(IPollRepository pollRepository, VoteRepository voteRepository, UserManager<IdentityUser> userManager)
        {
            this._pollRepository = pollRepository;
            this._voteRepository = voteRepository;
            this._userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var polls = _pollRepository.GetPolls().ToList();

            foreach(var poll in polls)
            {
                if(poll.CreatedByUser == null && !string.IsNullOrEmpty(poll.CreatedByUserId))
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.Id == poll.CreatedByUserId); 
                    
                    if(user != null)
                    {
                        poll.CreatedByUser = user; 
                    }
                }
            }
            return View(polls);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PollCreateViewModel());
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromServices] IPollRepository pollRepo, [FromServices] UserManager<IdentityUser> user, PollCreateViewModel model)
        {
            if (ModelState.IsValid)
            { 

                var newPoll = new Poll
                {
                    Title = model.Title,
                    Option1Text = model.Option1Text,
                    Option2Text = model.Option2Text,
                    Option3Text = model.Option3Text,
                    Option1VotesCount = 0,
                    Option2VotesCount = 0,
                    Option3VotesCount = 0,
                    CreatedByUserId = user.GetUserId(User),
                    DateCreated = DateTime.UtcNow
                };

                _pollRepository.CreatePoll(newPoll);
                TempData["message"] = "Poll Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error creating Poll"; 
                return View(model);
            }


        }



        [HttpGet("/Poll/Results/{id}")]
        public IActionResult Results(int id)
        {
            if (_pollRepository is PollFileRepository)
            {
                return RedirectToAction("Index", "Poll");
            }
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null) return RedirectToAction("Index", "Poll");
            return View(poll);
        }


        [Authorize]
        [HttpGet("/Poll/Vote/{id}")]
        [ServiceFilter(typeof(HasNotVotedFilter))]
        public IActionResult Vote(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);
            if (poll == null)
                return RedirectToAction("Index", "Poll");

            return View(poll);
        }


        [Authorize]
        [HttpPost("/Poll/Vote/{id}")]
        public IActionResult Vote(int pollId, int selectedOption)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_voteRepository.HasUserVoted(pollId, userId))
            {
                TempData["error"] = "You've already voted!";
                return RedirectToAction("Results", new { id = pollId });
            }

            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == pollId);  
            if (poll == null) return NotFound();

            switch (selectedOption)
            {
                case 1: poll.Option1VotesCount++; break;
                case 2: poll.Option2VotesCount++; break;
                case 3: poll.Option3VotesCount++; break;
                default: return BadRequest("Invalid option");
            }


            _voteRepository.AddVote(new Vote
            {
                PollId = pollId,
                UserId = userId,
                SelectedOption = selectedOption,
                DateVoted = DateTime.UtcNow
            });


            TempData["message"] = "Vote submitted!";
            return RedirectToAction("Results", new { id = pollId });
        }
    }
    }
