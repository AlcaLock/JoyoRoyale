using Crucero.Application.Profiles;
using Crucero.Application.Services.Implementations;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Repository.Implementations;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using System.Text;
using Crucero.Application.Services;
using JoyoRoyale.Infraestructure.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using ProyectoPDF.Extension;
using System.Globalization;
using Crucero.Application.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using IServiceCorreo = Crucero.Application.Services.Interfaces.IServiceCorreo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<wsindicadoreseconomicosSoapClient>(_ =>
    new wsindicadoreseconomicosSoapClient(
        wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap
    )
);

// Mapeo de la clase AppConfig para leer appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddMemoryCache();
//***********************
// Configurar D.I.
//Repository
builder.Services.AddTransient<IRepositoryBarcos, RepositoryBarcos>();
builder.Services.AddTransient<IRepositoryBarcoHabitaciones, RepositoryBarcoHabitaciones>();
builder.Services.AddTransient<IRepositoryCruceros, RepositoryCruceros>();
builder.Services.AddTransient<IRepositoryHabitaciones, RepositoryHabitaciones>();
builder.Services.AddTransient<IRepositoryReservas, RepositoryReservas>();
builder.Services.AddTransient<IRepositoryItinerarios, RepositoryItinerario>();
builder.Services.AddTransient<IRepositoryFechasCruceros, RepositoryFechaCrucero>();
builder.Services.AddTransient<IRepositoryPreciosHabitaciones, RepositoryPrecioHabitacion>();
builder.Services.AddTransient<IRepositoryPuertos, RepositoryPuertos>();
builder.Services.AddTransient<IRepositoryComplementos, RepositoryComplementos>();
builder.Services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();

//Services
builder.Services.AddTransient<IServiceBarcos, ServiceBarcos>();
builder.Services.AddTransient<IServiceBarcoHabitaciones, ServiceBarcoHabitaciones>();
builder.Services.AddTransient<IServiceCruceros, ServiceCruceros>();
builder.Services.AddTransient<IServiceHabitaciones, ServiceHabitaciones>();
builder.Services.AddTransient<IServiceReservas, ServiceReservas>();
builder.Services.AddTransient<IServiceFechasCruceros, ServiceFechaCrucero>();
builder.Services.AddTransient<IServiceItinerarios, ServiceItinerarios>();
builder.Services.AddTransient<IServicePrecioHabitaciones, ServicePrecioHabitacion>();
builder.Services.AddTransient<IServicePuertos, ServicePuertos>();
builder.Services.AddTransient<IServiceComplementos, ServiceComplementos>();
builder.Services.AddTransient<IServiceCorreo, ServiceCorreo>();
builder.Services.AddScoped<IServiceUsuarios, ServiceUsuarios>();
builder.Services.AddScoped<IServicePaises, ServicePaises>();

builder.Services.AddTransient<ITipoCambioService, WebTipoCambioService>();
builder.Services.AddHostedService<TipoCambioHostedService>();

//Seguridad
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Login/Forbidden/";
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});

//Configurar Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<BarcoProfile>();
    config.AddProfile<BarcoHabitacionesProfile>();
    config.AddProfile<CrucerosProfile>();

    config.AddProfile<ComplementosProfile>();
    config.AddProfile<FechasCrucerosProfile>();
    config.AddProfile<HabitacionesProfile>();
    config.AddProfile<HuespedesProfile>();
    config.AddProfile<ItinerariosProfile>();
    config.AddProfile<PrecioHabitacionesProfile>();
    config.AddProfile<PuertosProfile>();
    config.AddProfile<ReservasComplentosProfile>();
    config.AddProfile<ReservasHabitacionProfile>();
    config.AddProfile<ReservasProfile>();
    config.AddProfile<RolesProfile>();
    config.AddProfile<UsuariosProfile>();

});
// Configuar Conexi�n a la Base de Datos SQL
builder.Services.AddDbContext<JoyoRoyaleContext>(options =>
{
    // it read appsettings.json file

    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});
//***********************
//Configuraci�n Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
                    // Limitar la informaci�n de depuraci�n
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    // Log LogEventLevel.Verbose muestra mucha informaci�n, pero no es necesaria solo para el proceso de depuraci�n
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);
//***************************
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "LibreriaPDF/libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Establecer cultura por defecto a Costa Rica
var defaultCulture = new CultureInfo("es-CR");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseMiddleware<Crucero.Web.Middleware.ErrorHandlingMiddleware>();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();


app.UseAuthorization();

// Activar Antiforgery 
app.UseAntiforgery();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
