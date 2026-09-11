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

// var users = new List<User>
// {
//     new User
//     {
//         Id = 1,
//         Name = "Sazid",
//         IsActive = true,
//         Address = new Address {
//             City="Dhaka",
//             Country = "Bangladesh"
//         }
//     },

//     new User
//     {
//         Id = 2,
//         Name = "Suravee",
//         IsActive = false,
//         Address = new Address {
//             City="Dhaka",
//             Country = "Bangladesh"
//         }
//     }
// };

// var data = new Dictionary<string, object>
// {
//     ["name"] = "Zafy",
//     ["age"] = 1,
//     ["active"] = true
// };

// Console.WriteLine(JsonSerializer.Serialize("John \"Johnny\" Doe"));
// Console.WriteLine(JsonSerializer.Serialize("Line 1\nLine 2"));
// Console.WriteLine(JsonSerializer.Serialize(42));
// Console.WriteLine(JsonSerializer.Serialize(99.99));
// Console.WriteLine(JsonSerializer.Serialize(true));
// Console.WriteLine(JsonSerializer.Serialize(false));
// Console.WriteLine(JsonSerializer.Serialize(null));
// Console.WriteLine(JsonSerializer.Serialize(names));
// Console.WriteLine(JsonSerializer.Serialize(numbers));
// Console.WriteLine(JsonSerializer.Serialize(data));
// Console.WriteLine(JsonSerializer.Serialize(users));

// var parser = new JsonParser("true");

// var result = parser.Parse();

// Console.WriteLine(result);


// var parser = new JsonParser("\"Hello World\"");

// var result = parser.Parse();

// Console.WriteLine(result);

// var parser = new JsonParser("\"Hello\\nWorld\"");

// var result = parser.Parse();

// Console.WriteLine(result);

// var parser = new JsonParser("\"\\u0048\\u0065\\u006C\\u006C\\u006F\"");

// var result = parser.Parse();

// Console.WriteLine(result);

// Test("0");
// Test("42");
// Test("-42");
// Test("3.14");
// Test("-3.14");
// Test("1e5");
// Test("1.5e-3");

// static void Test(string json)
// {
//     var parser = new JsonParser(json);

//     var result = parser.Parse();

//     Console.WriteLine($"{json} => {result} ({result?.GetType().Name})");
// }


// var parser = new JsonParser(
//     """
//     {
//         "name": "Zafy",
//         "age": 25,
//         "active": true
//     }
//     """
// );

// var result = parser.Parse();

// Console.WriteLine(result);

// var dictionary = (Dictionary<string, object?>)result!;

// Console.WriteLine(dictionary["name"]);
// Console.WriteLine(dictionary["age"]);
// Console.WriteLine(dictionary["active"]);

// var parser = new JsonParser(
//     """
//     {
//         "name": "John",
//         "age": 25,
//         "scores": [90, 85, 95],
//         "address": {
//             "city": "Dhaka",
//             "country": "Bangladesh"
//         }
//     }
//     """
// );

// var result = parser.Parse();
// Console.WriteLine(result);
// var dictionary = (Dictionary<string, object?>)result!;

// Console.WriteLine(dictionary["name"]);
// Console.WriteLine(dictionary["age"]);

// var scores = (List<object?>)dictionary["scores"]!;

// foreach (var score in scores)
// {
//     Console.WriteLine(score);
// }

// string json = """

//    {
//     "name": "John",
//     "matrix": [
//         [1, 2],
//         [3, 4],
//         [5, 6]
//     ]

// }
// """;

// var user = JsonDeserializer.Deserialize<User>(json);

// Console.WriteLine(user?.Name);

// foreach (var row in user?.Matrix ?? [])
// {
//     Console.WriteLine(string.Join(", ", row));
// }
string json = """
{
    "name": "Suravee",
    "age": 25,
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "createdAt": "2026-09-11T20:00:00",
    "status": "Active"
}
""";
var user =
    JsonDeserializer.Deserialize<User>(json);

Console.WriteLine(user!.Name);
Console.WriteLine(user.Age);
Console.WriteLine(user.Id);
Console.WriteLine(user.CreatedAt);
Console.WriteLine(user.Status);

var data =
    JsonDeserializer.Deserialize<Dictionary<string, int>>(
        """{"math":90,"science":85}"""
    );

Console.WriteLine(data["math"]);
Console.WriteLine(data["science"]);

// public class User
// {
//     public int Id { get; set; }

//     public string Name { get; set; } = "";
//     public int Age { get; set; }

//     public bool IsActive { get; set; }
//     public Address Address { get; set; } = new Address();
// }

// public class User
// {
//     public string Name { get; set; } = "";

//     public List<int> Scores { get; set; } = [];
// }


// public class User
// {
//     public string Name { get; set; } = "";

//     public List<List<int>> Matrix { get; set; } = [];
// }

// public class User
// {
//     public string Name { get; set; } = "";
//     public int? Age { get; set; }
// }

public enum UserStatus
{
    Active,
    Inactive,
    Suspended
}

public class User
{
    public string Name { get; set; } = "";
    public int? Age { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserStatus Status { get; set; }
}

public class Address
{
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
}






