using MicrosoftEntra.VerifiedId;

var builder = WebApplication.CreateBuilder(args);

// Retrieve configuration.
var requestClientOptions = new RequestClientOptions();
builder.Configuration.Bind("MicrosoftEntra", requestClientOptions);
builder.Services.AddSingleton<RequestClientOptions>(requestClientOptions);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
