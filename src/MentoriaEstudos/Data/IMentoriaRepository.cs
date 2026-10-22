using MentoriaEstudos.Models;

namespace MentoriaEstudos.Data;

/// <summary>
/// Fonte de dados do aplicativo. Para ligar a base de dados real (SQL Server, SQLite, API…),
/// basta criar outra implementação desta interface e passá-la ao <c>MainViewModel</c>.
/// </summary>
public interface IMentoriaRepository
{
    Aluno? ObterAluno(string matricula);

    /// <summary>Simulados do aluno, em ordem de aplicação.</summary>
    IReadOnlyList<ResultadoSimulado> ObterSimulados(string matricula);

    /// <summary>Mentorias do aluno, da mais recente para a mais antiga.</summary>
    IReadOnlyList<RegistroMentoria> ObterHistorico(string matricula);

    void SalvarMentoria(RegistroMentoria registro);
}
