namespace Model;

public class Location : BaseEntity
{
    public string Place { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string Region { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}