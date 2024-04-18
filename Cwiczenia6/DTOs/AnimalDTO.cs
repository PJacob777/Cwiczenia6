namespace Cwiczenia6.DTOs;

public record GetAnimalResponse(int ID,string Name, string Desc, string Category, string Area);
public record CreateAnimalRequest(string Name, string Desc, string Category, string Area);