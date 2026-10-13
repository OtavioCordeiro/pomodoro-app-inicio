using System.ComponentModel.DataAnnotations;
using PomodoroApi.Domain;

namespace PomodoroApi.Contratos;

public class ConfiguracaoRequest
{
    [Range(1, 120)]
    public int FocoMinutos { get; set; }

    [Range(1, 120)]
    public int PausaCurtaMinutos { get; set; }

    [Range(1, 120)]
    public int PausaLongaMinutos { get; set; }

    [Range(1, 12)]
    public int FocosAteLonga { get; set; }

    public bool IniciarAutomaticamente { get; set; }

    public Configuracao ParaDominio()
    {
        return new Configuracao(
            FocoMinutos,
            PausaCurtaMinutos,
            PausaLongaMinutos,
            FocosAteLonga,
            IniciarAutomaticamente);
    }
}
