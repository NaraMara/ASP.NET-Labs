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
    public class DiagnosesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiagnosesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
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

            var items = await this._context.Diagnoses
                .Include(p => p.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            this.ViewBag.Patient = patient;
            return View(items);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(Int32? patientId, Int32? diagnosisId)
        {
            if (patientId == null || diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Patient)
                .SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId==patientId );
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public async Task<IActionResult>Create(Int32? patientId)
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

            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Int32? patientId, DiagnosesCreateViewModel model )
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
                var diagnosisId = this._context.Diagnoses.Any() ? this._context.Diagnoses.Max(x => x.DiagnosisId) + 1 : 1;
                var diagnosis = new Diagnosis 
                {
                    PatientId=patient.Id,
                    DiagnosisId=diagnosisId,
                    Type=model.Type,
                    Complications=model.Complications,
                    Details=model.Details
                };

                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { patientId = patient.Id });
            }
            this.ViewBag.Patient = patient;
            return this.View(model);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(Int32? patientId,Int32? diagnosisId)
        {
            if (patientId == null|| diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId==patientId);
            if (diagnosis == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", diagnosis.PatientId);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? patientId, Int32? diagnosisId,  DiagnosesCreateViewModel model)
        {
            if (patientId == null || diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                diagnosis.Type = model.Type;
                diagnosis.Details = model.Details;
                diagnosis.Complications = model.Complications;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { patientId = patientId });
            }
            
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(Int32? patientId, Int32? diagnosisId)
        {
            if (patientId == null || diagnosisId == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.Include(x=>x.Patient).SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return NotFound();
            }
            

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? patientId, Int32? diagnosisId)
        {
            var diagnosis = await _context.Diagnoses.SingleOrDefaultAsync(m => m.DiagnosisId == diagnosisId && m.PatientId == patientId);
            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { patientId = patientId });
        }

       
    }
}
