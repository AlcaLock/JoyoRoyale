using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace JoyoRoyale.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IServiceUsuarios _serviceUsuarios;
        private readonly IServicePaises _servicePaises;  // Agregar el servicio para obtener los países
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IServiceUsuarios serviceUsuarios, IServicePaises servicePaises, ILogger<UsuariosController> logger)
        {
            _serviceUsuarios = serviceUsuarios;
            _servicePaises = servicePaises;  // Inicializar el servicio de países
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var paises = await _servicePaises.GetAllPaisesAsync();  // Obtener la lista de países
            var model = new UsuariosDTO
            {
                Paises = paises  // Asignar la lista de países al modelo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuariosDTO dto)
        {
            // Obtener la lista de países en caso de que se necesite en caso de error de validación
            // Verificar que los países estén presentes
            if (dto.Paises == null || !dto.Paises.Any())
            {
                var paises = await _servicePaises.GetAllPaisesAsync();
                dto.Paises = paises;
            }

            // Validación del correo electrónico
            if (string.IsNullOrEmpty(dto.Correo) || !Regex.IsMatch(dto.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError("Correo", "El correo electrónico no es válido.");
            }

            // Validación del teléfono (formato 1234 5678)
            if (string.IsNullOrEmpty(dto.Telefono) || !Regex.IsMatch(dto.Telefono, @"^\d{4} \d{4}$"))
            {
                ModelState.AddModelError("Telefono", "El teléfono debe tener el formato 1234 5678.");
            }

            // Validación del país
            if (string.IsNullOrEmpty(dto.Pais))
            {
                ModelState.AddModelError("Pais", "Por favor, seleccione un país.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido en registro:");
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        _logger.LogWarning($"Campo: {error.Key}, Error: {subError.ErrorMessage}");
                    }
                }

                // Regresar a la vista con los países cargados
                return View(dto);
            }

            try
            {
                _logger.LogInformation("Intentando registrar usuario...");
                await _serviceUsuarios.AddAsync(dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje("Registro", "Usuario creado exitosamente", SweetAlertMessageType.success);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario.");
                ViewBag.Message = "Ocurrió un error al registrar el usuario.";
                return View(dto);
            }
        }
    }
}
