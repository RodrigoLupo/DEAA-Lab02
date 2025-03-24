using Lab2_RodrigoLupo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_RodrigoLupo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController :ControllerBase
{
    private static readonly List<Product> Products = new();
    
    [HttpGet]
    public IActionResult GetAll() => Ok(Products);

    [HttpPost]
    public IActionResult Add([FromBody] Product product)
    {
        Products.Add(product);
        return Ok(new { message = "Product added", Products });
    }

    [HttpDelete("{index}")]
    public IActionResult Delete(int productIndex)
    {
        if (productIndex < 0 || productIndex >= Products.Count)
            return NotFound(new { message = "Product not found" });
        Products.RemoveAt(productIndex);
        return Ok(new { message = "Product removed", Products });
    }
}