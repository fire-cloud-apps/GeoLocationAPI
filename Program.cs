using GeoLocationAPI.DBHandler;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

#region Application Scope & JWT Middleware
services.AddHttpContextAccessor();//TO Access HTTPContext from constructor.
services.AddTransient(typeof(IConnectionService), typeof(ConnectionService));
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
