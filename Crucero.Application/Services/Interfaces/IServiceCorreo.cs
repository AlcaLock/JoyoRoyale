using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceCorreo
    {
        Task<bool> SendEmail(string to, string subject, string body, byte[] attachmentBytes, string attachmentFileName);

    }
}
