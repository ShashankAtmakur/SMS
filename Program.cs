var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Add memory cache for storing temporary verification codes
builder.Services.AddMemoryCache();
// Register email service (reads SMTP settings from configuration)
builder.Services.AddSingleton<SMS.Services.EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Serve attached School Management static asset folder from /school-static/{*path}
// This maps requests like /school-static/bootstrap.css to the physical folder 'd:\source\School Management_files'
app.MapGet("/school-static/{**filePath}", async (string filePath, HttpContext context) =>
{
    var baseDir = Path.Combine("d:", "source", "School Management_files");
    // normalize and prevent path traversal
    var fullPath = Path.GetFullPath(Path.Combine(baseDir, filePath ?? string.Empty));
    if (!fullPath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase) || !System.IO.File.Exists(fullPath))
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Not found");
        return;
    }

    var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
    if (!provider.TryGetContentType(fullPath, out var contentType))
    {
        contentType = "application/octet-stream";
    }

    var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
    context.Response.ContentType = contentType;
    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
