using Microsoft.AspNetCore.Mvc;
using PomodoroApi.Contratos;
using PomodoroApi.Domain;

namespace PomodoroApi.Controllers;

[ApiController]
[Route("configuracao")]
public class ConfiguracaoController : ControllerBase
{
    private readonly IConfiguracaoRepositorio _repositorio;

    public ConfiguracaoController(IConfiguracaoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public ConfiguracaoResponse Obter()
    {
        var configuracao = _repositorio.Obter();

        return new ConfiguracaoResponse(configuracao);
    }

    [HttpPut]
    public ConfiguracaoResponse Salvar(ConfiguracaoRequest pedido)
    {
        var configuracao = pedido.ParaDominio();
        _repositorio.Salvar(configuracao);

        return new ConfiguracaoResponse(configuracao);
    }
}
