using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private int _maxPageSize;
        private int _minPageNumber;
        private int _topStoriesLimit;
        public NewsController(INewsService newsService, IConfiguration configuration, IUriService uriService)
        {
            _newsService = newsService;
            _configuration = configuration;
            _uriService = uriService;
            _maxPageSize = _configuration.GetValue<int>("Limits:MaxPageSize");
            _minPageNumber = _configuration.GetValue<int>("Limits:MinPageNumber");
            _topStoriesLimit = _configuration.GetValue<int>("Limits:TopStories");
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
                pageNumber = pageNumber < _minPageNumber ? _minPageNumber : pageNumber;
                pageSize = pageSize > _maxPageSize ? _maxPageSize : pageSize;
                
                var route = Request.Path.Value;
                
                var topStories =  _newsService.GetTopStories().Result.Take(_topStoriesLimit);
                
                List<Task<Story>> tasks = new List<Task<Story>>();
                foreach (var storyId in topStories)
                {
                    tasks.Add(Task.Run(() => _newsService.GetStory(storyId)));
                }
                
                var results =   await Task.WhenAll(tasks);
                
                var filteredResult = string.IsNullOrEmpty(searchText) ? results.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                                    : results
                                    .Where(x =>
                                    (string.IsNullOrEmpty(x.title) ? false : x.title.ToLower().Contains(searchText.ToLower()))
                                    ||
                                    (string.IsNullOrEmpty(x.url) ? false : x.url.ToLower().Contains(searchText.ToLower()))
                                    ).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                
                var recordsCount = results.Count();
                
                var pagedResponse = PagingHelper.CreatePagedReponse(filteredResult, pageNumber, pageSize, recordsCount, _uriService, route);
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
