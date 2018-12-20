﻿using System;
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
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository _repository;

        public EntriesController(ApplicationDbContext context)
        {
            _context = context;
            _repository = new Repository(_context);
        }

        // GET: Entries
        public async Task<IActionResult> Index(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            ViewData["IsAnEntryActive"] = _repository.IsAnEntryActive(id);
            ViewData["StoryId"] = id;
            var entriesOfStory = _context.Entries.Where(e => e.Story.StoryId == id);
            return View(await entriesOfStory.ToListAsync());
        }

        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries.Include(m => m.Story)
                .FirstOrDefaultAsync(m => m.EntryId == id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        // GET: Entries/Create
        public async Task<IActionResult> Create(int? StoryId)
        {
            Story story = new Story();
            story = await _context.Stories.FindAsync(StoryId);
            Entry entry = new Entry()
            {
                IsActive = true,
                Story = story
            };
            var entriesOfStory = _context.Entries.Where(e => e.Story.StoryId == StoryId);
            _context.Add(entry);
            _context.SaveChanges();
            ViewData["StoryId"] = StoryId;
            return RedirectToAction(nameof(Index), new { id = StoryId });
        }

        public async Task<IActionResult> Deactivate(int? StoryId)
        {
            if(StoryId == null)
            {
                return NotFound();
            }
            Entry entry = new Entry();
            entry = _repository.GetActiveEntryOfStory(StoryId);
            if(entry==null)
            {
                return NotFound();
            }
            entry.IsActive = !entry.IsActive;

            Story story = new Story();
            story = _context.Stories.Find(StoryId);

            var fragments = _context.Fragments.Include(f => f.Entry)
                .Where(f => f.Entry.EntryId == entry.EntryId).ToList();
            var votes = _context.Votes.Include(v => v.Fragment)
                .Include(v => v.Entry).Where(v => v.Entry.EntryId == entry.EntryId).ToList();
            int mostPoints;
            if (votes.Any())
            {
                mostPoints = votes.Max(v => v.VotePoints);
            }
            else
                return RedirectToAction(nameof(Index), new { id = StoryId });

            var winningVotes = votes.Where(v => v.VotePoints == mostPoints);

            List<StoryFragment> winner = new List<StoryFragment>();
            foreach(var vote in winningVotes)
            {
                winner.Add(vote.Fragment);
            }

            if(winner.Count() != 1)
                return RedirectToAction(nameof(Index), new { id = StoryId });

            var frag = fragments.Where(f => f.FragmentId == winner.FirstOrDefault().FragmentId).FirstOrDefault();
            frag.Story = story;
            frag.Entry = null;
            story.FragCount++;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = StoryId });
        }

        // POST: Entries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("EntryId,IsActive")] Entry entry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(entry);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(entry);
        //}

        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return View(entry);
        }

        // POST: Entries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntryId,IsActive")] Entry entry)
        {
            if (id != entry.EntryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.EntryId))
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
            return View(entry);
        }

        // GET: Entries/Delete/5
        public async Task<IActionResult> Delete(int? id, int StoryId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries.Include(m => m.Story)
                .FirstOrDefaultAsync(m => m.EntryId == id);
            if (entry == null)
            {
                return NotFound();
            }
            ViewData["StoryId"] = StoryId;
            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int StoryId)
        {
            var fragment = await _context.Fragments.Where(f => f.Entry.EntryId == id).ToListAsync();
            if(fragment.Any())
            {
                foreach (var frag in fragment)
                {
                    var votes = await _context.Votes.Where(v => v.Fragment.FragmentId == frag.FragmentId).ToListAsync();
                    foreach(var vote in votes)
                    {
                        _context.Votes.Remove(vote);
                        await _context.SaveChangesAsync();
                    }
                    _context.Fragments.Remove(frag);
                    await _context.SaveChangesAsync();
                }
                bool IsStoryFragCountUpdated = _repository.UpdateStoryNumberOfFragments(StoryId);
            }
            var entryVote = await _context.Votes.Include(v => v.Entry)
                .Where(v => v.Entry.EntryId == id).ToListAsync();
            if(entryVote.Any())
            {
                foreach(var vote in entryVote)
                {
                    vote.Entry = null;
                    await _context.SaveChangesAsync();
                }
            }
            var entry = await _context.Entries.FindAsync(id);
            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = StoryId });
        }

        private bool EntryExists(int id)
        {
            return _context.Entries.Any(e => e.EntryId == id);
        }
    }
}
