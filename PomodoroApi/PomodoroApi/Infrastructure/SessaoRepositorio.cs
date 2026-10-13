using System.Collections.Concurrent;
using PomodoroApi.Domain;

namespace PomodoroApi.Infrastructure;

public class SessaoRepositorio : ISessaoRepositorio
{
    private readonly ConcurrentDictionary<Guid, Sessao> _sessoes = new();

    public Sessao? Obter(Guid id)
    {
        return _sessoes.GetValueOrDefault(id);
    }

    public void Salvar(Sessao sessao)
    {
        _sessoes[sessao.Id] = sessao;
    }
}
