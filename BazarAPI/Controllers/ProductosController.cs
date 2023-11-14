using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BazarAPI.Context;
using System.Linq;
using System.Threading.Tasks;
using BazarAPI.Models;

namespace BazarAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly BazarContext _context;

        public ProductosController(BazarContext context)
        {
            _context = context;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProductos([FromQuery] string? search)
        {
            IQueryable<Producto> query = _context.Productos;

            if (!string.IsNullOrEmpty(search)) // Verifica si 'search' no es nulo y no es una cadena vacía
            {
                string searchLower = search!.ToLower(); // El '!' es el operador de supresión de nulos, solo necesario si tienes habilitados los tipos de referencia anulables
                query = query.Where(p => p.Title.ToLower().Contains(searchLower));
            }

            var productos = await query
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Price,
                    p.DiscountPercentage,
                    p.Rating,
                    p.Stock,
                    p.Brand,
                    p.Category,
                    p.Thumbnail,
                    Images = p.Images.Select(img => img.ImageUrl).ToList() // Solo seleccionamos la URL de las imágenes
                })
                .ToListAsync();

            return Ok(productos); // Retorna la lista de productos con sus imágenes
        }


        // GET: api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Price,
                    p.DiscountPercentage,
                    p.Rating,
                    p.Stock,
                    p.Brand,
                    p.Category,
                    p.Thumbnail,
                    Images = p.Images.Select(img => img.ImageUrl).ToList() // Solo seleccionamos la URL de las imágenes
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto); // Retorna el producto específico con sus imágenes
        }
    }
}
