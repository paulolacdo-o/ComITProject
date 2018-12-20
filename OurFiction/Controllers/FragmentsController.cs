using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            List<StoryFragment> fragments = new List<StoryFragment>();
            ViewData["StoryList"] = stories = _repository.GetActiveStories();
            foreach(Story story in stories)
            {
                entry = _repository.GetActiveEntryOfStory(story.StoryId);
                if (entry == null)
                {
                    displayList.Add(new FragmentViewModel {
                        StoryTitle = story.Title,
                        IsActive = false,
                        StoryId = story.StoryId
                    });
                }
                else
                {
                    fragments.AddRange(_context.Fragments.Include(f => f.Owner)
                        .Include(f => f.Entry)
                        .Where(f => f.Entry.EntryId == entry.EntryId).ToList());
                    displayList.Add(new FragmentViewModel
                    {
                        StoryTitle = story.Title,
                        IsActive = true,
                        EntryId = entry.EntryId,
                        StoryId = story.StoryId
                    });
                }
            }
            string idStringLoggedUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["FragList"] = fragments.Where(f => f.Owner.Id == idStringLoggedUser).ToList();
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
            public StoryFragment fragment { get; set; }

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
        public async Task<IActionResult> Create([Bind("FragmentId,Content")] StoryFragment storyFragment, int EntryId)
        {
            
            if (ModelState.IsValid)
            {
                string idLoggedUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Owner = _context.Users.Where(u => u.Id == idLoggedUser).FirstOrDefault();
                storyFragment.Owner = Owner;
                
                _context.Add(storyFragment);
                Entry entry = new Entry();
                entry = await _context.Entries.FindAsync(EntryId);
                storyFragment.Entry = entry;
                storyFragment.Story = null;
                
                await _context.SaveChangesAsync();

                Vote vote = new Vote()
                {
                    Entry = entry,
                    Fragment = storyFragment
                };

                _context.Add(vote);

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
            var votes = await _context.Votes.Include(v => v.Fragment)
                .Where(v => v.Fragment.FragmentId == storyFragment.FragmentId).ToListAsync();
            foreach(var vote in votes)
            {
                _context.Votes.Remove(vote);
                await _context.SaveChangesAsync();
            }
            _context.Fragments.Remove(storyFragment);
            await _context.SaveChangesAsync();

            if (storyFragment.Story != null)
            {
                int storyId = storyFragment.Story.StoryId;
                bool IsStoryFragCountUpdated = _repository.UpdateStoryNumberOfFragments(storyId);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StoryFragmentExists(int id)
        {
            return _context.Fragments.Any(e => e.FragmentId == id);
        }
    }
}
