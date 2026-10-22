namespace MentoriaEstudos.Models;

public class Aluno
{
    public string Matricula { get; set; } = "";
    public string Nome { get; set; } = "";

    /// <summary>Turma do aluno, ex.: "3º B".</summary>
    public string Sala { get; set; } = "";
}
