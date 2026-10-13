using PomodoroApi.Domain;

namespace PomodoroApi.Infrastructure;

public class ConfiguracaoRepositorio : IConfiguracaoRepositorio
{
    private Configuracao _atual = Configuracao.Padrao();

    public Configuracao Obter()
    {
        return _atual;
    }

    public void Salvar(Configuracao configuracao)
    {
        _atual = configuracao;
    }
}
