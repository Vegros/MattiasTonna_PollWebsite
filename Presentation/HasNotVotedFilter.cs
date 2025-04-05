using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation
{
    public class HasNotVotedFilter : IActionFilter
    {
        private readonly VoteRepository _voteRepository;

        public HasNotVotedFilter(VoteRepository voteRepo)
        {
            _voteRepository = voteRepo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var pollId = (int)context.ActionArguments["id"];
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_voteRepository.HasUserVoted(pollId, userId))
            {
                var controller = (Controller)context.Controller;
                controller.TempData["error"] = "You already voted!";
                context.Result = new RedirectToActionResult("Results", "Poll", new { id = pollId });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
