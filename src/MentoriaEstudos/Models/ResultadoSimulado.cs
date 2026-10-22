using System.Text.Json.Serialization;

namespace MentoriaEstudos.Models;

/// <summary>Desempenho do aluno em uma área do conhecimento de um simulado.</summary>
public class DesempenhoArea
{
    public decimal Media { get; set; }
    public int Faceis { get; set; }
    public int Medias { get; set; }
    public int Dificeis { get; set; }
}

/// <summary>Uma linha da planilha de simulados. Todos os valores vêm da base de dados.</summary>
public class ResultadoSimulado
{
    public int Ciclo { get; set; }
    public string Serie { get; set; } = "";
    public int Ano { get; set; }

    public DesempenhoArea Linguagens { get; set; } = new();
    public DesempenhoArea Humanas { get; set; } = new();
    public DesempenhoArea Natureza { get; set; } = new();
    public DesempenhoArea Matematica { get; set; } = new();

    public decimal Redacao { get; set; }

    /// <summary>Fornecida pela base de dados (possui cálculo próprio).</summary>
    public decimal MediaFinal { get; set; }

    /// <summary>Posição do aluno no ranking, fornecida pela base de dados.</summary>
    public int Classificacao { get; set; }

    [JsonIgnore]
    public string Titulo => $"Simulado Ciclo {Ciclo:00} - {Serie} - {Ano}";
}
