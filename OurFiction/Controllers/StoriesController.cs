using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OurFiction.Data;
using OurFiction.Models;

namespace OurFiction.Controllers
{
    [Authorize(Policy ="Admin")]
    public class StoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository _repository;

        public StoriesController(ApplicationDbContext context)
        {
            _context = context;
            _repository = new Repository(_context);
        }

        // GET: Stories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stories.Include(s => s.Owner).ToListAsync());
        }

        // GET: Stories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .FirstOrDefaultAsync(m => m.StoryId == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // GET: Stories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoryId,Title,Synopsis,FragCount,InitialContent,IsActive")] Story story)
        {
            string idLoggedUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Owner = _context.Users.Where(u => u.Id == idLoggedUser).FirstOrDefault();
            story.Owner = Owner;
            if (ModelState.IsValid)
            {
                _context.Add(story);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(story);
        }

        // GET: Stories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoryId,Title,Synopsis,FragCount,InitialContent,IsActive")] Story story)
        {
            if (id != story.StoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(story);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryExists(story.StoryId))
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
            return View(story);
        }

        public async Task<IActionResult> ActivateDeactivate(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories.FindAsync(id);
            if(story == null)
            {
                return NotFound();
            }
            story.IsActive = !story.IsActive;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Stories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .FirstOrDefaultAsync(m => m.StoryId == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            var entries = await _context.Entries.Where(e => e.Story.StoryId == id).ToListAsync();
            if(entries.Any())
            {
                foreach(var entry in entries)
                {

                    var votes = await _context.Votes.Where(v => v.Entry.EntryId == entry.EntryId).ToListAsync();
                    if(votes.Any())
                    {
                        foreach(var vote in votes)
                        {
                            _context.Votes.Remove(vote);
                            await _context.SaveChangesAsync();
                        }
                    }
                    var fragments = await _context.Fragments.Where(v => v.Entry.EntryId == entry.EntryId).ToListAsync();
                    if (fragments.Any())
                    {
                        foreach (var fragment in fragments)
                        {
                            _context.Fragments.Remove(fragment);
                            await _context.SaveChangesAsync();
                        }
                        bool IsStoryFragCountUpdated = _repository.UpdateStoryNumberOfFragments(id);
                    }
                    
                    _context.Entries.Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            var frag = await _context.Fragments.Include(f => f.Story).Where(f => f.Story.StoryId == id).ToListAsync();
            if (frag.Any())
            {
                foreach (var fragment in frag)
                {
                    var votes = await _context.Votes.Include(v => v.Fragment).Where(v => v.Fragment.FragmentId == fragment.FragmentId).ToListAsync();
                    foreach(var vote in votes)
                    {
                        _context.Votes.Remove(vote);
                        await _context.SaveChangesAsync();
                    }
                    _context.Fragments.Remove(fragment);
                    await _context.SaveChangesAsync();
                }
            }
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StoryExists(int id)
        {
            return _context.Stories.Any(e => e.StoryId == id);
        }
    }
}
