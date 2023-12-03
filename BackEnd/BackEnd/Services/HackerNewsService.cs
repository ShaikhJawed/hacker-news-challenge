using BackEnd.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public class HackerNewsService : INewsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HackerNewsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }
        /// <summary>
        /// Returns a single story by id
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<Story> GetStory(int storyId)
        {
            Story story = null;
            var httpClient = _httpClientFactory.CreateClient("HackerNews");
            var httpResponseMessage = await httpClient.GetAsync($"item/{storyId}.json?print=pretty");
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                story  = await httpResponseMessage.Content.ReadAsAsync<Story>();
            }
            return story;
        }

        /// <summary>
        /// Returns ids of top stories
        /// </summary>
        /// <returns></returns>
        public async Task<int[]> GetTopStories()
        {
            int[] stories = null;
            var httpClient = _httpClientFactory.CreateClient("HackerNews");
            var httpResponseMessage = await httpClient.GetAsync($"topstories.json?print=pretty");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                stories = await httpResponseMessage.Content.ReadAsAsync<int[]>();
            }
            return stories;
        }
    }
}
