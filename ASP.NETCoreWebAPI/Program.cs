using Application;
using ASP.NETCoreWebAPI.CustomMiddleware;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<CoreWebAppDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:conn"]));
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();
app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials()); // allow credentials



//var serviceProvider = app.Services;
//using (var serviceScope = serviceProvider.CreateScope())
//{
//    var context = serviceScope.ServiceProvider.GetRequiredService<CoreWebAppDbContext>();
//    context.Database.EnsureDeleted();
//    context.Database.Migrate();
//    context.Database.EnsureCreated();
//    RelationalDatabaseCreator databaseCreator =
//     (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
//    databaseCreator.CreateTables();
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
