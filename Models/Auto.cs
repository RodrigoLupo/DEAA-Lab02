namespace Lab2_RodrigoLupo.Models;

public class Auto<T>
{
    public int Id { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Año { get; set; }
    public string Placa { get; set; }
    public T Propietario { get; set; }
}