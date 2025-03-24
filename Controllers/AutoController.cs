using Lab2_RodrigoLupo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2_RodrigoLupo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AutoController:ControllerBase
{
    private static readonly List<object> Autos = new();
    [HttpGet]
    public IActionResult GetAll() => Ok(Autos);

    [HttpPost]
    public IActionResult Add([FromBody] Auto<object> auto)
    {
        Autos.Add(auto);
        return Ok(new { message = "Persona adicionada", Autos});
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] object updAuto)
    {
        var index = Autos.FindIndex(x => ((dynamic)x).Id == id);
        if (index == -1) return NotFound(new { message = "Auto no encontrado" });

        Autos[index] = updAuto;
        return Ok(new { message = "Auto actualizado completamente", auto = updAuto });
    }
    [HttpPatch("{id}")]
    public IActionResult PatchAuto(int id, [FromBody] Dictionary<string, object> updates)
    {
        var auto = Autos.FirstOrDefault(a => ((dynamic)a).Id == id);
        if (auto == null) return NotFound(new { message = "Auto no encontrado" });

        var autoDict = auto.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(auto));

        foreach (var update in updates)
        {
            if (autoDict.ContainsKey(update.Key))
            {
                auto.GetType().GetProperty(update.Key)?.SetValue(auto, update.Value);
            }
        }

        return Ok(new { message = "Auto actualizado parcialmente", auto });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int index)
    {
        if (index < 0 || index >= Autos.Count)
            return NotFound(new { message = "Auto no encontrado" });
        Autos.RemoveAt(index);;
        return Ok(new { message = "Auto actualizado completamente", Autos });
    }
    
}