using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class WardStuffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStuffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStuffs
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }
            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);
            if (ward == null)
            {
                return this.NotFound();
            }

            var items = await this._context.WardStuffs
                .Include(p => p.Ward)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            this.ViewBag.Ward = ward;
            return View(items);
        }

        // GET: WardStuffs/Details/5
        public async Task<IActionResult> Details(Int32? wardId, Int32? WardStuffId)
        {
            if (wardId == null || WardStuffId == null)
            {
                return NotFound();
            }

            var wardStuff = await _context.WardStuffs
                .Include(d => d.Ward)
                .SingleOrDefaultAsync(m => m.WardId == wardId&& m.WardStuffId== WardStuffId);
            if (wardStuff == null)
            {
                return NotFound();
            }

            return View(wardStuff);
        }

        // GET: WardStuffs/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }
            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);
            if (ward == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Ward = ward;

            return View();
        }

        // POST: WardStuffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStuffsCreateEditViewModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }
            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);
            if (ward == null)
            {
                return this.NotFound();
            }


            if (ModelState.IsValid)
            {
                var wardStuffId = this._context.WardStuffs.Any() ? this._context.WardStuffs.Max(x => x.WardStuffId) + 1 : 1;
                var wardStuff = new WardStuff
                {
                    WardId=ward.Id,
                    WardStuffId=wardStuffId,
                    Name=model.Name,
                    Position=model.Position
                };

                _context.Add(wardStuff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = ward.Id });
            }
            this.ViewBag.Ward= ward;
            return this.View(model);
        }

        // GET: WardStuffs/Edit/5
        public async Task<IActionResult> Edit(Int32? wardId, Int32? wardStuffId)
        {
            if (wardId == null || wardStuffId== null)
            {
                return NotFound();
            }

            var wardStuff = await _context.WardStuffs.SingleOrDefaultAsync(m => m.WardStuffId == wardStuffId && m.WardId == wardId);
            if (wardStuff == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", wardStuff.WardId);
            return View(wardStuff);
        }

        // POST: WardStuffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? wardId, Int32? wardStuffId, WardStuffsCreateEditViewModel model)
        {
            if (wardId == null || wardStuffId== null)
            {
                return NotFound();
            }

            var ward= await _context.WardStuffs.SingleOrDefaultAsync(m => m.WardStuffId == wardStuffId&& m.WardId == wardId);
            if (ward == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ward.Name = model.Name;
                ward.Position = model.Position;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = wardId});
            }

            return View(ward);
        }

        // GET: WardStuffs/Delete/5
        public async Task<IActionResult> Delete(Int32? wardId, Int32? wardStuffId)
        {
            if (wardId == null || wardStuffId == null)
            {
                return NotFound();
            }

            var wardStuff = await _context.WardStuffs.Include(x => x.Ward).SingleOrDefaultAsync(m => m.WardStuffId == wardStuffId&& m.WardId == wardId);
            if (wardStuff == null)
            {
                return NotFound();
            }


            return View(wardStuff);
        }

        // POST: WardStuffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? wardId, Int32? wardStuffId)
        {
            var wardStuff = await _context.WardStuffs.SingleOrDefaultAsync(m => m.WardStuffId == wardStuffId&& m.WardId == wardId);
            _context.WardStuffs.Remove(wardStuff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = wardId });
        }

       
    }
}
