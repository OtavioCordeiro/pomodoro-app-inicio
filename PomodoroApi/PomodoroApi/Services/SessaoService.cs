using PomodoroApi.Contratos;
using PomodoroApi.Domain;

namespace PomodoroApi.Services;

public class SessaoService
{
    private readonly ISessaoRepositorio _sessaoRepositorio;
    private readonly IConfiguracaoRepositorio _configuracaoRepositorio;
    private readonly TimeProvider _relogio;

    public SessaoService(
        ISessaoRepositorio sessaoRepositorio,
        IConfiguracaoRepositorio configuracaoRepositorio,
        TimeProvider relogio)
    {
        _sessaoRepositorio = sessaoRepositorio;
        _configuracaoRepositorio = configuracaoRepositorio;
        _relogio = relogio;
    }

    public SessaoResponse Iniciar(TipoSessao tipo)
    {
        var agora = _relogio.GetUtcNow();
        var configuracao = _configuracaoRepositorio.Obter();
        var minutos = configuracao.Minutos(tipo);

        var sessao = Sessao.Iniciar(tipo, TimeSpan.FromMinutes(minutos), agora);
        _sessaoRepositorio.Salvar(sessao);

        return new SessaoResponse(sessao, agora);
    }

    public SessaoResponse? Obter(Guid id)
    {
        var sessao = _sessaoRepositorio.Obter(id);
        if (sessao is null)
            return null;

        var agora = _relogio.GetUtcNow();

        return new SessaoResponse(sessao, agora);
    }

    public SessaoResponse? Pausar(Guid id)
    {
        var sessao = _sessaoRepositorio.Obter(id);
        if (sessao is null)
            return null;

        var agora = _relogio.GetUtcNow();
        sessao.Pausar(agora);
        _sessaoRepositorio.Salvar(sessao);

        return new SessaoResponse(sessao, agora);
    }

    public SessaoResponse? Retomar(Guid id)
    {
        var sessao = _sessaoRepositorio.Obter(id);
        if (sessao is null)
            return null;

        var agora = _relogio.GetUtcNow();
        sessao.Retomar(agora);
        _sessaoRepositorio.Salvar(sessao);

        return new SessaoResponse(sessao, agora);
    }

    public SessaoResponse? Finalizar(Guid id)
    {
        var sessao = _sessaoRepositorio.Obter(id);
        if (sessao is null)
            return null;

        var agora = _relogio.GetUtcNow();
        sessao.Finalizar(agora);
        _sessaoRepositorio.Salvar(sessao);

        return new SessaoResponse(sessao, agora);
    }
}
