using Lab2_RodrigoLupo.Models;

namespace Lab2_RodrigoLupo.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase
{
    private static readonly List<Persona> Personas = new();

    [HttpGet]
    public IActionResult GetAll() => Ok(Personas);

    [HttpPost]
    public IActionResult Add([FromBody] Persona persona)
    {
        Personas.Add(persona);
        return Ok(new { message = "Persona agregada", Personas });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Persona updatedPersona)
    {
        var index = Personas.FindIndex(p => p.Id == id);
        if (index == -1) return NotFound(new { message = "Persona no encontrada" });

        Personas[index] = updatedPersona;
        return Ok(new { message = "Persona actualizada completamente", persona = updatedPersona });
    }

    [HttpPatch("{id}")]
    public IActionResult PatchPersona(int id, [FromBody] Dictionary<string, object> updates)
    {
        var persona = Personas.FirstOrDefault(p => p.Id == id);
        if (persona == null) return NotFound(new { message = "Persona no encontrada" });

        var personaDict = persona.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(persona));

        foreach (var update in updates)
        {
            if (personaDict.ContainsKey(update.Key))
            {
                persona.GetType().GetProperty(update.Key)?.SetValue(persona, update.Value);
            }
        }

        return Ok(new { message = "Persona actualizada parcialmente", persona });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var index = Personas.FindIndex(p => p.Id == id);
        if (index == -1) return NotFound(new { message = "Persona no encontrada" });

        Personas.RemoveAt(index);
        return Ok(new { message = "Persona eliminada", Personas });
    }
}
