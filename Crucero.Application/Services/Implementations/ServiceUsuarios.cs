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
            // Llave secreta
            string secret = _options.Value.Crypto.Secret;
            // Password encriptado
            string passwordEncrypted = Cryptography.Encrypt(dto.Contrasena!, secret);
            // Establecer password DTO
            dto.Contrasena = passwordEncrypted;
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

            // Llave secreta
            string secret = _options.Value.Crypto.Secret;
            // Password encriptado
            string passwordEncrypted = Cryptography.Encrypt(Contrasenna, secret);

            var @object = await _repository.LoginAsync(id, passwordEncrypted);

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
