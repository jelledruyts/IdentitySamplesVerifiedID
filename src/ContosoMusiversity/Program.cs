using MicrosoftEntra.VerifiedId.Client;

var builder = WebApplication.CreateBuilder(args);

// Add Verified ID issuance services.
builder.Services.AddVerifiedIdIssuance(builder.Configuration.GetSection("EntraVerifiedId"));
builder.Services.AddVerifiedIdWellKnownEndpoints(builder.Configuration.GetSection("EntraVerifiedId"));

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();
app.UseVerifiedIdWellKnownEndpoints();

app.Run();
