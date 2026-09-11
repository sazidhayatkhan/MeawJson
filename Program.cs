using MeawJson;

// var user = new User
// {
//     Id = 1,
//     Name = "Sazid",
//     IsActive = true,
//     Address = new Address
//     {
//         City = "Dhaka",
//         Country = "Bangladesh"
//     }
// };


// var numbers = new[] { 1, 2, 3, 4, 5 };



// var names = new List<string>
// {
//     "Sazid",
//     "Suravee",
//     "Zafy"
// };



// var users = new List<User>
// {
//     new User
//     {
//         Id = 1,
//         Name = "Sazid",
//         IsActive = true
//     },

//     new User
//     {
//         Id = 2,
//         Name = "Suravee",
//         IsActive = false
//     }
// };

var users = new List<User>
{
    new User
    {
        Id = 1,
        Name = "Sazid",
        IsActive = true,
        Address = new Address {
            City="Dhaka",
            Country = "Bangladesh"
        }
    },

    new User
    {
        Id = 2,
        Name = "Suravee",
        IsActive = false,
        Address = new Address {
            City="Dhaka",
            Country = "Bangladesh"
        }
    }
};

var data = new Dictionary<string, object>
{
    ["name"] = "Zafy",
    ["age"] = 1,
    ["active"] = true
};

// Console.WriteLine(JsonSerializer.Serialize("John \"Johnny\" Doe"));
// Console.WriteLine(JsonSerializer.Serialize("Line 1\nLine 2"));
// Console.WriteLine(JsonSerializer.Serialize(42));
// Console.WriteLine(JsonSerializer.Serialize(99.99));
// Console.WriteLine(JsonSerializer.Serialize(true));
// Console.WriteLine(JsonSerializer.Serialize(false));
// Console.WriteLine(JsonSerializer.Serialize(null));
// Console.WriteLine(JsonSerializer.Serialize(names));
// Console.WriteLine(JsonSerializer.Serialize(numbers));
Console.WriteLine(JsonSerializer.Serialize(data));
// Console.WriteLine(JsonSerializer.Serialize(users));

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






