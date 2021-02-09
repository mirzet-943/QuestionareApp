using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAPI.EmailFeatures;
using QuestionariesAppData;
using QuestionariesAppData.Models;
using QuestionariesAppData.Models.Contracts.Questionare;
using Newtonsoft.Json.Linq;
using QuestionariesAPI.Properties;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionareController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly ILogger<QuestionareController> _logger;
        private readonly IEmailSender _emailSender;

        public QuestionareController(ILogger<QuestionareController> logger, IRepository repository, IEmailSender emailSender)
        {
            _logger = logger;
            _repository = repository;
            _emailSender = emailSender;
        }

        [HttpPost("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] Guid id, [FromBody] PinSubmit pinCode)
        {
            var questionare = await _repository.SelectById<Questionare>(id);
            if (questionare == null)
                return NotFound();
            if (!ReCaptchaPassed(pinCode.CaptchaChallenge))
            {
                return BadRequest("Invalid captcha");
            }
            if (BCrypt.Net.BCrypt.Verify(pinCode.Pincode, questionare.PinCode))
            {
                questionare.AccessTime = DateTime.Now;
                if (questionare.IsSubmitted)
                {
                    var questionareResults = (await _repository.GetAllAsync<QuestionResult>(new Models.Parameters.PageParameters() { PageNumber = 0 }, s => s.QuestionareId == id, s => s.QuestionResultId)).Items;
                    var qResult = CalculateQuestionareResults(questionareResults);
                    return Ok( new { type = "result", questionare_result = new QuestionareResultsViewModel() { QuestionareId = questionare.QuestionareId, QuestionarePoints = qResult } });
                }
                return Ok(new { type = "questions", questionare_result = questionare, questions = (await _repository.GetAllAsync<Question>(new Models.Parameters.PageParameters { PageNumber = 0 }, s => true, s => s.QuestionId)).Items });
            }
            else
            {
                questionare.WrongPinsEntered++;
                return Unauthorized("Wrong pin entered");
            }
        }

        public static bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LcAKlAaAAAAAP2KVs2gpz3LYyL-j-5IDOie7kZ_&response={gRecaptchaResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" && JSONdata.score > 0.6d)
                return false;

            return true;
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

        [HttpPost("{id}/submit"), AllowAnonymous]
        public async Task<IActionResult> SubmitQuestionare([FromRoute] Guid id, [FromBody] QuestionareSubmitCommand results)
        {
            var questionare = await _repository.SelectById<Questionare>(id);
            if (questionare == null)
                return NotFound();
            if (questionare.IsSubmitted || questionare.WrongPinsEntered > 3)
                return Unauthorized();
            if (BCrypt.Net.BCrypt.Verify(results.QuestionarePinCode, questionare.PinCode))
            {
                questionare.IsSubmitted = true;
                questionare.FinishTime = DateTime.Now;
                await _repository.UpdateAsync(questionare);
                foreach (var item in results.Results)
                {
                    item.QuestionareId = id;
                    await _repository.CreateAsync(item);
                }
                var questionareResults = (await _repository.GetAllAsync<QuestionResult>(new Models.Parameters.PageParameters() { PageNumber = 0 }, s => s.QuestionareId == id, s => s.QuestionResultId)).Items;
                var qResult = CalculateQuestionareResults(questionareResults);
                return Ok(new { type = "result", questionare_result = new QuestionareResultsViewModel() { QuestionareId = questionare.QuestionareId, QuestionarePoints = qResult } });
            }
            else
            {
                questionare.WrongPinsEntered++;
                return Unauthorized("Wrong pin entered");
            }
        }

        private Random _random = new Random();

        [HttpPost, Route("create"), Authorize("Admin")]
        public async Task<IActionResult> CreateQuestionare(Questionare questionare)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var candidate = (await _repository.Find<Candidate>(s => s.CandidateId == questionare.CandidateId)).FirstOrDefault();
            if (candidate == null)
                return BadRequest("Candidate is null");

            var pincode = _random.Next(0, 9999).ToString("D4");
            questionare.PinCode = BCrypt.Net.BCrypt.HashPassword(pincode);
            await _repository.CreateAsync<Questionare>(questionare);
            questionare.User = candidate;
            var contentTxt = Resources.QuestionareTemplate;
            String messageContent = String.Format(contentTxt,questionare.User.FullName, String.Format("{0}://{1}/questionares/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, questionare.QuestionareId), pincode);

            await _emailSender.SendEmailAsync(new Message
            {
                Subject = "Questionare access",
                Content = messageContent,
                To = new MimeKit.MailboxAddress("", questionare.User.Email)
            });
            return Ok(questionare);
        }


    }
}
