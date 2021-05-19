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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analyses
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            var analyses = await this._context.Analyses
                .Include(w => w.Patient)
                .Include(i=> i.Lab)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(analyses);
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(Int32? analysisId)
        {
            if (analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .SingleOrDefaultAsync(m => m.PatientId == analysisId);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // GET: Analyses/Create
        public async Task<IActionResult>Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }
            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x=>x.Id== patientId);
            if (patient == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Patient = patient;
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
            return View();
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysesCreateEditViewModel model)
        {

            if (patientId == null)
            {
                return this.NotFound();
            }
            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);
            if (patient == null)
            {
                return this.NotFound();
            }


            if (ModelState.IsValid)
            {
                var analysisId = this._context.Analyses.Any() ? this._context.Analyses.Max(x => x.AnalysisId) + 1 : 1;
                var analysis = new Analysis
                {
                    PatientId=patient.Id,
                    AnalysisId = analysisId,
                    LabId = model.LabId,
                    Date = model.Date,
                    Status = model.Status,
                    Type = model.Type
                };
                _context.Add(analysis);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",new {patientId=patientId});
            }

            this.ViewBag.Patient = patient;
            return View(model);
        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(Int32? patientId, Int32? analysisId)
        {
            if (patientId == null|| analysisId==null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses.SingleOrDefaultAsync(m => m.PatientId == patientId && m.AnalysisId==analysisId);
            if (analysis == null)
            {
                return NotFound();
            }
            ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name", analysis.LabId);
            this.ViewBag.Analysis = analysis;
            return View(analysis);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? patientId, Int32? analysisId, AnalysesCreateEditViewModel model)
        {
            if (patientId == null || analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses.SingleOrDefaultAsync(m => m.PatientId == patientId && m.AnalysisId == analysisId);
            if (analysis == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                analysis.Status = model.Status;
                analysis.Type = model.Type;
                analysis.Date = model.Date;
                analysis.LabId = model.LabId;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { patientId = patientId });
            }
            return View(model);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(Int32? patientId, Int32? analysisId)
        {
            if (patientId == null || analysisId == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(x=>x.Patient)
                .Include(x=>x.Lab)
                .SingleOrDefaultAsync(m => m.PatientId == patientId && m.AnalysisId == analysisId);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? patientId, Int32? analysisId)
        {
            var analysis = await _context.Analyses.SingleOrDefaultAsync(m => m.PatientId == patientId && m.AnalysisId == analysisId);
            _context.Analyses.Remove(analysis);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { patientId = patientId });
        }

        
    }
}
