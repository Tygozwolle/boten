namespace RoeiVerenigingLibrary.Model;

public class Statistic( int id, string name, string description, string value, bool selected)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public string Value { get; set; } = value;
    public bool Selected { get; set; } = selected;
}