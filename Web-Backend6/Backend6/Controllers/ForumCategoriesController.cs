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
    public class ForumCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;
        public ForumCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this._context = context;
        }


        // GET: ForumCategories
        public async Task<IActionResult> Index()
        {
            var items = await this._context.ForumCategories
               .Include(c => c.Forums)
               .ThenInclude(t => t.ForumTopics)
               .ToListAsync();
            return View(items);
        }

        // GET: ForumCategories/Create
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ForumCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Create([Bind("Id,Name")] ForumCategory forumCategory)
        {
            if (ModelState.IsValid)
            {
                forumCategory.Id = Guid.NewGuid();
                _context.Add(forumCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forumCategory);
        }

        // GET: ForumCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }
            var model = new ForumCategoriesEditViewModel
            {
                Name = forumCategory.Name
            };
            return View(model);
        }

        // POST: ForumCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumCategoriesEditViewModel model)
        {
            if (id ==null)
            {
                return NotFound();
            }
            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                forumCategory.Name = model.Name;   
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ForumCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumCategories.Remove(forumCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumCategoryExists(Guid id)
        {
            return _context.ForumCategories.Any(e => e.Id == id);
        }
    }
}
