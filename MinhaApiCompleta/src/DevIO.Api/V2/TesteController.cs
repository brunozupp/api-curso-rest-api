using DevIO.Api.Controllers;
using DevIO.Business.Intefaces;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TesteController : MainController
    {

        private readonly ILogger _logger;
        public TesteController(
            INotificador notificador,
            IUser appUser, 
            ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            // O que tem menos importância em nível de impacto
            // É para desenvolvimento
            _logger.LogTrace("Log de Trace");

            // É para desenvolvimento
            _logger.LogDebug("Log de Debug");

            // Daqui para baixo pode ser usado em produção

            // Usado para nada importante, mas se querer registrar algo simples pode usar esse
            _logger.LogInformation("Log de Informação");

            // Situação de erro 404, não é um erro, mas tbm não deveria acontecer
            _logger.LogWarning("Log de Aviso");

            _logger.LogError("Log de Erro");

            // Algo que ameaça a performance/saúde da aplicação
            _logger.LogCritical("Log de Problema Critico");

            return "Sou a V2";
        }

        [HttpGet("elmahio")]
        public string TestandoElmahIO()
        {
            throw new Exception("We're done when I say we're done");
        }

        [HttpGet("divisao_elmahio")]
        public void Divisao()
        {
            try
            {
                var i = 0;

                var result = 42 / i;
            }
            catch (DivideByZeroException e)
            {   
                // O .Ship é um extension method do elmah.io para que possa enviar o erro que
                // aconteceu para o servidor do elmah.io
                e.Ship(HttpContext);
            }
        }
    }
}
