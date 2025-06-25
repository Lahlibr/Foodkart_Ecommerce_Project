

public static class FileHelper
{
    public static IFormFile CreateFormFileFromPath(string filePath)
    {
        var stream = File.OpenRead(filePath);
        return new FormFile(stream, 0, stream.Length, "image", Path.GetFileName(filePath))
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg" // change if your file is png, gif, etc
        };
    }
}
