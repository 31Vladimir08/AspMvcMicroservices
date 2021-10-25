namespace WebApplication.Controllers
{
    using AutoMapper;

    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Collections.Generic;
    using System.IO;
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

        [HttpGet]
        public async Task<IActionResult> GetPicture(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var res = await _dbContext.Set<СategoryEntity>().FirstOrDefaultAsync(x => x.CategoryID == id);
            var result = _mapper.Map<Сategory>(res);
            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var res = await _dbContext.Set<СategoryEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.CategoryID == id);
            var image = res.Picture;
            return File(image, "image/png");
        }

        [HttpPost]
        public async Task<ActionResult> GetPicture(long? id, IFormFile uploadedFile)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            var category = await SavePictureAsync((long)id, uploadedFile);


            return View(category);
        }

        private async Task<Сategory> SavePictureAsync(long id, IFormFile uploadedFile)
        {
            var result = await Task.Run(
                () =>
                {
                    var db = _dbContext.Set<СategoryEntity>().AsNoTracking().FirstOrDefault(x => x.CategoryID == id);
                    var category = _mapper.Map<Сategory>(db);
                    using (var memoryStream = new MemoryStream())
                    {
                        uploadedFile.CopyTo(memoryStream);

                        // Upload the file if less than 2 MB
                        if (memoryStream.Length < 2097152)
                        {
                            category.Picture = memoryStream.ToArray();
                            db = _mapper.Map<СategoryEntity>(category);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                    }
                    try
                    {
                        _dbContext.Database.BeginTransaction();
                        _dbContext.Set<СategoryEntity>().Update(db);

                        _dbContext.SaveChanges();
                        _dbContext.Database.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Database.RollbackTransaction();
                        throw;
                    }

                    return category;
                });
            return result;
        }
    }
}
