using Microsoft.Extensions.Caching.Memory;
using WebApplication.Filters;
using WebApplication.Interfaces;
using WebApplication.Middleware;

namespace WebApplication.Controllers
{
    using AutoMapper;
    using DataAccessLayer.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using WebApplication.Models;
    
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;

        public CategoryController(ILogger<CategoryController> logger, IMapper mapper, ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return View(result ?? new List<Сategory>());
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> GetPicture(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            var result = await _categoryService.GetСategoryAsync((int)id);
            return View(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<ActionResult> GetImage(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            HttpContext.Items
                .TryGetValue(CacheFileMiddleware.HttpContextItemsCacheFileMiddlewareKey,
                    out var middlewareSetValue);
            if (middlewareSetValue != null)
            {
                return File((byte[])middlewareSetValue, "image/png");
            }

            var image = await _categoryService.GetСategoryImageAsync((int)id);
            return File(image, "image/png");
        }

        [HttpPost]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<ActionResult> GetPicture(int? id, IFormFile uploadedFile)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await _categoryService.GetСategoryAsync((int)id);
            await SavePictureAsync(category, uploadedFile);
            await _categoryService.EditСategoryAsync(category);
            return RedirectToAction(nameof(GetCategories));
        }

        private async Task<Сategory> SavePictureAsync(Сategory category, IFormFile uploadedFile)
        {
            if (category == null)
                return null;
            using (var memoryStream = new MemoryStream())
            {
                await uploadedFile?.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    category.Picture = memoryStream.ToArray();
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            return category;
        }
    }
}
