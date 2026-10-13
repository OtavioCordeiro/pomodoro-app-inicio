using PomodoroApi.Domain;

namespace PomodoroApi.Contratos;

public class ConfiguracaoResponse
{
    public int FocoMinutos { get; set; }
    public int PausaCurtaMinutos { get; set; }
    public int PausaLongaMinutos { get; set; }
    public int FocosAteLonga { get; set; }
    public bool IniciarAutomaticamente { get; set; }

    public ConfiguracaoResponse(Configuracao configuracao)
    {
        FocoMinutos = configuracao.FocoMinutos;
        PausaCurtaMinutos = configuracao.PausaCurtaMinutos;
        PausaLongaMinutos = configuracao.PausaLongaMinutos;
        FocosAteLonga = configuracao.FocosAteLonga;
        IniciarAutomaticamente = configuracao.IniciarAutomaticamente;
    }
}
