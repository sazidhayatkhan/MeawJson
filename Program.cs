
using MeawJson ;



Console.WriteLine("===== Meaw JSON TESTS =====");

Console.WriteLine("\n1. Primitive serialization");

Console.WriteLine(JsonSerializer.Serialize(25));
Console.WriteLine(JsonSerializer.Serialize("Hello"));
Console.WriteLine(JsonSerializer.Serialize(true));
Console.WriteLine(JsonSerializer.Serialize(null));

Console.WriteLine("\n2. Object serialization");

var user = new User
{
    Name = "John",
    Age = 25,
    IsActive = true
};

string userJson = JsonSerializer.Serialize(user);

Console.WriteLine(userJson);

Console.WriteLine("\n3. Nested object");

user.Address = new Address
{
    City = "Dhaka",
    Country = "Bangladesh"
};

Console.WriteLine(JsonSerializer.Serialize(user));

Console.WriteLine("\n4. List serialization");

var numbers = new List<int>
{
    10,
    20,
    30
};

Console.WriteLine(JsonSerializer.Serialize(numbers));

Console.WriteLine("\n5. List of objects");

var users = new List<User>
{
    new User
    {
        Name = "John",
        Age = 25
    },
    new User
    {
        Name = "Sarah",
        Age = 30
    }
};

Console.WriteLine(JsonSerializer.Serialize(users));

Console.WriteLine("\n6. Dictionary");

var scores = new Dictionary<string, object?>
{
    ["math"] = 90,
    ["science"] = 85,
    ["passed"] = true
};

Console.WriteLine(JsonSerializer.Serialize(scores));

Console.WriteLine("\n7. Simple deserialization");

var json = """
{
    "Name": "John",
    "Age": 25,
    "IsActive": true
}
""";

var deserializedUser =
    JsonDeserializer.Deserialize<User>(json);

Console.WriteLine(deserializedUser!.Name);
Console.WriteLine(deserializedUser.Age);
Console.WriteLine(deserializedUser.IsActive);

Console.WriteLine("\n8. Nested object deserialization");

var nestedJson = """
{
    "Name": "John",
    "Age": 25,
    "IsActive": true,
    "Address": {
        "City": "Dhaka",
        "Country": "Bangladesh"
    }
}
""";

var nestedUser =
    JsonDeserializer.Deserialize<User>(nestedJson);

Console.WriteLine(nestedUser!.Address!.City);
Console.WriteLine(nestedUser.Address.Country);

Console.WriteLine("\n9. List deserialization");

var listJson = "[10, 20, 30, 40]";

var numbers2 =
    JsonDeserializer.Deserialize<List<int>>(listJson);

foreach (var number in numbers2!)
{
    Console.WriteLine(number);
}

Console.WriteLine("\n10. Array deserialization");

var array =
    JsonDeserializer.Deserialize<int[]>(
        "[1, 2, 3, 4]"
    );

Console.WriteLine(array!.Length);
Console.WriteLine(array[0]);
Console.WriteLine(array[3]);

Console.WriteLine("\n11. Dictionary deserialization");

var dictionary =
    JsonDeserializer.Deserialize<Dictionary<string, int>>(
        """
        {
            "math": 90,
            "science": 85
        }
        """
    );

Console.WriteLine(dictionary!["math"]);
Console.WriteLine(dictionary["science"]);

Console.WriteLine("\n12. Nullable value");

var nullableUser =
    JsonDeserializer.Deserialize<User>(
        """
        {
            "Name": "John",
            "Age": 25,
            "IsActive": true,
            "Address": null
        }
        """
    );

Console.WriteLine(nullableUser!.Address == null);

Console.WriteLine("\n13. Special types");

Guid id = Guid.NewGuid();

var specialUser = new SpecialUser
{
    Name = "John",
    CreatedAt = DateTime.Now,
    Id = id,
    Role = UserRole.Admin
};

string specialJson =
    JsonSerializer.Serialize(specialUser);

Console.WriteLine(specialJson);

Console.WriteLine("\n14. Invalid JSON");

try
{
    JsonDeserializer.Deserialize<User>(
        """{"Name":"John","Age":}"""
    );
}
catch (JsonException ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

Console.WriteLine("\n15. Invalid type");

try
{
    JsonDeserializer.Deserialize<User>(
        """
        {
            "Name": "John",
            "Age": "hello"
        }
        """
    );
}
catch (JsonException ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

Console.WriteLine("\n===== TESTS FINISHED =====");

public enum UserRole
{
    User,
    Admin
}

public class Address
{
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
}

public class User
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public Address? Address { get; set; }
    public List<string> Hobbies { get; set; } = [];
}

public class SpecialUser
{
    public string Name { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public Guid Id { get; set; }
    public UserRole Role { get; set; }
}

