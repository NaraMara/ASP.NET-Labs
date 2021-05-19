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
    public class HospitalPhonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalPhonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalPhones
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }
            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);
            if (hospital == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Hospital = hospital;
            var phones = await this._context.HospitalPhones
                .Include(w => w.Hospital)
                .Where(x => x.HospitalId == hospitalId).ToListAsync();

            
            return this.View(phones);
        }

        // GET: HospitalPhones/Details/5
        public async Task<IActionResult> Details(Int32? hospitalId,Int32? phoneId)
        {
            if (hospitalId == null|| phoneId==null)
            {
                return NotFound();
            }

            var hospitalPhone = await _context.HospitalPhones
                .Include(h => h.Hospital)
                .SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.PhoneId==phoneId);
            if (hospitalPhone == null)
            {
                return NotFound();
            }

            return View(hospitalPhone);
        }

        // GET: HospitalPhones/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();

            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);
            if (hospital == null)
            {
                return this.NotFound();

            }
            this.ViewBag.Hospital = hospital;
            return this.View(new HospitalPhoneCreateViewModel());
        }

        // POST: HospitalPhones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId,HospitalPhoneCreateViewModel model)
        {
            if(hospitalId==null)
            {
                return this.NotFound();
            }
            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();

            }
            if (ModelState.IsValid)
            {
                var phoneId = this._context.HospitalPhones.Any() ? this._context.Diagnoses.Max(x => x.DiagnosisId) + 1 : 1;

                var hospitalPhone = new HospitalPhone
                {
                    PhoneId = phoneId,
                    HospitalId = hospital.Id,
                    Number=model.Number
                };


                _context.Add(hospitalPhone);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",new { hospitalId=hospital.Id});
            }
            this.ViewBag.Hospital = hospital;
            return View(model);
        }

        // GET: HospitalPhones/Edit/5
        public async Task<IActionResult> Edit(Int32? hospitalId, Int32? phoneId)
        {
            if (hospitalId == null||phoneId==null)
            {
                return NotFound();
            }

            var hospitalPhone = await _context.HospitalPhones.SingleOrDefaultAsync(m => m.HospitalId == hospitalId&& phoneId==m.PhoneId);
            if (hospitalPhone == null)
            {
                return NotFound();
            }
            this.ViewBag.HospitalPhone = hospitalPhone;
            return View(hospitalPhone);
        }

        // POST: HospitalPhones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? hospitalId, Int32? phoneId, HospitalPhoneCreateViewModel model)
        {
            if (hospitalId == null || phoneId == null)
            {
                return NotFound();
            }
            var hospitalPhone = await this._context.HospitalPhones
                .SingleOrDefaultAsync(x=>x.HospitalId==hospitalId&&x.PhoneId==phoneId);
            if(hospitalPhone==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                hospitalPhone.Number = model.Number;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index",new { hospitalId=hospitalId});
            }
            return View(hospitalPhone);
        }

        // GET: HospitalPhones/Delete/5
        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? phoneId)
        {
            if (hospitalId == null || phoneId == null)
            {
                return NotFound();
            }
            var hospitalPhone = await this._context.HospitalPhones
                .Include(x=>x.Hospital)
                .SingleOrDefaultAsync(x => x.HospitalId == hospitalId && x.PhoneId == phoneId);
            if (hospitalPhone == null)
            {
                return NotFound();
            }

            return View(hospitalPhone);
        }

        // POST: HospitalPhones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? hospitalId, Int32? phoneId)
        {
            var hospitalPhone = await _context.HospitalPhones.SingleOrDefaultAsync(x => x.HospitalId == hospitalId && x.PhoneId == phoneId);
            _context.HospitalPhones.Remove(hospitalPhone);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { hospitalId = hospitalId });
        }

        
    }
}
