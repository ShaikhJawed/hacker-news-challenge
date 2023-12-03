using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public interface INewsService
    {
        Task<Story> GetStory(int storyId);
        Task<int[]> GetTopStories();
    }
}
