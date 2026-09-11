using MeawJson;

var user = new User
{
    Id = 1,
    Name = "Sazid",
    IsActive = true,
    Address = new Address
    {
        City = "Dhaka",
        Country = "Bangladesh"
    }
};

Console.WriteLine(JsonSerializer.Serialize(user));

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public bool IsActive { get; set; }
    public Address Address { get; set; } = new Address();
}

public class Address
{
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
}





// Console.WriteLine(JsonSerializer.Serialize("John \"Johnny\" Doe"));
// Console.WriteLine(JsonSerializer.Serialize("Line 1\nLine 2"));
// Console.WriteLine(JsonSerializer.Serialize(42));
// Console.WriteLine(JsonSerializer.Serialize(99.99));
// Console.WriteLine(JsonSerializer.Serialize(true));
// Console.WriteLine(JsonSerializer.Serialize(false));
// Console.WriteLine(JsonSerializer.Serialize(null));