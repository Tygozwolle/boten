namespace RoeiVerenigingLibrary.Model;

public class Statistic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
    public bool Selected { get; set; }

    public Statistic(int id, string name, string description, string value, bool selected)
    {
        Id = id;
        Name = name;
        Description = description;
        Value = value;
        Selected = selected;
    }
}