using Microsoft.AspNetCore.Mvc;
using PomodoroApi.Contratos;
using PomodoroApi.Services;

namespace PomodoroApi.Controllers;

[ApiController]
[Route("sessoes")]
public class SessoesController : ControllerBase
{
    private readonly SessaoService _sessaoService;

    public SessoesController(SessaoService sessaoService)
    {
        _sessaoService = sessaoService;
    }

    [HttpPost("iniciar")]
    public ActionResult<SessaoResponse> Iniciar(IniciarRequest pedido)
    {
        var sessao = _sessaoService.Iniciar(pedido.Tipo);

        return Created($"/sessoes/{sessao.Id}", sessao);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<SessaoResponse> Obter(Guid id)
    {
        var sessao = _sessaoService.Obter(id);
        if (sessao is null)
            return NotFound();

        return sessao;
    }

    [HttpPost("{id:guid}/pausar")]
    public ActionResult<SessaoResponse> Pausar(Guid id)
    {
        var sessao = _sessaoService.Pausar(id);
        if (sessao is null)
            return NotFound();

        return sessao;
    }

    [HttpPost("{id:guid}/retomar")]
    public ActionResult<SessaoResponse> Retomar(Guid id)
    {
        var sessao = _sessaoService.Retomar(id);
        if (sessao is null)
            return NotFound();

        return sessao;
    }

    [HttpPost("{id:guid}/finalizar")]
    public ActionResult<SessaoResponse> Finalizar(Guid id)
    {
        var sessao = _sessaoService.Finalizar(id);
        if (sessao is null)
            return NotFound();

        return sessao;
    }
}
