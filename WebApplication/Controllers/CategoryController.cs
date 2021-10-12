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
            var result = await Task.Run(
                () =>
                {
                    var db = _dbContext.Set<СategoryEntity>().AsNoTracking().ToList();
                    var res = _mapper.Map<IEnumerable<Сategory>>(db);
                    return res;
                });
            return View(result ?? new List<Сategory>());
        }
    }
}
