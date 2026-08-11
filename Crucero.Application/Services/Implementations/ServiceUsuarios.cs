using AutoMapper;
using Crucero.Application.Config;
using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using Crucero.Application.Utils;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceUsuarios : IServiceUsuarios
    {
        private readonly IRepositoryUsuarios _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;

        public ServiceUsuarios(IRepositoryUsuarios repository, IMapper mapper, IOptions<AppConfig> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<string> AddAsync(UsuariosDTO dto)
        {
            // Store passwords using PBKDF2 with per-user salt.
            dto.Contrasena = Cryptography.HashPassword(dto.Contrasena!);
            var objectMapped = _mapper.Map<Usuarios>(dto);

            return await _repository.AddAsync(objectMapped);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<UsuariosDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<UsuariosDTO>(@object);
            return objectMapped;
        }


        public async Task<ICollection<UsuariosDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<UsuariosDTO>>(list);
            // Return Data
            return collection;
        }

        public async Task<UsuariosDTO> LoginAsync(string id, string Contrasenna)
        {
            UsuariosDTO usuarioDTO = null!;

            string secret = _options.Value.Crypto.Secret;

            var @object = await _repository.FindByEmailAsync(id);

            if (@object == null)
            {
                return usuarioDTO;
            }

            bool isValidPassword = Cryptography.VerifyPassword(Contrasenna, @object.Contrasena, secret);

            if (!isValidPassword)
            {
                return usuarioDTO;
            }

            // Seamless migration: legacy encrypted passwords are upgraded after successful login.
            if (!Cryptography.IsPbkdf2Hash(@object.Contrasena))
            {
                @object.Contrasena = Cryptography.HashPassword(Contrasenna);
                await _repository.UpdateAsync();
            }

            if (@object != null)
            {
                usuarioDTO = _mapper.Map<UsuariosDTO>(@object);
            }

            return usuarioDTO;
        }

        public async Task UpdateAsync(int id, UsuariosDTO dto)
        {
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            _mapper.Map(dto, @object!);
            await _repository.UpdateAsync();
        }
    }
}
