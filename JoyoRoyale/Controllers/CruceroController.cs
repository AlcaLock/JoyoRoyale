using Crucero.Application.DTOs;
using Crucero.Application.Services;
using Crucero.Application.Services.Implementations;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JoyoRoyale.Web.Controllers
{
    public class CruceroController : Controller
    {
        private readonly IServiceCruceros _serviceCrucero;
        private readonly IServiceBarcos _serviceBarco;
        private readonly IServicePuertos _servicePuerto;
        private readonly IServiceItinerarios _serviceItinerarios;
        private readonly IServiceFechasCruceros _serviceFechaCruceros;


        public CruceroController(
            IServiceCruceros serviceCrucero,
            IServiceBarcos serviceBarco,
            IServicePuertos servicePuerto,
            IServiceItinerarios serviceItinerarios,
            IServiceFechasCruceros serviceFechaCruceros,
            IServiceHabitaciones serviceHabitaciones)
        {
            _serviceCrucero = serviceCrucero;
            _serviceBarco = serviceBarco;
            _servicePuerto = servicePuerto;
            _serviceItinerarios = serviceItinerarios;
            _serviceFechaCruceros = serviceFechaCruceros;

        }

        public async Task<ActionResult> Index()
        {
            var collection = await _serviceCrucero.ListAsync();
            return View(collection);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> IndexAdmin()
        {
            var collection = await _serviceCrucero.ListAsync();
            return View(collection);
        }

        public async Task<ActionResult> Details(int id)
        {
            var crucero = await _serviceCrucero.FindByIdAsync(id);
            if (crucero == null)
            {
                return NotFound();
            }
            return View(crucero);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Barcos = await _serviceBarco.ListAsync();
            ViewBag.Puertos = await _servicePuerto.ListAsync();
            return View(new CrucerosDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(CrucerosDTO dto, IFormFile? imageFile)
        {
            var errores = new Dictionary<string, List<string>>();

            MemoryStream target = new MemoryStream();

            // Cuando es Insert Image viene en null porque se pasa diferente
            if (dto.Imagen == null)
            {
                if (imageFile != null)
                {
                    imageFile.OpenReadStream().CopyTo(target);

                    dto.Imagen = target.ToArray();
                    ModelState.Remove("Imagen");
                }
                else
                {
                    var rutaImagenDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "nophoto.jpg");

                    if (System.IO.File.Exists(rutaImagenDefault))
                    {
                        byte[] imagenDefault = System.IO.File.ReadAllBytes(rutaImagenDefault);
                        dto.Imagen = imagenDefault;
                        ModelState.Remove("Imagen");
                    }

                }
            }

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                ModelState.AddModelError("Nombre", "El nombre del crucero es obligatorio.");

            if (dto.BarcoId <= 0)
                ModelState.AddModelError("BarcoId", "Debe seleccionar un barco válido.");

            if (dto.Dias < 2)
                ModelState.AddModelError("Dias", "La duración del crucero debe ser mayor o igual a 2.");

            if (dto.Itinerarios == null || dto.Itinerarios.Count < 2)
                ModelState.AddModelError("Itinerarios", "Debe agregar al menos dos puertos en el itinerario.");

            if (dto.FechasCruceros == null || dto.FechasCruceros.Count == 0)
            {
                ModelState.AddModelError("FechasCruceros", "Debe agregar al menos una fecha de inicio.");
            }
            else
            {
                foreach (var fecha in dto.FechasCruceros)
                {
                    // Validación para cada FechaInicio individual
                    if (fecha.FechaInicio < DateOnly.FromDateTime(DateTime.Today))
                    {
                        ModelState.AddModelError("FechasCruceros", "La fecha de inicio debe ser mayor o igual al día de hoy.");
                    }
                }
            }

            // Ignorar validaciones de navegación (Barco, Crucero, Puerto)
            ModelState.Remove("Barco");
            if (dto.Itinerarios != null)
            {
                for (int i = 0; i < dto.Itinerarios.Count; i++)
                {
                    ModelState.Remove($"Itinerarios[{i}].Crucero");
                    ModelState.Remove($"Itinerarios[{i}].Puerto");
                }
            }
            if (dto.FechasCruceros != null)
            {
                for (int i = 0; i < dto.FechasCruceros.Count; i++)
                {
                    ModelState.Remove($"FechasCruceros[{i}].Crucero");
                }
            }

            if (dto.FechasCruceros != null)
            {
                for (int i = 0; i < dto.FechasCruceros.Count; i++)
                {
                    ModelState.Remove($"FechasCruceros[{i}].Crucero");

                    if (dto.FechasCruceros[i].PreciosHabitaciones != null)
                    {
                        for (int j = 0; j < dto.FechasCruceros[i].PreciosHabitaciones.Count; j++)
                        {
                            ModelState.Remove($"FechasCruceros[{i}].PreciosHabitaciones[{j}].Habitacion");
                            ModelState.Remove($"FechasCruceros[{i}].PreciosHabitaciones[{j}].FechaCrucero");
                        }
                    }
                }
            }


            if (!ModelState.IsValid)
            {
                TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                .SelectMany(x => x.Errors)
                                                .Select(x => x.ErrorMessage));
                return View(dto);
            }

            try
            {

                // Crear el crucero y obtener su ID
                int cruceroId = await _serviceCrucero.AddAsync(dto);

                TempData["Mensaje"] = "Crucero creado con éxito";


                return RedirectToAction(nameof(IndexAdmin));
            }
            catch (Exception ex)
            {
                errores.Add("General", new List<string> { "Error al crear el crucero: " + ex.Message });
                return Json(new { success = false, errors = errores });
            }
        }



    }
}
