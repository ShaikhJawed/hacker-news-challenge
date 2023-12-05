using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Tests.MockData
{
    public class StoryMockData
    {
        public static IEnumerable<Story> GetStories(int count)
        {
            var fakeStories = new List<Story>();

            for (int i = 1; i <= count; i++)
            {
                fakeStories.Add(new Story
                {
                    title = $"Fake Title {i}",
                    url = $"http://fakeurl{i}.com"
                });
            }

            return fakeStories;

        }
        public static IEnumerable<Story> GetEmptyStories()
        {
            return new List<Story>();
        }
    }
}
