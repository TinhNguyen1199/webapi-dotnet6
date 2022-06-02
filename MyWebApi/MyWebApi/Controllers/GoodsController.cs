using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        public  static List<Goods> merchandise = new List<Goods> 
        {
            new Goods
            {
                GoodsId = Guid.NewGuid(),
                GoodsName = "Lap Top",
                GoodsPrice = 1500000
            },
            new Goods
            {
                GoodsId = Guid.NewGuid(),
                GoodsName = "Smart Phone",
                GoodsPrice = 950000
            },
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(merchandise);
        }

        [HttpPost]
        public IActionResult Create(GoodsVM request )
        {
            var goods = new Goods()
            {
                GoodsId = Guid.NewGuid(),
                GoodsName = request.GoodsName,
                GoodsPrice = request.GoodsPrice,
            };
            merchandise.Add(goods);
            return Ok(goods);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
           try
            {
                var goods = merchandise.SingleOrDefault(m => m.GoodsId == Guid.Parse(id));
                if (goods == null) return NotFound();
                return Ok(goods);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, GoodsVM request)
        {
            try
            {
                var goods = merchandise.Find(m => m.GoodsId == Guid.Parse(id));
                if( goods == null) return NotFound();

                goods.GoodsName = request.GoodsName;
                goods.GoodsPrice = request.GoodsPrice;
                
                return Ok(goods);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var goods = merchandise.Find(m => m.GoodsId == Guid.Parse(id));
            if(goods == null) return NotFound();

            merchandise.Remove(goods);
            return NoContent();
        }
    }
}
