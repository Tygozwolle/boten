namespace RoeiVerenigingLibary;

public class Reservation
{
    public int UserId { get; set; }
    public int BoatId { get; set; }
    public string? Email { get; set; }
    public DateTime StartTijd { get; set; }
    public DateTime? EindTijd { get; set; }

    public Reservation(Member member, int boat, DateTime begin, DateTime eind)
    {
        this.Email = member.Email;
        this.UserId = member.Id;
        this.EindTijd = eind;
        this.StartTijd = begin;
        this.BoatId = boat;
    }
    //wpf comminuceert met dit en stuurt de details naar deze code.
    //in deze code word de data alleen verwerkt en doorgestuurd naar de dB als dat nodig is

    //public void Option_Button(object sender, RoutedEventArgs e) //assign this to window/button
    //{
    //  this._name = name.Text;
    //this._date = date.SelectedDate;
    //this._emailAdress = email.Text;
    //this._people = int.Parse(combo.Text);

    //}
}