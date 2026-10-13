using PomodoroApi.Domain;

namespace PomodoroApi.Contratos;

public class SessaoResponse
{
    public Guid Id { get; set; }
    public TipoSessao Tipo { get; set; }
    public int SegundosRestantes { get; set; }
    public bool Pausada { get; set; }
    public bool Finalizada { get; set; }

    public SessaoResponse(Sessao sessao, DateTimeOffset agora)
    {
        Id = sessao.Id;
        Tipo = sessao.Tipo;
        SegundosRestantes = (int)sessao.TempoRestante(agora).TotalSeconds;
        Pausada = sessao.Pausada;
        Finalizada = sessao.Finalizada;
    }
}
