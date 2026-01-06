using Microsoft.AspNetCore.Hosting;

namespace LibraryManagementSystem.Service.Email;

public class EmailAssetLoader
{
    private readonly IWebHostEnvironment _env;

    public EmailAssetLoader(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] LoadLogoPng()
    {
        var path = Path.Combine(_env.WebRootPath, "images", "logo.png");
        return File.Exists(path) ? File.ReadAllBytes(path) : Array.Empty<byte>();
    }
}
