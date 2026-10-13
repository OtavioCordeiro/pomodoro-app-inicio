namespace PomodoroApi.Domain;

public class Sessao
{
    public Guid Id { get; private set; }
    public TipoSessao Tipo { get; private set; }
    public DateTimeOffset IniciadaEm { get; private set; }
    public TimeSpan DuracaoPlanejada { get; private set; }
    public DateTimeOffset? PausadaEm { get; private set; }
    public TimeSpan TempoPausado { get; private set; }
    public DateTimeOffset? FinalizadaEm { get; private set; }

    public bool Pausada => PausadaEm is not null;
    public bool Finalizada => FinalizadaEm is not null;

    public static Sessao Iniciar(TipoSessao tipo, TimeSpan duracao, DateTimeOffset agora)
    {
        return new Sessao
        {
            Id = Guid.NewGuid(),
            Tipo = tipo,
            IniciadaEm = agora,
            DuracaoPlanejada = duracao
        };
    }

    public void Pausar(DateTimeOffset agora)
    {
        if (Finalizada || Pausada) return;
        PausadaEm = agora;
    }

    public void Retomar(DateTimeOffset agora)
    {
        if (Finalizada || !Pausada) return;
        TempoPausado += agora - PausadaEm!.Value;
        PausadaEm = null;
    }

    public void Finalizar(DateTimeOffset agora)
    {
        if (Finalizada) return;
        FinalizadaEm = agora;
    }

    public TimeSpan TempoRestante(DateTimeOffset agora)
    {
        var ate = FinalizadaEm ?? PausadaEm ?? agora;
        var passou = ate - IniciadaEm - TempoPausado;
        var restante = DuracaoPlanejada - passou;
        return restante > TimeSpan.Zero ? restante : TimeSpan.Zero;
    }
}
