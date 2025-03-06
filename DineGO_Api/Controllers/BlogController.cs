using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DineGO_Api.Model;
using DineGO_Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DineGO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
        private readonly IBlogRepositoy _blogRepositoy;
        public BlogController(IBlogRepositoy blogRepositoy )
        {
            _blogRepositoy = blogRepositoy;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blogRepositoy.GetBlogs());
        }
        [HttpGet("id")]
        public IActionResult GetOne(int ID)
        {
            return Ok(_blogRepositoy.FindBlogById(ID));
        }

        [HttpPost]
        public IActionResult Addblog(Blog p)
        {
            _blogRepositoy.SaveBlog(p);
            return Ok(_blogRepositoy.GetBlogs());
        }
        [HttpPut]
        public IActionResult Updateblog(Blog p)
        {
            _blogRepositoy.UpdateBlog(p);
            return Ok(_blogRepositoy.GetBlogs());
        }
        [HttpDelete]
        public IActionResult Deleteblog(int Id)
        {
            _blogRepositoy.DeleteBlog(Id);
            return Ok(_blogRepositoy.GetBlogs());
        }
    }
}