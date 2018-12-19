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
            entry = _context.Entries.Where(e => e.Story.StoryId == id).Where(e => e.IsActive).FirstOrDefault();
            return entry;
        }

        public int GetFragCountOfStory(int id)
        {
            return _context.Stories.Find(id).FragCount;
        }
    }
}
