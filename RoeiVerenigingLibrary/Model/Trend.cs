namespace RoeiVerenigingLibrary.Model;

public class Trend
{
    public Trend(List<int> Values, Member loggedinMember)
    {
        this.Values = Values;
        this.LoggedinMember = loggedinMember;
    }

    public Trend(Statistic statistic, Member member, string description)
    {
        this.description = description;
        this.LoggedinMember = member;
        this.Statistic = statistic;
    }
    
    public List<int> Values;
    public Member LoggedinMember { get; set; }
    public Statistic Statistic { get; set; }
    public string description { get; set; }
}