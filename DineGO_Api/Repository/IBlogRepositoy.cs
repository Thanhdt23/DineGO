using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DineGO_Api.Model;

namespace DineGO_Api.Repository
{
    public interface IBlogRepositoy
    {
        List<Blog> GetBlogs();
        Blog FindBlogById(int ID);
        void SaveBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(int blog);
    }
}