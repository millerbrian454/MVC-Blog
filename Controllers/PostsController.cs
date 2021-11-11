using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewMVCapp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace NewMVCapp.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostContext _context;

        public PostsController(PostContext context)
        {
            _context = context;
        }

        //GET Post/Display Posts

        [HttpGet]
        public async Task<IActionResult> DisplayPosts()
        {
            var list = await _context.Posts.ToListAsync();
            list.Reverse();
            return View(list);
        }

        // Registered Users Section

        //GET: Post
        [Authorize]
        public IActionResult CreatePost()
        {
            return View();
        }

        // GET: Posts/CreatePosts
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([Bind("Title, Content")] Posts posts)
        {
            posts.Id = posts.GetHashCode();
            posts.TimeStamp = DateTime.Now.ToString();
            posts.CreatedBy = User.Identity.Name;
            _context.Posts.Add(posts);
            await _context.SaveChangesAsync();
            return RedirectToAction("DisplayPosts");
        }

        // GET: Posts/EditPost/{id}
        [Authorize]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.FindAsync(id);
            if (posts.CreatedBy != User.Identity.Name)
            {
                return Content("Not Authorized");
            }
            if (posts == null)
            {
                return NotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [Bind("Id,Title,Content,TimeStamp,CreatedBy")] Posts posts)
        {
            if (id != posts.Id)
            {
                Console.WriteLine("POST ID does not match! Supplied ID:" + id + "Post ID:" + posts.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Update Succeded");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.Id))
                    {
                        Console.WriteLine("Post Not Found!");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("DisplayPosts");
            }
            return View(posts);
        }

        // GET: Posts/DeletePost/{id}
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posts == null)
            {
                return NotFound();
            }

            if(User.Identity.Name != posts.CreatedBy)
            {
                return Content("Not Authorized!");
            }

            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();

            return RedirectToAction("DisplayPosts");
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
