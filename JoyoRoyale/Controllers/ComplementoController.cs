using Crucero.Application.DTOs;
using Crucero.Application.Services.Implementations;
using Crucero.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyoRoyale.Web.Controllers
{
    public class ComplementoController : Controller
    {
        private readonly IServiceComplementos _serviceComplemento;

        public ComplementoController(IServiceComplementos serviceComplemento)
        {
            _serviceComplemento = serviceComplemento;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> IndexAdmin()
        {
            var collection = await _serviceComplemento.ListAsync();
            return View(collection);
        }

        [Authorize(Roles = "Administrador")]
        // GET: ComplementoController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComplementoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(ComplementosDTO dto)
        {
            try
            {
                // Validar que el DTO no sea nulo
                if (dto == null)
                {
                    ModelState.AddModelError("", "Los datos del complemento son inválidos.");
                }

                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre del complemento es obligatorio.");
                }

                // Validar que la descripción no exceda un límite si es necesario
                if (!string.IsNullOrEmpty(dto.Descripcion) && dto.Descripcion.Length > 500)
                {
                    ModelState.AddModelError("Descripcion", "La descripción no puede exceder los 500 caracteres.");
                }

                // Validar el precio
                if (dto.Precio <= 0)
                {
                    ModelState.AddModelError("Precio", "El precio debe ser mayor que cero.");
                }

                // Validar el tipo de aplicación
                if (string.IsNullOrWhiteSpace(dto.TipoAplicacion))
                {
                    ModelState.AddModelError("TipoAplicacion", "El tipo de aplicación es obligatorio.");
                }

                // Si hay errores, devolver la vista con los errores
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                        .SelectMany(x => x.Errors)
                                                        .Select(x => x.ErrorMessage));
                    return View(dto);
                }

                // Guardar el complemento en la base de datos
                await _serviceComplemento.AddAsync(dto);

                // Mensaje de confirmación con SweetAlert
                TempData["Mensaje"] = "Complemento creado con éxito";

              
                return RedirectToAction("IndexAdmin");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud. Intenta nuevamente.");
                return View(dto);
            }
        }


        // GET: ComplementoController/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateCo(int id)
        {

            var complemento = await _serviceComplemento.FindByIdAsync(id);


            if (complemento == null)
            {
                return NotFound();
            }

            // Pasar el DTO a la vista para editar
            return View("UpdateCo", complemento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(ComplementosDTO dto)
        {
            try
            {
                // Obtener el complemento existente desde la base de datos
                var complementoExistente = await _serviceComplemento.FindByIdAsync(dto.Id);

                if (complementoExistente == null)
                {
                    return NotFound();
                }

                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre del complemento es obligatorio.");
                }

                // Validar que la descripción no exceda un límite si es necesario
                if (!string.IsNullOrEmpty(dto.Descripcion) && dto.Descripcion.Length > 500)
                {
                    ModelState.AddModelError("Descripcion", "La descripción no puede exceder los 500 caracteres.");
                }

                // Validar el precio
                if (dto.Precio <= 0)
                {
                    ModelState.AddModelError("Precio", "El precio debe ser mayor que cero.");
                }

                // Validar el tipo de aplicación
                if (string.IsNullOrWhiteSpace(dto.TipoAplicacion))
                {
                    ModelState.AddModelError("TipoAplicacion", "El tipo de aplicación es obligatorio.");
                }

                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessages"] = string.Join("|", ModelState.Values
                                                  .SelectMany(x => x.Errors)
                                                  .Select(x => x.ErrorMessage));
                    return View(dto);
                }

                // Llamar al servicio para actualizar el complemento
                await _serviceComplemento.UpdateAsync(dto.Id, dto);

                // Mensaje de confirmación con SweetAlert
                TempData["Mensaje"] = "Complemento actualizado con éxito";

                return RedirectToAction("IndexAdmin");
            }
            catch (Exception)
            {
                TempData["ErrorMessages"] = "Ocurrió un error al procesar la solicitud. Intenta nuevamente.";
                return View(dto);
            }
        }
    }
}
