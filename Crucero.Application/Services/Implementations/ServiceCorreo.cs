using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Crucero.Application.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JoyoRoyale.Infraestructure.Repository.Interfaces;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceCorreo : IServiceCorreo
    {
        private readonly IOptions<AppConfig> _options;
        private readonly ILogger<ServiceCorreo> _logger;

        public ServiceCorreo(
            IOptions<AppConfig> options,
            ILogger<ServiceCorreo> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task<bool> SendEmail(string to, string subject, string body, byte[] attachmentBytes, string attachmentFileName)
        {
            try
            {
                var config = _options.Value.SmtpConfiguration;

                if (string.IsNullOrEmpty(config.Server) || config.PortNumber <= 0)
                {
                    _logger.LogError($"SMTP Server o Port inválido en {MethodBase.GetCurrentMethod()?.DeclaringType?.FullName}");
                    return false;
                }

                if (string.IsNullOrEmpty(config.UserName) || string.IsNullOrEmpty(config.FromName))
                {
                    _logger.LogError($"SMTP UserName o FromName faltantes en {MethodBase.GetCurrentMethod()?.DeclaringType?.FullName}");
                    return false;
                }

                var mailMessage = new MailMessage(
                    new MailAddress(config.UserName, config.FromName),
                    new MailAddress(to))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                if (attachmentBytes != null && attachmentBytes.Length > 0 && !string.IsNullOrEmpty(attachmentFileName))
                {
                    var stream = new MemoryStream(attachmentBytes);
                    var attachment = new Attachment(stream, attachmentFileName, "application/pdf");
                    mailMessage.Attachments.Add(attachment);
                }

                using var smtpClient = new SmtpClient(config.Server, config.PortNumber)
                {
                    Credentials = new NetworkCredential(config.UserName, config.Password),
                    EnableSsl = config.EnableSsl,
                    UseDefaultCredentials = false
                };

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar el correo.");

                Console.WriteLine("ERROR AL ENVIAR CORREO: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);

                return false;
            }
        }
    }
}
