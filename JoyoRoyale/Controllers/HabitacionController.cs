using Crucero.Application.DTOs;
using Crucero.Application.Services.Implementations;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JoyoRoyale.Web.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly IServiceHabitaciones _serviceHabitacion;

        public HabitacionController(IServiceHabitaciones serviceHabitacion)
        {
            _serviceHabitacion = serviceHabitacion;
        }

        // GET: HabitacionController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceHabitacion.ListAsync();
            return View(collection);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> IndexAdmin()
        {
            var collection = await _serviceHabitacion.ListAsync();
            return View(collection);
        }


        // GET: HabitacionController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var habitacion = await _serviceHabitacion.FindByIdAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }
            return View(habitacion);
        }

        // GET: HabitacionController/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HabitacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(HabitacionesDTO dto, IFormFile? imageFile)
        {
            try
            {
                // Validar que el DTO no sea nulo
                if (dto == null)
                {
                    ModelState.AddModelError("", "Los datos de la habitación son inválidos.");
                }

                // Validar si ya existe una habitación con el mismo nombre
                var habitacionesConMismoNombre = await _serviceHabitacion.FindByNameAsync(dto.Nombre);
                if (habitacionesConMismoNombre.Any())
                {
                    ModelState.AddModelError("Nombre", "El nombre ya existe, usa uno diferente.");
                }


                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre de la habitación es obligatorio.");
                }

                // Validar la capacidad mínima y máxima
                if (dto.CapacidadMinima < 1)
                {
                    ModelState.AddModelError("CapacidadMinima", "La capacidad mínima debe ser al menos 1.");
                }

                if (dto.CapacidadMaxima < 1)
                {
                    ModelState.AddModelError("CapacidadMaxima", "La capacidad máxima debe ser al menos 1.");
                }

                if (dto.CapacidadMaxima < dto.CapacidadMinima)
                {
                    ModelState.AddModelError("CapacidadMaxima", "La capacidad máxima no puede ser menor que la capacidad mínima.");
                }

                if (dto.CapacidadMinima > dto.CapacidadMaxima)
                {
                    ModelState.AddModelError("CapacidadMinima", "La capacidad mínima no puede ser mayor que la capacidad máxima.");
                }

                // Validar el tamaño de la habitación
                if (dto.Tamano <= 0)
                {
                    ModelState.AddModelError("Tamano", "El tamaño de la habitación debe ser un valor positivo.");
                }

                // Procesar la imagen solo si se sube una nueva
                if (imageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.OpenReadStream().CopyToAsync(memoryStream);
                        dto.Imagen = memoryStream.ToArray();
                        ModelState.Remove("Imagen"); // Eliminar cualquier error previo de imagen
                    }
                }
                else if (dto.Imagen == null || dto.Imagen.Length == 0)
                {
                    ModelState.AddModelError("Imagen", "Debe proporcionar una imagen de la habitación.");
                }

                // Si hay errores, devolver la vista con los errores
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
                    return View(dto);
                }

                // Guardar la habitación en la base de datos
                await _serviceHabitacion.AddAsync(dto);

                // Mensaje de confirmación con SweetAlert
                TempData["Mensaje"] = "Habitación creada con éxito";

                // Redireccionar a IndexAdmin
                return RedirectToAction("IndexAdmin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud. Intenta nuevamente.");
                return View(dto);
            }
        }





        // GET: HabitacionController/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateHa(int id)
        {
            // Obtener la habitación por su id
            var habitacion = await _serviceHabitacion.FindByIdAsync(id);

            // Si no se encuentra la habitación, redirigir a la página de error o a la lista
            if (habitacion == null)
            {
                return NotFound();
            }

            // Pasar el DTO a la vista para editar
            return View("UpdateHa", habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(HabitacionesDTO dto, IFormFile nuevaImagen)
        {
            try
            {
                // Remover la validación del campo nuevaImagen para evitar errores si no se sube una nueva imagen
                ModelState.Remove("nuevaImagen");

                // Obtener la habitación existente desde la base de datos
                var habitacionExistente = await _serviceHabitacion.FindByIdAsync(dto.Id);

                if (habitacionExistente == null)
                {
                    return NotFound();
                }
                // Validar si ya existe una habitación con el mismo nombre (excluyendo la actual)
                var habitacionesConMismoNombre = await _serviceHabitacion.FindByNameAsync(dto.Nombre);
                if (habitacionesConMismoNombre.Any(h => h.Id != dto.Id)) // Ignorar la habitación actual
                {
                    ModelState.AddModelError("Nombre", "El nombre ya existe, usa uno diferente.");
                }

                // Si el usuario sube una nueva imagen, convertirla a byte[]
                if (nuevaImagen != null && nuevaImagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await nuevaImagen.CopyToAsync(memoryStream);
                        dto.Imagen = memoryStream.ToArray(); // Guardar la imagen en byte[]
                    }
                    ModelState.Remove("Imagen"); // Eliminar el error de validación de la imagen
                }
                else
                {
                    // Mantener la imagen existente si no se subió una nueva
                    dto.Imagen = habitacionExistente.Imagen;
                    ModelState.Remove("Imagen"); // Eliminar el error de validación de la imagen
                }

                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
                    return View("UpdateHa", dto);
                }



                // Llamar al servicio para actualizar la habitación
                await _serviceHabitacion.UpdateAsync(dto.Id, dto);
                // Mensaje de confirmación con SweetAlert
                TempData["Mensaje"] = "Habitación actualizada con éxito";

                return RedirectToAction("IndexAdmin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Nombre", ex.Message);
                return View("UpdateHa", dto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetByBarco(int barcoId)
        {
            var habitaciones = await _serviceHabitacion.GetHabitacionesByBarco(barcoId);

            if (habitaciones == null || !habitaciones.Any())
            {
                return NotFound("No se encontraron habitaciones para este barco.");
            }

            return Json(habitaciones);
        }



    }
}
