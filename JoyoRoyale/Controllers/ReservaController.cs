using Crucero.Application.DTOs;
using Crucero.Application.Services;
using Crucero.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace JoyoRoyale.Web.Controllers
{
    [Route("Reserva")]
    public class ReservaController : Controller
    {
        private readonly IServiceReservas _serviceReserva;
        private readonly IServiceCruceros _serviceCrucero;
        private readonly IServiceHabitaciones _serviceHabitacion;
        private readonly IServiceComplementos _serviceComplemento;
        private readonly IServiceFechasCruceros _serviceFechaCrucero;
        private readonly IServicePrecioHabitaciones _servicePrecioHabitaciones;
        private readonly IServiceItinerarios _serviceItinerarios;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceCorreo _serviceCorreo;
        private readonly IMemoryCache _cache;
        private readonly ITipoCambioService _webTipoCambioService;
        private readonly ILogger<ServiceReservas> _logger;
        private readonly IServiceUsuarios _serviceUsuarios;

        private const string SessionReservaKey = "CurrentReservaData";



        public ReservaController(
        IServiceReservas serviceReserva,
        IServiceCruceros serviceCrucero,
        IServiceHabitaciones serviceHabitacion,
        IServiceComplementos serviceComplemento,
        IServiceFechasCruceros serviceFechaCrucero,
        ILogger<ServiceReservas> logger,
        IServicePrecioHabitaciones servicePrecioHabitaciones,
        IServiceItinerarios serviceItinerarios,
            IConverter converter,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceCorreo serviceCorreo,
            IMemoryCache cache,
            ITipoCambioService webTipoCambioService,
            IServiceUsuarios serviceUsuarios)

        {
            _serviceReserva = serviceReserva;
            _serviceCrucero = serviceCrucero;
            _serviceHabitacion = serviceHabitacion;
            _serviceComplemento = serviceComplemento;
            _serviceFechaCrucero = serviceFechaCrucero;
            _logger = logger;
            _servicePrecioHabitaciones = servicePrecioHabitaciones;
            _serviceItinerarios = serviceItinerarios;
            _converter = converter;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceCorreo = serviceCorreo;
            _cache = cache;
            _webTipoCambioService = webTipoCambioService;
            _serviceUsuarios = serviceUsuarios;
        }


        [HttpPost("ProcesarPago")]
        public async Task<IActionResult> ProcesarPago(
            [FromForm] int reservaId,
            [FromForm] string monto) // Recibir como string
        {
            try
            {
                // Convertir a decimal manualmente
                if (!decimal.TryParse(monto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal montoDecimal))
                {
                    return Json(new { success = false, message = "Formato de monto inválido" });
                }

                await _serviceReserva.ActualizarTotalReservaAsync(reservaId, montoDecimal);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("IndexAdmin")]
        public async Task<IActionResult> IndexAdmin(int? cruceroId, int? fechaCruceroId)
        {
            ViewBag.Cruceros = await _serviceCrucero.ListAsync();

            if (cruceroId.HasValue)
            {
                ViewBag.FechasDisponibles = await _serviceFechaCrucero.GetByCruceroIdAsync(cruceroId.Value);
            }

            var reservas = await _serviceReserva.ListByCruceroAndFechaAsync(cruceroId, fechaCruceroId);
            return View(reservas);
        }

        [HttpGet("ObtenerFechasPorCrucero/{cruceroId}")]
        public async Task<IActionResult> ObtenerFechasPorCrucero(int cruceroId)
        {
            var fechas = await _serviceFechaCrucero.GetByCruceroIdAsync(cruceroId);
            return Json(fechas.Select(f => new
            {
                id = f.Id,
                texto = f.FechaInicio.ToString("dd/MM/yyyy")
            }));
        }

        // GET: ReservaController
        [HttpGet("Index")]
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceReserva.ListByUserIdAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return View(collection);
        }

        // GET: ReservaController/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var reserva = await _serviceReserva.FindByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            var usuario = await _serviceUsuarios.FindByIdAsync(reserva.UsuarioId);
            ViewBag.Usuario = usuario;
            
            // Verificar si el tipo de cambio está en la caché
            var tipoCambio = _cache.Get<TipoCambioDto>("TipoCambioDolar");

            if (tipoCambio == null)
            {
                // Si no está en caché, obtenerlo y almacenarlo
                tipoCambio = await _webTipoCambioService.ObtenerYGuardarTipoCambioAsync();
            }

            if (tipoCambio != null)
            {
                ViewBag.TipoCambio = tipoCambio.Valor;
                ViewBag.FechaTipoCambio = tipoCambio.Fecha;
                Console.WriteLine($"Tipo de cambio en cache: {tipoCambio.Valor}");
            }
            else
            {
                ViewBag.TipoCambio = "No disponible";
                ViewBag.FechaTipoCambio = "Desconocida";
            }

            return View(reserva);
        }

        [HttpGet("GetFechasDisponibles")]
        public async Task<IActionResult> GetFechasDisponibles(int cruceroId)
        {
            try
            {
                var fechas = await _serviceFechaCrucero.GetFechasDisponiblesByCruceroIdAsync(cruceroId);

                var resultado = fechas.Select(f => new {
                    f.Id,
                    FechaInicio = f.FechaInicio.ToString("yyyy-MM-dd"),
                    FechaLimitePago = f.FechaLimitePago.ToString("yyyy-MM-dd")
                }).ToList();

                return Json(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener fechas para crucero {CruceroId}", cruceroId);
                return StatusCode(500, new { error = "Error interno al obtener fechas" });
            }
        }

        [HttpGet("GetHabitacionesDisponibles")]
        public async Task<IActionResult> GetHabitacionesDisponibles(int cruceroId, int fechaCruceroId)
        {
            try
            {
                // Obtener habitaciones disponibles para el crucero y fecha específicos
                var habitaciones = await _serviceHabitacion.GetHabitacionesDisponiblesAsync(cruceroId, fechaCruceroId);

                // Formatear la respuesta usando las propiedades existentes de tus modelos
                var resultado = habitaciones.Select(h => new
                {
                    habitacion = new
                    {
                        h.Habitacion.Id,
                        h.Habitacion.Nombre,
                        h.Habitacion.Descripcion,
                        h.Habitacion.CapacidadMinima,
                        h.Habitacion.CapacidadMaxima,
                        h.Habitacion.Tamano,
                        Imagen = Convert.ToBase64String(h.Habitacion.Imagen)
                    },
                    precio = h.Habitacion.PreciosHabitaciones.FirstOrDefault()?.Precio ?? 0m,
                    cantidadDisponible = h.CantidadDisponible
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habitaciones disponibles. CruceroId: {CruceroId}, FechaCruceroId: {FechaCruceroId}",
                    cruceroId, fechaCruceroId);
                return StatusCode(500, new { error = "Error interno al obtener habitaciones disponibles" });
            }
        }

        [Authorize]
        [HttpGet("GetObtenerDatosUsuario")]
        public IActionResult ObtenerDatosUsuario()
        {
            var nombre = User.Identity?.Name;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Json(new
            {
                nombre,
                email,
                usuarioId
            });
        }

        // GET: ReservaController/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Obtener datos necesarios para el formulario
                var cruceros = await _serviceCrucero.ListAsync();
                var complementos = await _serviceComplemento.ListAsync();

                // Aquí deberías obtener el ID del usuario autenticado en lugar del valor fijo
                // Por ejemplo: var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                ViewBag.Cruceros = cruceros;
                ViewBag.Complementos = complementos;
                ViewBag.UsuarioId = usuarioId;

                return View(new ReservasDTO
                {
                    UsuarioId = usuarioId,

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar datos para creación de reserva");
                return View("Error");
            }
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ReservasDTO dto)
        {
            try
            {
                // Crear la reserva
                int reservaId = await _serviceReserva.AddAsync(dto);

                // Buscar la reserva completa para la factura
                var reservaCreada = await _serviceReserva.FindByIdAsync(reservaId);

                // Enviar correo con factura
                var enviado = await EnviarFacturaPorCorreoAsync(reservaCreada);
                var mensajeCorreo = enviado ? "y el correo fue enviado correctamente." : "pero no se pudo enviar el correo.";

                return Ok(new
                {
                    success = true,
                    title = "Éxito",
                    message = $"Reserva creada correctamente {mensajeCorreo}",
                    reservaId = reservaId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reserva");
                return StatusCode(500, new
                {
                    success = false,
                    title = "Error",
                    message = "Ocurrió un error al crear la reserva. Por favor intente nuevamente."
                });
            }
        }


        private void CleanModelStateForDTO(ReservasDTO dto)
        {
            // Limpiar propiedades de navegación para evitar errores de validación
            ModelState.Remove("Usuario");
            ModelState.Remove("Crucero");

            if (dto.ReservasHabitaciones != null)
            {
                for (int i = 0; i < dto.ReservasHabitaciones.Count; i++)
                {
                    ModelState.Remove($"ReservasHabitaciones[{i}].Habitacion");
                    ModelState.Remove($"ReservasHabitaciones[{i}].Reserva");
                }
            }

            if (dto.ReservasComplementos != null)
            {
                for (int i = 0; i < dto.ReservasComplementos.Count; i++)
                {
                    ModelState.Remove($"ReservasComplementos[{i}].Complemento");
                    ModelState.Remove($"ReservasComplementos[{i}].Reserva");
                }
            }
        }

        [HttpGet("GetNombrePuertoSalidaAsync")]
        public async Task<IActionResult> GetNombrePuertoSalida(int cruceroId)
        {
            var nombre = await _serviceItinerarios.GetNombrePuertoSalidaAsync(cruceroId);
            return Ok(nombre);
        }

        [HttpGet("GetNombrePuertoRegresoAsync")]
        public async Task<IActionResult> GetNombrePuertoRegreso(int cruceroId)
        {
            var nombre = await _serviceItinerarios.GetNombrePuertoRegresoAsync(cruceroId);
            return Ok(nombre);
        }



        [HttpPost("VerificarDisponibilidad")]
        [HttpGet("VerificarDisponibilidad")]
        [Consumes("application/json")]
        public async Task<IActionResult> VerificarDisponibilidad(
    [FromQuery] int cruceroId,
    [FromQuery] int fechaCruceroId,
    [FromBody] Dictionary<int, int> habitacionesSolicitadas)
        {
            try
            {
                var resultado = await _serviceHabitacion.VerificarDisponibilidad(
                    cruceroId,
                    fechaCruceroId,
                    habitacionesSolicitadas);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar disponibilidad");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> FacturaReserva(int id)
        {
            var reserva = await _serviceReserva.FindByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            var usuario = await _serviceUsuarios.FindByIdAsync(reserva.UsuarioId);
            reserva.Usuario = usuario;

            var tipoCambio = _cache.Get<TipoCambioDto>("TipoCambioDolar");

            if (tipoCambio == null)
            {
                tipoCambio = await _webTipoCambioService.ObtenerYGuardarTipoCambioAsync();
            }

            // Crea ViewData personalizado
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
{
    { "TipoCambio", tipoCambio?.Valor },
    { "FechaTipoCambio", tipoCambio?.Fecha.ToShortDateString() ?? "Desconocida" }
};

            // Ya no uses ViewBag acá, solo pasá ViewData al render
            var html = await RenderViewToStringAsync("FacturaReserva", reserva, viewData);



            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    DocumentTitle = $"Factura Reserva #{reserva.Id}"
                },
                Objects = {
            new ObjectSettings
            {
                HtmlContent = html,
                WebSettings = {DefaultEncoding = "utf-8", LoadImages = true}
            }
        }
            };

            var file = _converter.Convert(pdf);

            // Forzar visualización en navegador como archivo PDF
            Response.Headers.Add("Content-Disposition", "inline; filename=FacturaReserva.pdf");
            return File(file, "application/pdf");
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model, ViewDataDictionary viewData = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.ActionDescriptor.ActionName;
            }

            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} no se encontró");
                }

                viewData ??= new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
                viewData.Model = model;

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewData,
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return writer.ToString();
            }
        }


        private async Task<bool> EnviarFacturaPorCorreoAsync(ReservasDTO reserva)
        {
            var usuario = await _serviceUsuarios.FindByIdAsync(reserva.UsuarioId);
            var destinatario = usuario.Correo;
            reserva.Usuario = usuario;

            var tipoCambio = _cache.Get<TipoCambioDto>("TipoCambioDolar");

            if (tipoCambio == null)
            {
                tipoCambio = await _webTipoCambioService.ObtenerYGuardarTipoCambioAsync();
            }

            // Crea ViewData personalizado
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
{
    { "TipoCambio", tipoCambio?.Valor },
    { "FechaTipoCambio", tipoCambio?.Fecha.ToShortDateString() ?? "Desconocida" }
};

            // Ya no uses ViewBag acá, solo pasá ViewData al render
            var html = await RenderViewToStringAsync("FacturaReserva", reserva, viewData);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    DocumentTitle = $"Factura Reserva #{reserva.Id}"
                },
                Objects = {
            new ObjectSettings
            {
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8", LoadImages = true }
            }
        }
            };

            var file = _converter.Convert(pdf);




            return await _serviceCorreo.SendEmail(
     destinatario,
     $"Factura de Reserva #{reserva.Id}",
     $"Estimado/a {usuario.Nombre},\n\n" +
     "Gracias por realizar su reserva con nosotros. " +
     "Adjunto encontrará la factura correspondiente a su reserva, en formato PDF.\n\n" +
     "Le recordamos que puede presentar este documento al momento del embarque.\n\n" +
     "Si tiene alguna consulta adicional, estaremos encantados de ayudarle.\n\n" +
     "¡Le deseamos un excelente viaje!",
     file,
     $"FacturaReserva_{reserva.Id}.pdf"
 );

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarFacturaPorCorreo(int id)
        {
            var reserva = await _serviceReserva.FindByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            var usuario = await _serviceUsuarios.FindByIdAsync(reserva.UsuarioId);
            reserva.Usuario = usuario;
            var destinatario = usuario.Correo;

            var tipoCambio = _cache.Get<TipoCambioDto>("TipoCambioDolar");

            if (tipoCambio == null)
            {
                tipoCambio = await _webTipoCambioService.ObtenerYGuardarTipoCambioAsync();
            }

            // Crea ViewData personalizado
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
{
    { "TipoCambio", tipoCambio?.Valor },
    { "FechaTipoCambio", tipoCambio?.Fecha.ToShortDateString() ?? "Desconocida" }
};

            // Ya no uses ViewBag acá, solo pasá ViewData al render
            var html = await RenderViewToStringAsync("FacturaReserva", reserva, viewData);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    DocumentTitle = $"Factura Reserva #{reserva.Id}"
                },
                Objects = {
            new ObjectSettings
            {
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8", LoadImages = true }
            }
        }
            };

            var file = _converter.Convert(pdf);

            
           

            var enviado = await _serviceCorreo.SendEmail(
                 destinatario,
    $"Reenvío de Factura - Reserva #{reserva.Id}",
    $"Estimado/a {usuario.Nombre},\n\n" +
    "Le reenviamos la factura correspondiente a su reserva. " +
    "Adjunto encontrará el documento en formato PDF.\n\n" +
    "Si tiene alguna duda o consulta, no dude en contactarnos.\n\n" +
    "Gracias por confiar en nosotros.",
    file,
    $"FacturaReserva_{reserva.Id}.pdf"
            );

            if (enviado)
            {
                TempData["Mensaje"] = "La factura fue enviada por correo correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo enviar la factura por correo.";
            }

           
            return RedirectToAction("Index");

        }




    }
}