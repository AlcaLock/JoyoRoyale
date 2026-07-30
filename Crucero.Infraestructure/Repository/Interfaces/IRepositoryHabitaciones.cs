
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryHabitaciones
    {
        /// <summary>
        /// Obtiene todas las habitaciones disponibles.
        /// </summary>
        /// <returns>Lista de habitaciones.</returns>
        Task<ICollection<Habitaciones>> ListAsync();

        /// <summary>
        /// Busca una habitación por su ID.
        /// </summary>
        /// <param name="id">ID de la habitación.</param>
        /// <returns>Objeto Habitación o null si no existe.</returns>
        Task<Habitaciones> FindByIdAsync(int id);

        /// <summary>
        /// Agrega una nueva habitación a la base de datos.
        /// </summary>
        /// <param name="entity">Entidad de la habitación.</param>
        /// <returns>ID de la habitación creada.</returns>
        Task<int> AddAsync(Habitaciones entity);

        /// <summary>
        /// Actualiza los datos de una habitación existente.
        /// </summary>
        /// <param name="entity">Entidad de la habitación con datos actualizados.</param>
        Task UpdateAsync(Habitaciones entity);

        /// <summary>
        /// Elimina una habitación de la base de datos.
        /// </summary>
        /// <param name="id">ID de la habitación a eliminar.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Busca habitaciones por nombre.
        /// </summary>
        /// <param name="nombre">Nombre o parte del nombre de la habitación.</param>
        /// <returns>Lista de habitaciones que coincidan con el nombre.</returns>
        Task<ICollection<Habitaciones>> FindByNameAsync(string nombre);

        /// <summary>
        /// Obtiene todas las habitaciones asociadas a un barco específico.
        /// </summary>
        /// <param name="barcoId">ID del barco.</param>
        /// <returns>Lista de habitaciones pertenecientes al barco.</returns>
        Task<ICollection<Habitaciones>> GetHabitacionesByBarco(int barcoId);


        Task<List<BarcoHabitaciones>> GetHabitacionesDisponiblesAsync(int cruceroId, int fechaCruceroId);
    }
}

