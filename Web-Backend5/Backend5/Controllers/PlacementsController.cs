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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital== null)
            {
                return this.NotFound();
            }
            this.ViewBag.Hospital= hospital;
            var placements= await this._context.Placements
                .Include(w => w.Patient)
                .Include(w => w.Ward)
                .Include(h=>h.Hospital)
                .Where(x => x.HospitalId == hospitalId)
                .ToListAsync();
            return View(placements);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .Include(h=>h.Hospital)
                .SingleOrDefaultAsync(m => m.Id == placementId);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult>Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }
            var hospital= await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);
            if (hospital == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Hospital = hospital;
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name");
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name");
            return View();
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, PlacementCreateEditViewModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }
            var hospital= await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);
            if (hospital == null)
            {
                return this.NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var placement = new Placement { 
                    Bed=model.Bed,
                    HospitalId=hospitalId,
                    PatientId=model.PatientId,
                    WardId=model.WardId

                };
                if (_context.Placements.Any(x => x.PatientId == model.PatientId))
                {
                    this.ModelState.AddModelError("error","error mnoga");
                    this.ViewBag.Hospital = hospital;
                    return View(model);
                }
                _context.Add(placement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",new {hospitalId=hospitalId});
            }
            this.ViewBag.Hospital = hospital;
            return View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == placementId);
            if (placement == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return View(placement);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? placementId, PlacementCreateEditViewModel model)
        {
            if (placementId == null )
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id==placementId);
            if (placement == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                placement.Bed = model.Bed;
                placement.PatientId = model.PatientId;
                placement.WardId = model.WardId;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { hospitalId =placement.HospitalId});
            }
            return View(placement);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? placementId)
        {
            if (placementId == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == placementId);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? placementId)
        {
            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == placementId);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { hospitalId = placement.HospitalId });
        }

        private bool PlacementExists(int id)
        {
            return _context.Placements.Any(e => e.Id == id);
        }
    }
}
