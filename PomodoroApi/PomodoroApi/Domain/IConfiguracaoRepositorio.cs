namespace PomodoroApi.Domain;

public interface IConfiguracaoRepositorio
{
    Configuracao Obter();
    void Salvar(Configuracao configuracao);
}
