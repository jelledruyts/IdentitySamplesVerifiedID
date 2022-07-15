using Microsoft.Identity.Client;
using MicrosoftEntra.VerifiedId;
using MicrosoftEntra.VerifiedId.Client;

var builder = WebApplication.CreateBuilder(args);

// Retrieve configuration.
var requestClientOptions = new IssuanceRequestClientOptions();
builder.Configuration.Bind("EntraVerifiedId", requestClientOptions);

// Add Verified ID services.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IssuanceRequestClientOptions>(requestClientOptions);
builder.Services.AddScoped<IssuanceRequestClient>();

// Add MSAL services.
var msalOptions = new ConfidentialClientApplicationOptions();
builder.Configuration.Bind("EntraVerifiedId", msalOptions);
var confidentialClientApplication = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(msalOptions).Build();
builder.Services.AddSingleton<IConfidentialClientApplication>(confidentialClientApplication);

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

app.Run();
