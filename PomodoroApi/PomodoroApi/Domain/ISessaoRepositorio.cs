namespace PomodoroApi.Domain;

public interface ISessaoRepositorio
{
    Sessao? Obter(Guid id);
    void Salvar(Sessao sessao);
}
