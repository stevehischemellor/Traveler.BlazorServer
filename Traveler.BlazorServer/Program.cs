using Microsoft.AspNetCore.HttpLogging;
using MudBlazor.Services;
using Serilog;
using Traveler.BlazorServer.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddLogging(loggingBuilder => 
    loggingBuilder.AddSerilog(dispose: true)
);


// Use only in Development (test heavily before turning on in Staging/Production)
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("x-api-version");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddScoped<ISitesService, SitesService>();
builder.Services.Configure<SitesServiceConfiguration>(builder.Configuration.GetSection("NPS"));
builder.Services.AddScoped<IJournalService, JournalService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");      
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpLogging();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
