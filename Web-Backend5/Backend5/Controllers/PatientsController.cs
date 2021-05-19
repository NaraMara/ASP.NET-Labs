﻿using System;
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
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient {
                    Name=model.Name,
                    Address=model.Address,
                    Birthday=model.Birthday,
                    Gender=model.Gender
                };
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            var model = new PatientCreateEditViewModel
            {
                Name = patient.Name,
                Address = patient.Address,
                Birthday = patient.Birthday,
                Gender = patient.Gender
            };
            return View(model);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id,PatientCreateEditViewModel model)
        {
            if (id ==null)
            {
                return NotFound();
            }
            var patient = await this._context.Patients.
                SingleOrDefaultAsync(x=>x.Id==id);
            if (patient == null)
            {
                return NotFound();

            }
            if (ModelState.IsValid)
            {
                patient.Name = model.Name;
                patient.Address = model.Address;
                patient.Birthday = model.Birthday;
                patient.Gender = model.Gender;

                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}