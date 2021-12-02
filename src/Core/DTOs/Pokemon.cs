namespace Core.DTOs;

public class Pokemon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Habitat { get; set; }
    public bool IsLegendary { get; set; }

    public Pokemon(string name, string description, string habitat, bool isLegendary) =>
    (Name, Description, Habitat, IsLegendary) = (name, description, habitat, isLegendary);
}
