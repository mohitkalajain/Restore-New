using API.Data;
using API.Middleware;
using API.Repository.Implementation;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt=>{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IResponseService,ResponseService>();
builder.Services.AddScoped<IBasketService,BasketService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors(options=>{
options.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});
app.UseAuthorization();

app.MapControllers();

var scope=app.Services.CreateScope();
var context=scope.ServiceProvider.GetRequiredService<StoreContext>();
var logger=scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    context.Database.Migrate();
   await DbInitializer.Initialize(context);
}
catch (Exception ex)
{
    logger.LogError(ex,"A problem occured during migrations");
}

app.Run();
