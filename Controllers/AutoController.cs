using Lab2_RodrigoLupo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lab2_RodrigoLupo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AutoController:ControllerBase
{
    private static readonly List<Auto<object>> Autos = new();
    [HttpGet]
    public IActionResult GetAll() => Ok(Autos);

    [HttpPost]
    public IActionResult Add([FromBody] Auto<object> auto)
    {
        Autos.Add(auto);
        return Ok(new { message = "Persona adicionada", Autos});
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Auto<object> updAuto)
    {
        var index = Autos.FindIndex(x => ((dynamic)x).Id == id);
        if (index == -1) return NotFound(new { message = "Auto no encontrado" });

        Autos[index] = updAuto;
        return Ok(new { message = "Auto actualizado completamente", auto = updAuto });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int index)
    {
        if (index < 0 || index >= Autos.Count)
            return NotFound(new { message = "Auto no encontrado" });
        Autos.RemoveAt(index);
        return Ok(new { message = "Auto actualizado completamente", Autos });
    }
    
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] JsonPatchDocument? patchDoc)
    {
        if (patchDoc == null)
            return BadRequest(new { message = "El documento de parcheo es nulo o incorrecto" });
        
        var auto = Autos.FirstOrDefault(a => a.Id == id);
        if (auto == null)
            return NotFound(new { message = "Auto no encontrado" });
        try
        {
            patchDoc.ApplyTo(auto, error =>
            {
                ModelState.AddModelError(error.Operation.path, error.ErrorMessage);
            });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al aplicar el parche", error = ex.Message });
        }

        return Ok(new { message = "Auto actualizado parcialmente", auto });
    }
    
}