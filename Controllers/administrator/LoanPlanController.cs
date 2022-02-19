using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers.administrator
{
    public class LoanPlanController : Controller
    {
        private readonly LoanManagementContext _context;

        public LoanPlanController(LoanManagementContext context)
        {
            _context = context;
            ViewBag.LoanPlanActive = "active";
        }

        // GET: LoanPlan
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoanPlans.ToListAsync());
        }

        // GET: LoanPlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanPlan = await _context.LoanPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanPlan == null)
            {
                return NotFound();
            }

            return View(loanPlan);
        }

        // GET: LoanPlan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoanPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Interest,MonthlyOverDuePenalty")] LoanPlan loanPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loanPlan);
        }

        // GET: LoanPlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanPlan = await _context.LoanPlans.FindAsync(id);
            if (loanPlan == null)
            {
                return NotFound();
            }
            return View(loanPlan);
        }

        // POST: LoanPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Month,Interest,MonthlyOverDuePenalty")] LoanPlan loanPlan)
        {
            if (id != loanPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanPlanExists(loanPlan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loanPlan);
        }

        // GET: LoanPlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanPlan = await _context.LoanPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loanPlan == null)
            {
                return NotFound();
            }

            return View(loanPlan);
        }

        // POST: LoanPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanPlan = await _context.LoanPlans.FindAsync(id);
            _context.LoanPlans.Remove(loanPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanPlanExists(int id)
        {
            return _context.LoanPlans.Any(e => e.Id == id);
        }
    }
}
