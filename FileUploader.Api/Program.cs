using FileUploader.Domain.Data;
using FileUploader.Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp", 
    policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

#region Service Injected
builder.Services.AddScoped<IFileService, FileService>();

#endregion

var app = builder.Build();

app.UseCors("corsapp");

app.MapControllers();

app.Run();
