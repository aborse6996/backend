using System.Text.Json;
using MyApi.Models;

namespace MyApi.Services;

public class UserService
{
    private readonly string _filePath;

    public UserService(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(
            environment.ContentRootPath,
            "..",
            "database.json"
        );
    }

    public async Task<List<User>> GetUsers()
    {
        if (!File.Exists(_filePath))
        {
            return new List<User>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<User>();
        }

        return JsonSerializer.Deserialize<List<User>>(json)
               ?? new List<User>();
    }

    public async Task SaveUsers(List<User> users)
    {
        var json = JsonSerializer.Serialize(
            users,
            new JsonSerializerOptions
            {
                WriteIndented = true
            }
        );

        await File.WriteAllTextAsync(_filePath, json);
    }
}