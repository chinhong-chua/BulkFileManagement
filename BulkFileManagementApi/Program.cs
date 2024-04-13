using BulkFileManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OriginFE",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                      });
});
builder.Services.AddScoped<IFileManipulationService, FileManipulationService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("OriginFE");
//Custom middleware to log request
app.Use(async (context, next) =>
{
    System.Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}]");
    await next(context);
});

app.UseAuthorization();

app.MapControllers();

app.Run();
