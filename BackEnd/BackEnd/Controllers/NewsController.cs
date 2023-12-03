using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsService _newsService;
        private IConfiguration _configuration;
        private IUriService _uriService;
        private IMemoryCache _memoryCache;
        private int _maxPageSize;
        private int _minPageNumber;
        private int _topStoriesLimit;
        private int _cacheExpiryMinutes;
        public NewsController(INewsService newsService, IConfiguration configuration, IUriService uriService, IMemoryCache memoryCache)
        {
            _newsService = newsService;
            _configuration = configuration;
            _uriService = uriService;
            _memoryCache = memoryCache;
            _maxPageSize = _configuration.GetValue<int>("Limits:MaxPageSize");
            _minPageNumber = _configuration.GetValue<int>("Limits:MinPageNumber");
            _topStoriesLimit = _configuration.GetValue<int>("Limits:TopStories");
            _cacheExpiryMinutes = _configuration.GetValue<int>("Limits:CacheExpiryMinutes");
        }


        /// <summary>
        /// Returns list of stories with filter and pagination.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10, string searchText = "")
        {
            try
            {

                //Getting top stories with details either from cache or API
                Story[] results;
                bool IsDataExistInCache = _memoryCache.TryGetValue("NewsStories", out results);
                if (!IsDataExistInCache)
                {
                    results = await GetStories();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheExpiryMinutes));
                    _memoryCache.Set("NewsStories", results, cacheEntryOptions);
                }

                //Filtering stories based on user request.
                pageNumber = pageNumber < _minPageNumber ? _minPageNumber : pageNumber;
                pageSize = pageSize > _maxPageSize ? _maxPageSize : pageSize;
                IEnumerable<Story> filteredResult = filterStories(pageNumber, pageSize, searchText, results);

                var recordsCount = results.Count();

                //Creating pagintaion with the filtered data/stories.
                var route = Request.Path.Value;
                var pagedResponse = PagingHelper.CreatePagedReponse(filteredResult, pageNumber, pageSize, recordsCount, _uriService, route);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static IEnumerable<Story> filterStories(int pageNumber, int pageSize, string searchText, Story[] results)
        {
            return string.IsNullOrEmpty(searchText) ? results.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                                                : results
                                                .Where(x =>
                                                (string.IsNullOrEmpty(x.title) ? false : x.title.ToLower().Contains(searchText.ToLower()))
                                                ||
                                                (string.IsNullOrEmpty(x.url) ? false : x.url.ToLower().Contains(searchText.ToLower()))
                                                ).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }


        /// <summary>
        /// Requests ids of limited top stories and gets their details
        /// </summary>
        /// <returns></returns>
        private async Task<Story[]> GetStories()
        {
            var response = await _newsService.GetTopStories();
            var topStories = response.Take(_topStoriesLimit);

            List<Task<Story>> tasks = new List<Task<Story>>();
            foreach (var storyId in topStories)
            {
                tasks.Add(Task.Run(() => _newsService.GetStory(storyId)));
            }

            var results = await Task.WhenAll(tasks);
            return results;
        }
    }
}
