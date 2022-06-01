using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Data;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoryController(MyDbContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryModel request)
        {
            var newcategory = new Category
            {
                CategoryName = request.CategoryName,
            };
            _context.Add(newcategory);
            _context.SaveChanges();

            return Ok(newcategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Update(int id, CategoryModel request)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category != null) 
            {
                category.CategoryName = request.CategoryName;
                _context.SaveChanges();
                return Ok(category);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Delete Success!!");
        }


    }
}
