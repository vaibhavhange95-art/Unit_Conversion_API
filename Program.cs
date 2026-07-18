using Unit_Conversion_API.Services.Implementation;
using Unit_Conversion_API.Services.Interfaces;
using Unit_Conversion_API.Middleware;
using Unit_Conversion_API.Repositories.Interfaces;
using Unit_Conversion_API.Repositories.Implementation;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddScoped<IConversionService, ConversionService>();
builder.Services.AddSingleton<IUnitRepository, InMemoryUnitRepository>();
// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();