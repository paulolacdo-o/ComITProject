using OurFiction.Data;
using OurFiction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction
{
    public class Repository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Story> GetActiveStories()
        {
            List<Story> activeStories = new List<Story>();
            activeStories = _context.Stories.Where(s => s.IsActive == true).ToList();
            return activeStories;
        }

        public bool IsAnEntryActive(int? id)
        {
            if (id == null)
                return false;
            List<Entry> entries = new List<Entry>();
            entries = _context.Entries.Where(e => e.Story.StoryId == id).ToList();
            foreach (Entry entry in entries)
            {
                if (entry.IsActive)
                    return true;
            }
            return false;
        }

        public Entry GetActiveEntryOfStory(int? id)
        {
            if (id == null)
                return null;
            Entry entry = new Entry();
            entry = _context.Entries
                .Where(e => e.Story.StoryId == id)
                .Where(e => e.IsActive).FirstOrDefault();
            return entry;
        }

        public int GetFragCountOfStory(int id)
        {
            return _context.Stories.Find(id).FragCount;
        }

        public List<StoryFragment> GetListOfFragmentsWithEntry(int? EntryId)
        {
            List<StoryFragment> fragList = new List<StoryFragment>();
            fragList =  _context.Fragments.Where(f => f.Entry.EntryId == EntryId).ToList();
            return fragList;
        }

        public int GetStoryIdWithEntry(int? EntryId)
        {
            var entry = _context.Entries.Find(EntryId);
            if (entry == null)
                return 0;
            return entry.Story.StoryId;
        }

        public bool UpdateStoryNumberOfFragments(int? StoryId)
        {
            bool success = true;
            if(StoryId == null)
            {
                return false;
            }

            var fragments = _context.Fragments.Where(f => f.Story.StoryId == StoryId).ToList();

            var story = _context.Stories.Find(StoryId);
            story.FragCount = fragments.Count();

            _context.SaveChanges();

            return success;
        }
    }
}
