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
        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNewss()
        {
            //.AsSplitQuery() 
            return await _context.Newss
                            .Include(n => n.Category)
                            .Include(n => n.Content)
                            .Include(n => n.ImageLink)
                            .AsSplitQuery()
                            .ToListAsync();
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


    }
}
