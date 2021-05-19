using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Microsoft.AspNetCore.Identity;
using Backend6.Services;
using Backend6.Models.ViewModels;

namespace Backend6.Controllers
{
    public class ForumMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;
        public ForumMessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this._context = context;
        }

        // GET: ForumMessages/Create
        public async  Task<IActionResult> Create(Guid id)
        {
            if (id==null)
            {
                return this.NotFound();
            }
            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == id);
            if (topic == null)
            {
                return this.NotFound();
            }
            var model = new ForumMessagesEditViewModel();
            this.ViewBag.Topic = topic;
            return View(model);
        }

        // POST: ForumMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,ForumMessagesEditViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }
            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == id);
            if (topic == null)
            {
                return this.NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.HttpContext.User);
                var now = DateTime.UtcNow;
                var message = new ForumMessage()
                {
                    Text = model.Text,
                    CreatorId = user.Id,
                    TopicId=topic.Id,
                    Created = now,
                    Modified = null
                };

                _context.Add(message);
                await _context.SaveChangesAsync();
                this.ViewBag.Topic = topic;
                return RedirectToAction("Index","ForumTopics",new { id=topic.Id});
            }
            return View(model);
        }

        // GET: ForumMessages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages.SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }
            this.ViewBag.Message = forumMessage;
            var model = new ForumMessagesEditViewModel
            {
                Text = forumMessage.Text
            };

            return View(model);
        }

        // POST: ForumMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumMessagesEditViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(x=>x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forumMessage.Text = model.Text;
                forumMessage.Modified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","ForumTopics",new { id=forumMessage.TopicId});
            }
            return View(forumMessage);
        }

        // GET: ForumMessages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }

            return View(forumMessage);
        }

        // POST: ForumMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }
            var topic = forumMessage.Topic;
            _context.ForumMessages.Remove(forumMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","ForumTopics",topic);
        }

        private bool ForumMessageExists(Guid id)
        {
            return _context.ForumMessages.Any(e => e.Id == id);
        }
    }
}
