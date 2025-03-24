using Lab2_RodrigoLupo.Models;

namespace Lab2_RodrigoLupo.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class EmpresaController : ControllerBase
{
    private static readonly List<Empresa> Empresas = new();

    [HttpGet]
    public IActionResult GetAll() => Ok(Empresas);

    [HttpPost]
    public IActionResult Add([FromBody] Empresa empresa)
    {
        Empresas.Add(empresa);
        return Ok(new { message = "Empresa agregada", Empresas });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Empresa updatedEmpresa)
    {
        var index = Empresas.FindIndex(e => e.Id == id);
        if (index == -1) return NotFound(new { message = "Empresa no encontrada" });

        Empresas[index] = updatedEmpresa;
        return Ok(new { message = "Empresa actualizada completamente", empresa = updatedEmpresa });
    }

    [HttpPatch("{id}")]
    public IActionResult PatchEmpresa(int id, [FromBody] Dictionary<string, object> updates)
    {
        var empresa = Empresas.FirstOrDefault(e => e.Id == id);
        if (empresa == null) return NotFound(new { message = "Empresa no encontrada" });

        var empresaDict = empresa.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(empresa));

        foreach (var update in updates)
        {
            if (empresaDict.ContainsKey(update.Key))
            {
                empresa.GetType().GetProperty(update.Key)?.SetValue(empresa, update.Value);
            }
        }

        return Ok(new { message = "Empresa actualizada parcialmente", empresa });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var index = Empresas.FindIndex(e => e.Id == id);
        if (index == -1) return NotFound(new { message = "Empresa no encontrada" });

        Empresas.RemoveAt(index);
        return Ok(new { message = "Empresa eliminada", Empresas });
    }
}
