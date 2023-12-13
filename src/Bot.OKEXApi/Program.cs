using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Bot.OKEXApi;

using RestEase;

// var a = new User {
// 	Age = 24,
// 	Name = "AAA"
// };

// var json = JsonSerializer.SerializeToUtf8Bytes(a);
// File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "./test.json"), json);
// 




// var str = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "test.json"));
// var a = JsonSerializer.Deserialize<User>(str);
// Console.WriteLine(a?.Name);
// 

// var a = new User {
// 	Age = 24,
// 	Name = "Test"
// };

// var json = JsonSerializer.Serialize(a, UserContext.Default.User);
// Console.WriteLine(json);




// var str = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "test.json"));
// var a = JsonSerializer.Deserialize<User>(str, UserContext.Default.User);
// Console.WriteLine(a?.Name);


// public sealed class User {
// 	[JsonPropertyName("age")]
// 	[JsonInclude]
// 	public int Age;
// 	[JsonPropertyName("name")]
// 	[JsonInclude]
// 	public string Name = "";
// }

// // [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
// [JsonSerializable(typeof(User), GenerationMode = JsonSourceGenerationMode.Metadata)]
// public partial class UserContext: JsonSerializerContext {

// }
// 

var api = new OKEXApi();


var resp = await api.GetPendingOrderList("FUTURES", "", "partially_filled");
Console.WriteLine(resp.Code);