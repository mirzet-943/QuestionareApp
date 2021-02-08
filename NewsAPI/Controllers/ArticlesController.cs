using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAPI.Models.Pagging;
using NewsAppData;
using NewsAppData.ViewModels;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionareController : ControllerBase
    {

        private readonly IRepository _repository;
        private readonly ILogger<QuestionareController> _logger;

        public QuestionareController(ILogger<QuestionareController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


        [HttpGet]
        public async Task<PagedList<ArticleVM>> Get([FromQuery] int PageNumber, [FromQuery] bool myArticles = false, [FromQuery] string searchTerm = "")
        {
            var claimId = User.Claims.Where(s => s.Type == ClaimTypes.Name).FirstOrDefault();
            if (searchTerm == null)
                searchTerm = "";
            if (myArticles && claimId == null)
            {
                return null;
            }
            var result = await _repository.GetAllAsync<Article>(new Models.Parameters.PageParameters() { PageNumber = PageNumber }, (s => s.Subject.ToLower().Contains(searchTerm.ToLower()) && (myArticles == true?Convert.ToInt32(claimId.Value) == s.WriterId:true)), s => s.CreatedAt);

            var articleItemsVM = result.Items.Select(s => new ArticleVM { ArticleId = s.ArticleId, CreatedAt = s.CreatedAt, Likes = s.Likes, Subject = s.Subject, Text = s.Text, WriterFullName = s.Writer.FullName, WriterId = s.WriterId }).ToList();
                
            var articleVm = new PagedList<ArticleVM>(articleItemsVM, result.TotalCount, result.CurrentPage, result.PageSize);
            return articleVm;
        }


        [HttpGet("{id}")]
        public async Task<Article> GetById(int id)
        {
            return await _repository.SelectById<Article>(id);
        }

        [HttpPost, Route("create"),Authorize("Writer")]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var claimId = User.Claims.Where(s => s.Type == ClaimTypes.Name).FirstOrDefault();
            article.CreatedAt = DateTime.Now;
            article.WriterId = Convert.ToInt32(claimId.Value);

            await _repository.CreateAsync<Article>(article);
            return Ok();
        }

        [HttpPut, Route("edit/{articleId}"), Authorize(Policy = "Writer")]
        public async Task<IActionResult> UpdateArticle([FromRoute] int articleId, [FromBody] Article modifiedArticle)
        {
            var claimId = User.Claims.Where(s => s.Type == ClaimTypes.Name).FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest("Invalid object");
            var article = await _repository.SelectById<Article>(articleId);

            if (article == null)
            {
                return BadRequest("Article does not exist");
            }

            if (claimId == null || Convert.ToInt32(claimId.Value) != article.WriterId)
                return Unauthorized();

            article.Subject = modifiedArticle.Subject;
            article.Text = modifiedArticle.Text;
            await _repository.UpdateAsync<Article>(article);
            return Ok();
        }


        [HttpGet, Route("like/{articleId}")]
        public async Task<IActionResult> LikeArticle([FromRoute] int articleId)
        {

            var article = await _repository.SelectById<Article>(articleId);
            if (article == null)
            {
                return BadRequest("Article does not exist");
            }
            article.Likes++;
            await _repository.UpdateAsync<Article>(article);
            return Ok();
        }


        [HttpDelete, Route("{articleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticles([FromRoute] int articleId)
        {
            var claimId = User.Claims.Where(s => s.Type == ClaimTypes.Name).FirstOrDefault();

            var article = await _repository.SelectById<Article>(articleId);
            if (article == null)
            {
                return BadRequest("Article does not exist");
            }
            if (claimId == null || Convert.ToInt32(claimId.Value) != article.WriterId)
                return Unauthorized();
            await _repository.DeleteAsync<Article>(article);
            return Ok();
        }


    }
}
