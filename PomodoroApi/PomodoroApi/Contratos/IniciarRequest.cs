using System.ComponentModel.DataAnnotations;
using PomodoroApi.Domain;

namespace PomodoroApi.Contratos;

public class IniciarRequest
{
    [EnumDataType(typeof(TipoSessao))]
    public TipoSessao Tipo { get; set; }
}
