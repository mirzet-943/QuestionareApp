using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAPI.JWTFeatures;
using NewsAPI.JWTFeatures.Entities;
using NewsAPI.Models;
using NewsAPI.Models.Pagging;
using QuestionariesAppData;
using QuestionariesAppData.Models;
using QuestionariesAppData.ViewModels;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly ILogger<CandidatesController> _logger;

        public CandidatesController(ILogger<CandidatesController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


        [HttpGet, Authorize(Policy = "Admin")]
        public async Task<PagedList<CandidateViewModel>> Get([FromQuery] int PageNumber, string searchTerm)
        {
            if (searchTerm == null)
                searchTerm = "";
            var result = await _repository.GetAllAsync<Candidate>(new Models.Parameters.PageParameters() { PageNumber = PageNumber }, s => s.Email.Contains(searchTerm) || s.FullName.Contains(searchTerm), s => s.CreatedAt);


            var items = result.Items.ToList().Select(s => new CandidateViewModel()
            {
                CandidateId = s.CandidateId,
                CreatedAt = s.CreatedAt,
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                FullName = s.FullName,
                Gender = s.Gender
            }).ToList();

            foreach (var item in items)
            {
                var lastQuestionare = (await _repository.Find<Questionare>(s => s.CandidateId == item.CandidateId)).LastOrDefault();
                if (lastQuestionare != null)
                {
                    item.TotalScore = (CalculateQuestionareResults((await _repository.GetAllAsync<QuestionResult>(new Models.Parameters.PageParameters() { PageNumber = 0 }, f => f.QuestionareId == lastQuestionare.QuestionareId, f => f.QuestionResultId)).Items));

                }
            }

            var resultVM = new PagedList<CandidateViewModel>(items.ToList(), result.TotalCount, result.CurrentPage, result.PageSize);

            return resultVM;
        }

        private int CalculateQuestionareResults(List<QuestionResult> questionareResults)
        {
            int points = 0;
            foreach (var item in questionareResults)
            {
                var answerPoints = item.Question.GradePattern.Split(',')[(int)item.Answer];
                points += Convert.ToInt32(answerPoints);
            }
            return points;
        }

        [HttpPost, Route("register"), Authorize(Policy = "Admin")]
        public async Task<IActionResult> Register(Candidate candidate)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");

            if ((await _repository.Find<Candidate>(s => s.Equals(candidate))).FirstOrDefault() == null)
            {
                candidate.CreatedAt = DateTime.Now;
                await _repository.CreateAsync<Candidate>(candidate);
                return Ok();
            }
            return StatusCode(403, "User already exists");
        }


        [HttpPut, Route("edit/{candidateId}"), Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateUser([FromRoute] int candidateId, [FromBody] Candidate modifiedCandidate)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var candidate = await _repository.SelectById<Candidate>(candidateId);

            if (candidate == null)
            {
                return BadRequest("User does not exist");
            }
            candidate.FullName = modifiedCandidate.FullName;
            candidate.Email = modifiedCandidate.Email;
            candidate.DateOfBirth = modifiedCandidate.DateOfBirth;
            candidate.Gender = modifiedCandidate.Gender;
            await _repository.UpdateAsync<Candidate>(candidate);
            return Ok();
        }


        [HttpDelete, Route("delete/{candidateId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] int candidateId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var candidate = await _repository.SelectById<Candidate>(candidateId);
            if (candidate == null)
            {
                return BadRequest("Candidate does not exist");
            }
            await _repository.DeleteAsync<Candidate>(candidate);
            return Ok();
        }
    }
}
