namespace PomodoroApi.Domain;

public class Configuracao
{
    public int FocoMinutos { get; private set; }
    public int PausaCurtaMinutos { get; private set; }
    public int PausaLongaMinutos { get; private set; }
    public int FocosAteLonga { get; private set; }
    public bool IniciarAutomaticamente { get; private set; }

    public Configuracao(
        int focoMinutos,
        int pausaCurtaMinutos,
        int pausaLongaMinutos,
        int focosAteLonga,
        bool iniciarAutomaticamente)
    {
        FocoMinutos = focoMinutos;
        PausaCurtaMinutos = pausaCurtaMinutos;
        PausaLongaMinutos = pausaLongaMinutos;
        FocosAteLonga = focosAteLonga;
        IniciarAutomaticamente = iniciarAutomaticamente;
    }

    public static Configuracao Padrao()
    {
        return new Configuracao(25, 5, 15, 4, true);
    }

    public int Minutos(TipoSessao tipo)
    {
        if (tipo == TipoSessao.Foco)
            return FocoMinutos;

        if (tipo == TipoSessao.PausaCurta)
            return PausaCurtaMinutos;

        return PausaLongaMinutos;
    }
}
