using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTserviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;
        public ArticlesController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<ArticleDTO> Get()
        {
            var result = _articleBLL.GetArticleWithCategory();
            return result;
        }

        [HttpGet("{id}")]
        public ArticleDTO GetByID(int id)
        {
            var byID = _articleBLL.GetArticleById(id);
            return byID;
        }


        [HttpPost]
        public IActionResult Post(ArticleCreateDTO articleCreate)
        {
            _articleBLL.Insert(articleCreate);
            var result = _articleBLL.GetArticleByCategory(articleCreate.CategoryID);
            return Ok($"Data Category {articleCreate.CategoryID} berhasil diupdate !");
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(ArticleUpdateDTO article)
        {
            var getcateg = _articleBLL.GetArticleById(article.ArticleID);
            if (getcateg == null)
            {
                return NotFound();
            }
            _articleBLL.Update(article);
            return Ok($"Data Category {article.ArticleID} berhasil diupdate !");
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult deleteCategory(int id)
        {
            var getcateg = _articleBLL.GetArticleById(id);
            if (getcateg == null)
            {
                throw new ArgumentException($"Not found {id}");
            }
            _articleBLL.Delete(id);
            return Ok($"Data Category ID: {id} berhasil didelete");
        }

        [HttpGet("GetByName")]
        public IEnumerable<ArticleDTO> GetByName(int categoryID)
        {
            var getbyname = _articleBLL.GetArticleByCategory(categoryID);
            return getbyname;
        }
    }
}
