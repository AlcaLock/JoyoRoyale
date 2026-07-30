using Crucero.Application.DTOs;
using Crucero.Application.Services.Implementations;
using Crucero.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;


namespace JoyoRoyale.Web.Controllers
{
    public class BarcoController : Controller
    {
        private readonly IServiceBarcos _serviceBarco;
        private readonly IServiceHabitaciones _serviceHabitaciones;
        private readonly IServiceBarcoHabitaciones _serviceBarcoHabitaciones;

        public BarcoController(IServiceBarcos serviceBarco, IServiceHabitaciones serviceHabitaciones, IServiceBarcoHabitaciones serviceBarcoHabitaciones)
        {
            _serviceBarco = serviceBarco;
            _serviceHabitaciones = serviceHabitaciones;
            _serviceBarcoHabitaciones = serviceBarcoHabitaciones;
        }

        // GET: BarcoController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceBarco.ListAsync();
            return View(collection);
        }

        // GET: BarcoController/IndexAdmin
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> IndexAdmin()
        {
            var collection = await _serviceBarco.ListAsync();
            return View(collection);
        }

        // GET: BarcoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var barco = await _serviceBarco.FindByIdAsync(id);
            if (barco == null)
            {
                return NotFound();
            }
            return View(barco);
        }
        // GET: BarcoController/Create
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
            return View();
        }

        // POST: BarcoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create(BarcosDTO dto, List<int> HabitacionIds, List<int> Cantidades, IFormFile imageFile)
        {
            try
            {
                // Validar que el DTO no sea nulo
                if (dto == null)
                {
                    ModelState.AddModelError("", "Los datos del barco son inválidos.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que se haya seleccionado al menos una habitación
                if (HabitacionIds == null || Cantidades == null || !HabitacionIds.Any() || !Cantidades.Any())
                {
                    ModelState.AddModelError("", "Debes seleccionar al menos un tipo de habitación.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las listas de habitaciones y cantidades coincidan en longitud
                if (HabitacionIds.Count != Cantidades.Count)
                {
                    ModelState.AddModelError("", "Las habitaciones y cantidades deben coincidir.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las habitaciones no estén repetidas
                if (HabitacionIds.Distinct().Count() != HabitacionIds.Count)
                {
                    ModelState.AddModelError("", "No puedes agregar el mismo tipo de habitación más de una vez.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las cantidades sean mayores a 0
                if (Cantidades.Any(c => c <= 0))
                {
                    ModelState.AddModelError("", "Las cantidades deben ser mayores a 0.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar imagen si se subió
                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile.FileName).ToLower();
                    var formatosPermitidos = new List<string> { ".jpg", ".jpeg", ".png" };
                    if (!formatosPermitidos.Contains(extension))
                    {
                        ModelState.AddModelError("Imagen", "El formato de la imagen debe ser JPG o PNG.");
                        ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                        return View(dto);
                    }

                    // Validar tamaño de imagen (ejemplo: máximo 2MB)
                    const int maxSize = 2 * 1024 * 1024; // 2MB
                    if (imageFile.Length > maxSize)
                    {
                        ModelState.AddModelError("Imagen", "El tamaño de la imagen no debe superar los 2MB.");
                        ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                        return View(dto);
                    }

                    // Procesar la imagen
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.OpenReadStream().CopyToAsync(memoryStream);
                        dto.Imagen = memoryStream.ToArray();
                        ModelState.Remove("Imagen");
                    }
                }

                // Asignar habitaciones al barco
                var habitaciones = HabitacionIds.Zip(Cantidades, (id, cantidad) => (id, cantidad)).ToList();


                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
                    return View(dto);
                }
                // Guardar el barco en la base de datos
                await _serviceBarco.AddAsync(dto, habitaciones);

                TempData["Mensaje"] = "Barco creado con éxito";

                return RedirectToAction(nameof(IndexAdmin));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud. Intenta nuevamente.");
                ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                return View(dto);
            }
        }


        // GET: BarcoController/UpdateBa/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateBa(int id)
        {
            var barco = await _serviceBarco.FindByIdAsync(id);
            if (barco == null)
            {
                return NotFound();
            }

            // Obtener todas las habitaciones disponibles
            var todasHabitaciones = await _serviceHabitaciones.ListAsync();

            // Obtener habitaciones actuales del barco
            var habitacionesBarco = await _serviceBarco.GetHabitacionesPorBarcoIdAsync(id);

            // Crear una lista de habitaciones con información de selección
            ViewBag.Habitaciones = todasHabitaciones.Select(h => new
            {
                h.Id,
                h.Nombre,
                Seleccionada = habitacionesBarco.Any(hb => hb.HabitacionId == h.Id),
                HabitacionId = h.Id,
                BarcoId = barco.Id,
                CantidadDisponible = habitacionesBarco.FirstOrDefault(bh => bh.HabitacionId == h.Id)?.CantidadDisponible ?? 0
            }).ToList();

            return View("UpdateBa", barco);
        }


        // POST: BarcoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, BarcosDTO dto, List<int> HabitacionIds, List<int> Cantidades, IFormFile nuevaImagen)
        {
            try
            {
                // Remover la validación del campo nuevaImagen para evitar errores si no se sube una nueva imagen
                ModelState.Remove("nuevaImagen");

                // Validar que el DTO no sea nulo
                if (dto == null)
                {
                    ModelState.AddModelError("", "Los datos del barco son inválidos.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que se haya seleccionado al menos una habitación
                if (HabitacionIds == null || Cantidades == null || !HabitacionIds.Any() || !Cantidades.Any())
                {
                    ModelState.AddModelError("", "Debes seleccionar al menos un tipo de habitación.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las listas de habitaciones y cantidades coincidan en longitud
                if (HabitacionIds.Count != Cantidades.Count)
                {
                    ModelState.AddModelError("", "Las habitaciones y cantidades deben coincidir.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las habitaciones no estén repetidas
                if (HabitacionIds.Distinct().Count() != HabitacionIds.Count)
                {
                    ModelState.AddModelError("", "No puedes agregar el mismo tipo de habitación más de una vez.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Validar que las cantidades sean mayores a 0
                if (Cantidades.Any(c => c <= 0))
                {
                    ModelState.AddModelError("", "Las cantidades deben ser mayores a 0.");
                    ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                    return View(dto);
                }

                // Obtener el barco existente desde la base de datos
                var barcoExistente = await _serviceBarco.FindByIdAsync(id);

                if (barcoExistente == null)
                {
                    return NotFound();
                }

                // Si el usuario sube una nueva imagen, convertirla a byte[]
                if (nuevaImagen != null && nuevaImagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await nuevaImagen.CopyToAsync(memoryStream);
                        dto.Imagen = memoryStream.ToArray(); // Guardar la imagen en byte[]
                    }
                    ModelState.Remove("Imagen"); // Eliminar la validación de la imagen
                }
                else
                {
                    // Mantener la imagen existente si no se subió una nueva
                    dto.Imagen = barcoExistente.Imagen;
                    ModelState.Remove("Imagen"); // Eliminar la validación de la imagen
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        errores = ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList()
                    });
                }

                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
                    return View(dto);
                }

                var habitaciones = HabitacionIds.Zip(Cantidades, (h, c) => (h, c)).ToList();
                await _serviceBarco.UpdateAsync(id, dto, habitaciones);

                // Mensaje de confirmación con SweetAlert
                TempData["Mensaje"] = "Barco actualizado con éxito";

                return RedirectToAction(nameof(IndexAdmin));
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ModelState.AddModelError("Nombre", ex.Message);
                ViewBag.Habitaciones = await _serviceHabitaciones.ListAsync();
                return View("UpdateBa", dto);
            }
        }



        // GET: BarcoController/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var barco = await _serviceBarco.FindByIdAsync(id);
            if (barco == null)
            {
                return NotFound();
            }
            return View(barco);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}