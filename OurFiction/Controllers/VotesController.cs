using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OurFiction.Data;
using OurFiction.Models;

namespace OurFiction.Controllers
{
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;


        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class VotesIndexViewModel
        {
            public Story Story { get; set; }
            public Entry Entry { get; set; }
            public StoryFragment Fragment { get; set; }
            public Vote Vote { get; set; }
        }

        // GET: Votes
        public async Task<IActionResult> Index()
        {
            //get active entries
            var entries = await _context.Entries.Include(e => e.Story).Where(e => e.IsActive).ToListAsync();
            //get active stories
            List<VotesIndexViewModel> models = new List<VotesIndexViewModel>();
            foreach(var entry in entries)
            {
                models.Add(new VotesIndexViewModel() {
                    Story = entry.Story,
                    Entry = entry
                });
            }
            return View(models);
        }

        public async Task<IActionResult> VoteForId(int fId, int eId)
        {
            var vote = _context.Votes.Include(v => v.Entry)
                .Include(v => v.Fragment)
                .Where(v => v.Entry.EntryId == eId)
                .Where(v => v.Fragment.FragmentId == fId).FirstOrDefault();
            vote.VotePoints += 1;
            await _context.SaveChangesAsync();
            var entry = _context.Entries.Include(e => e.Story)
                .Where(e => e.EntryId == eId).FirstOrDefault();
            var storyId = entry.Story.StoryId;
            return RedirectToAction(nameof(ShowEntries),new { id = storyId });
        }

        public async Task<IActionResult> ShowEntries(int id)
        {
            ViewData["Counter"] = 1;
            ViewData["StoryId"] = id;
            var entry = _context.Entries.Include(e => e.Story)
                .Where(e => e.IsActive)
                .Where(e => e.Story.StoryId == id).FirstOrDefault();
            ViewData["StoryTitle"] = entry.Story.Title;
            var fragments = _context.Fragments.Include(e => e.Entry)
                .Where(e => e.Entry.Story.StoryId == entry.Story.StoryId).ToList();
            List<VotesIndexViewModel> models = new List<VotesIndexViewModel>();
            
            foreach(var frag in fragments)
            {
                var vote = _context.Votes.Include(v => v.Entry)
                    .Include(v => v.Fragment)
                    .Where(v => v.Entry.EntryId == frag.Entry.EntryId)
                    .Where(v => v.Fragment.FragmentId == frag.FragmentId).FirstOrDefault();
                models.Add(new VotesIndexViewModel
                {
                    Story = entry.Story,
                    Entry = frag.Entry,
                    Fragment = frag,
                    Vote = vote
                });
            }

            return View(models);
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteId,VotePoints")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vote);
        }

        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoteId,VotePoints")] Vote vote)
        {
            if (id != vote.VoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.VoteId))
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
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.VoteId == id);
        }
    }
}
