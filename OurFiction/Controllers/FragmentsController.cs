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
    public class FragmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository _repository;

        public FragmentsController(ApplicationDbContext context)
        {
            _context = context;
            _repository = new Repository(_context);
        }

        // GET: Fragments
        public async Task<IActionResult> Index()
        {
            Entry entry = new Entry();
            List<Story> stories = new List<Story>();
            List<FragmentViewModel> displayList = new List<FragmentViewModel>();
            ViewData["StoryList"] = stories = _repository.GetActiveStories();
            foreach(Story story in stories)
            {
                if ((entry = _repository.GetActiveEntryOfStory(story.StoryId)) == null)
                {
                    displayList.Add(new FragmentViewModel {
                        StoryTitle = story.Title,
                        IsActive = false,
                        StoryId = story.StoryId
                    });
                }
                else
                {
                    displayList.Add(new FragmentViewModel
                    {
                        StoryTitle = story.Title,
                        IsActive = true,
                        EntryId = entry.EntryId,
                        StoryId = story.StoryId
                    });
                }
            }
            ViewData["DisplayList"] = displayList;

            return View(await _context.Fragments.ToListAsync());
        }

        public class FragmentViewModel
        {
            public string StoryTitle { get; set; }
            public bool IsActive { get; set; }
            public int EntryId { get; set; }
            public int StoryId { get; set; }
            public string Contents { get; set; }

            public static implicit operator List<object>(FragmentViewModel v)
            {
                throw new NotImplementedException();
            }
        }

        // GET: Fragments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyFragment = await _context.Fragments
                .FirstOrDefaultAsync(m => m.FragmentId == id);
            if (storyFragment == null)
            {
                return NotFound();
            }

            return View(storyFragment);
        }

        // GET: Fragments/Create
        public IActionResult Create(int EntryId, string StoryTitle, int StoryId)
        {
            ViewData["EntryId"] = EntryId;
            ViewData["StoryTitle"] = StoryTitle;
            ViewData["StoryId"] = StoryId;
            return View();
        }

        // POST: Fragments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FragmentId,Content")] StoryFragment storyFragment, int EntryId, int StoryId)
        {
            
            if (ModelState.IsValid)
            {
                
                _context.Add(storyFragment);
                Story story = new Story();
                Entry entry = new Entry();
                entry = await _context.Entries.FindAsync(EntryId);
                storyFragment.Entry = entry;
                story = await _context.Stories.FindAsync(StoryId);
                int sequenceNumber = 0;
                if (_context.Fragments.Any())
                {
                    sequenceNumber = _context.Fragments.Where(f => f.Story.StoryId == StoryId)
                        .Where(f => f.Entry.EntryId == EntryId).Select(f => f.StorySequenceNumber).DefaultIfEmpty(0).Max();
                }
                storyFragment.StorySequenceNumber = sequenceNumber+1;
                story.FragCount++;
                storyFragment.Story = story;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storyFragment);
        }

        // GET: Fragments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyFragment = await _context.Fragments.FindAsync(id);
            if (storyFragment == null)
            {
                return NotFound();
            }
            return View(storyFragment);
        }

        // POST: Fragments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FragmentId,StorySequenceNumber,Content")] StoryFragment storyFragment)
        {
            if (id != storyFragment.FragmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storyFragment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryFragmentExists(storyFragment.FragmentId))
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
            return View(storyFragment);
        }

        // GET: Fragments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyFragment = await _context.Fragments
                .FirstOrDefaultAsync(m => m.FragmentId == id);
            if (storyFragment == null)
            {
                return NotFound();
            }

            return View(storyFragment);
        }

        // POST: Fragments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storyFragment = await _context.Fragments.FindAsync(id);
            _context.Fragments.Remove(storyFragment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryFragmentExists(int id)
        {
            return _context.Fragments.Any(e => e.FragmentId == id);
        }
    }
}
