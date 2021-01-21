using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.CrawlerModels;
using AutoMapper;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ClawlerContext _context;
        private readonly IMapper _mapper;

        public NewsController(ClawlerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/News
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<List<News>>> GetNewss(int skip, int take)
        {

            //.AsSplitQuery() 
            return await _context.Newss
                            .OrderByDescending(x=>x.Id)
                            .Skip(skip)
                            .Take(take)
                            .Include(n => n.Category)
                            .Include(n => n.Content)
                            .Include(n => n.ImageLink)
                            .AsSplitQuery()
                            .ToListAsync();
        }
        
        // GET: api/Count
        [HttpGet("count/{catId}")]
        public ActionResult<int> GetCount(long catId)
        {
            if(catId==0){
                return _context.Newss.Count();
            }
            return _context.Newss.Where(x=> x.CategoryRefId==catId).Count();
            
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(long id)
        {
            //var news = await _context.Newss.FindAsync(id);
            var news = await _context.Newss
                            .Include(n => n.Category)
                            .Include(n => n.Content)
                            .Include(n => n.ImageLink)
                            .AsSplitQuery()
                            .Where(n => n.Id ==id)
                            .FirstAsync();
            if (news == null)
            {
                return NotFound();
            }
            
            return news;
        }

         // GET: api/News/Cat
        [HttpGet("GetCats")]
        public async Task<ActionResult<List<Category>>> GetCats(){
            return await _context.Categories.ToListAsync();
        }
    }
}
