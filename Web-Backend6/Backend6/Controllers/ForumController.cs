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
using Microsoft.AspNetCore.Authorization;
using Backend6.Models.ViewModels;

namespace Backend6.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this._context = context;
        }

        // GET: Forum
        public async Task<IActionResult> Index(Guid Id)
        {
            var items = await this._context.Forums
                .Where(x=> x.Id== Id)
                .Include(c => c.ForumTopics)
                .ThenInclude(c => c.Creator)
                .Include(c => c.ForumTopics)
                .ThenInclude(c => c.ForumMessages)
                .SingleOrDefaultAsync();
            return View(items);
        }
        // GET: Forum/Create
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Create(Guid categoryId)
        {
            if (categoryId == null)
            {
                return this.NotFound();
            }

            var category = await this._context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == categoryId);
            if (category == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Category = category;
            return this.View(new ForumEditViewModel());
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Create(Guid categoryId, ForumEditViewModel model)
        {
            if (categoryId == null)
            {
                return this.NotFound();
            }

            var category = await this._context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == categoryId);
            if (category == null)
            {
                return this.NotFound();
            }
            if (ModelState.IsValid)
            {

                var forum = new Forum()
                {
                    Id = Guid.NewGuid(),
                    ForumCategoryId = categoryId,
                    Description = model.Description,
                    Name = model.Name,
                };
                _context.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ForumCategories");
            }
            return View(model);
        }

        // GET: Forum/Edit/5
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }
            var model  = new ForumEditViewModel
            {
                Name=forum.Name,
                Description=forum.Description
            };
            this.ViewBag.Forum = forum;
            return View(model);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Edit(Guid id,ForumEditViewModel model)
        {
            if (id==null)
            {
                return NotFound();
            }
            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forum.Name = model.Name;
                forum.Description = model.Description;
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index","Forum",new { id=forum.Id});
            }
            return View(forum);
        }

        // GET: Forum/Delete/5
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ForumCategories", new { id = id });
        }

        private bool ForumExists(Guid id)
        {
            return _context.Forums.Any(e => e.Id == id);
        }
    }
}
