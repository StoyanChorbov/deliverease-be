namespace Model;

public class Location : BaseEntity
{
    public string Address { get; set; }
    public string City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}