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
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;

namespace Backend6.Controllers
{
    public class ForumMessageAttachmentsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;
        public ForumMessageAttachmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: ForumMessageAttachments/Create
        public async Task<IActionResult> Create(Guid id)
        {
            if(id==null)
            {
                return this.NotFound();
            }
            var message = await this._context.ForumMessages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (message== null)
            {
                return this.NotFound();
            }
            this.ViewBag.Message = message;
            return View(new ForumMessageAttachmentViewModel());
        }

        // POST: ForumMessageAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, ForumMessageAttachmentViewModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }
            var message = await this._context.ForumMessages
                .Include(x=>x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return this.NotFound();
            }
            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Trim('"'));
            var fileExt = Path.GetExtension(fileName);
            if (!AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.File), "This file type is prohibited");
            }
            if (ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.HttpContext.User);

                var attachment = new ForumMessageAttachment 
                {
                    MessageId=message.Id,
                    Created=DateTime.UtcNow,
                    CreatorId=user.Id,
                    FileName=fileName
                    
                };
                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", attachment.Id.ToString("N") + fileExt);
                attachment.FilePath = $"/attachments/{attachment.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.File.CopyToAsync(fileStream);
                }
                this._context.Add(attachment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","ForumTopics", new { id=message.TopicId});
            }
            return View(model);
        }

      

        

        // GET: ForumMessageAttachments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessageAttachment = await _context.forumMessageAttachments
                .Include(f => f.Creator)
                .Include(f => f.Message).ThenInclude(x=>x.Topic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessageAttachment == null)
            {
                return NotFound();
            }
            this.ViewBag.Message = forumMessageAttachment.Message;
            return View(forumMessageAttachment);
        }

        // POST: ForumMessageAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return this.NotFound();
            }
            var forumMessageAttachment = await _context.forumMessageAttachments
                .Include(x=>x.Message)
                .SingleOrDefaultAsync(m => m.Id == id);
           
            if (forumMessageAttachment == null)
            {
                return this.NotFound();
            }
            var message = forumMessageAttachment.Message;
            _context.forumMessageAttachments.Remove(forumMessageAttachment);
            var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", forumMessageAttachment.Id.ToString("N") + Path.GetExtension(forumMessageAttachment.FilePath));
            System.IO.File.Delete(attachmentPath);
            await _context.SaveChangesAsync();
            this.ViewBag.Message = message;
            return RedirectToAction("Index", "ForumTopics", new { id = message.TopicId });
        }

       
    }
}
