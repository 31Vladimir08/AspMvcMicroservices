namespace WebApplication.Controllers
{
    using AutoMapper;

    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using WebApplication.Models;

    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;

        public CategoryController(ILogger<CategoryController> logger, IMapper mapper, IAplicationDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var db = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
            var result = _mapper.Map<IEnumerable<Сategory>>(db);
            return View(result ?? new List<Сategory>());
        }
    }
}
