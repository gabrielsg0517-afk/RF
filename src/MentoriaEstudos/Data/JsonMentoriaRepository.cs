using System.IO;
using System.Text.Json;
using MentoriaEstudos.Models;

namespace MentoriaEstudos.Data;

/// <summary>
/// Implementação provisória baseada em arquivos JSON: lê alunos e simulados de um arquivo
/// de base e grava as mentorias em %AppData%\MentoriaEstudos\mentorias.json.
/// </summary>
public sealed class JsonMentoriaRepository : IMentoriaRepository
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly BaseDados _base;
    private readonly string _arquivoMentorias;
    private readonly List<RegistroMentoria> _mentorias;

    public JsonMentoriaRepository(string arquivoBase, string arquivoMentorias)
    {
        _base = File.Exists(arquivoBase) ? Ler<BaseDados>(arquivoBase) ?? new() : new();
        _arquivoMentorias = arquivoMentorias;
        _mentorias = File.Exists(arquivoMentorias)
            ? Ler<List<RegistroMentoria>>(arquivoMentorias) ?? new()
            : new List<RegistroMentoria>(_base.Mentorias);
    }

    public static JsonMentoriaRepository CriarPadrao()
    {
        var arquivoBase = Path.Combine(AppContext.BaseDirectory, "Data", "Sample", "dados-exemplo.json");
        var pasta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MentoriaEstudos");
        return new JsonMentoriaRepository(arquivoBase, Path.Combine(pasta, "mentorias.json"));
    }

    public Aluno? ObterAluno(string matricula) => Buscar(matricula);

    public IReadOnlyList<ResultadoSimulado> ObterSimulados(string matricula) =>
        Buscar(matricula)?.Simulados.OrderBy(s => s.Ano).ThenBy(s => s.Ciclo).ToList()
        ?? new List<ResultadoSimulado>();

    public IReadOnlyList<RegistroMentoria> ObterHistorico(string matricula) =>
        _mentorias.Where(m => m.MatriculaAluno == matricula.Trim())
                  .OrderByDescending(m => m.DataHora)
                  .ToList();

    public void SalvarMentoria(RegistroMentoria registro)
    {
        _mentorias.Add(registro);
        Directory.CreateDirectory(Path.GetDirectoryName(_arquivoMentorias)!);
        File.WriteAllText(_arquivoMentorias, JsonSerializer.Serialize(_mentorias, Opcoes));
    }

    private AlunoComSimulados? Buscar(string matricula) =>
        _base.Alunos.FirstOrDefault(a => a.Matricula == matricula.Trim());

    private static T? Ler<T>(string arquivo) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(arquivo), Opcoes);

    private sealed class BaseDados
    {
        public List<AlunoComSimulados> Alunos { get; set; } = new();
        public List<RegistroMentoria> Mentorias { get; set; } = new();
    }

    private sealed class AlunoComSimulados : Aluno
    {
        public List<ResultadoSimulado> Simulados { get; set; } = new();
    }
}
