using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Config;

public class AppConfig
{
    public SmtpConfiguration SmtpConfiguration { get; set; } = default!;
    public Crypto Crypto { get; set; } = default!;
    public BccrSettings BccrSettings { get; set; } = default!;
}
public class SmtpConfiguration
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Server { get; set; } = default!;
    public int PortNumber { get; set; }
    public string FromName { get; set; } = default!;
    public bool EnableSsl { get; set; }
}
public class Crypto { 
    public string Secret { get; set; } = default!;
}

public class BccrSettings
{
    public string Token { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string NombreApp { get; set; } = default!;
}

