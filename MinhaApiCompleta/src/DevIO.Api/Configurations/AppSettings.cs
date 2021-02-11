using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configurations
{
    public class AppSettings
    {
        public string Secret { get; set; } // A chave de criptografia do token

        public int ExpiracaoHoras { get; set; } // quantos horas é válido

        public string Emissor { get; set; } // quem emite (a aplicacao)

        public string ValidoEm { get; set; } // em quais url esse token é válido
    }
}
