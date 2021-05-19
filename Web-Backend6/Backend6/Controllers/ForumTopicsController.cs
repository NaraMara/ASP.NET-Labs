using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Backend6.Services;
using Microsoft.AspNetCore.Identity;
using Backend6.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Backend6.Controllers
{
    [Authorize]
    public class ForumTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;
        public ForumTopicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this._context = context;
        }

        // GET: ForumTopics
        [AllowAnonymous]
        public async Task<IActionResult> Index(Guid id)
        {
            if (id==null)
            {
                this.NotFound();
            }
            var topic = await this._context.ForumTopics
                .Include(x=>x.ForumMessages).ThenInclude(x=>x.Attachments)
                .Include(x=>x.ForumMessages).ThenInclude(x=>x.Creator)
                .Include(x=>x.Forum)
                .Include(x=>x.Creator)
                .SingleOrDefaultAsync(x =>x.Id==id);

            if (topic== null)
            {
                this.NotFound();
            }
            return View(topic);
        }

      

        // GET: ForumTopics/Create
        public async Task<IActionResult>Create(Guid id)
        {
            if (id == null)
            {
                this.NotFound();
            }
            var forum= await this._context.Forums
                .SingleOrDefaultAsync(x => x.Id == id);

            if (forum== null)
            {
                this.NotFound();
            }
            this.ViewBag.Forum = forum;
            return View(new ForumTopicsEditViewModel());
        }

        // POST: ForumTopics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,ForumTopicsEditViewModel model)
        {
            if (id == null)
            {
                this.NotFound();
            }
            var forum = await this._context.Forums
                .SingleOrDefaultAsync(x => x.Id == id);

            if (forum == null)
            {
                this.NotFound();
            }
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (ModelState.IsValid)
            {
                var topic = new ForumTopic
                {
                    ForumId=forum.Id,
                    Created = DateTime.UtcNow,
                    Name=model.Name,
                    CreatorId=user.Id,
                };
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Forum",new { id=id});
            }
            return View(model);
        }

        // GET: ForumTopics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null  )
            {
                return NotFound();
            }
            var model = new ForumTopicsEditViewModel 
            {
                Name=forumTopic.Name
                
            };
            this.ViewBag.Topic = forumTopic;
            return View(model);
        }

        // POST: ForumTopics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumTopicsEditViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(x=>x.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                forumTopic.Name = model.Name;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Forum",new { id=forumTopic.ForumId});
            }
            return View(forumTopic);
        }

        // GET: ForumTopics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // POST: ForumTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var  topic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }
            var forum = topic.Forum;
            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumTopics.Remove(forumTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Forum",forum);
        }

        private bool ForumTopicExists(Guid id)
        {
            return _context.ForumTopics.Any(e => e.Id == id);
        }
    }
}
