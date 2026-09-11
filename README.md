# MeawJson

A tiny JSON library, hand written in C#, with no dependencies. Just because sometimes it's fun and a good exercise to roll your own.

It has three little pieces:

| What | File(s) | Job |
|---|---|---|
| Parser | `JsonParser.cs` (+ `.String`, `.Number`, `.Array`, `.Object`) | Reads JSON text and turns it into C# objects |
| Serializer | `JsonSerializer.cs` | Turns C# objects into JSON text |
| Deserializer | `JsonDeserializer.cs` (+ `.Object`, `.Collection`, `.Dictionary`) | The friendly layer that converts parsed JSON into your own types |

---

## How to run the tests

`Program.cs` is a small test harness. It prints what it's testing, runs it, and shows the result or the error. Just build and run:

```sh
dotnet run
```

PRs and ideas welcome
