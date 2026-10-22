using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace MentoriaEstudos.Models;

/// <summary>Relatório de uma mentoria registrado por um mentor.</summary>
public class RegistroMentoria
{
    /// <summary>Código de verificação derivado do conteúdo do relatório.</summary>
    public string Codigo { get; set; } = "";
    public DateTime DataHora { get; set; }
    public string Mentor { get; set; } = "";
    public string MatriculaAluno { get; set; } = "";
    public string AnaliseTecnica { get; set; } = "";
    public string IndicacaoEstudo { get; set; } = "";
    public string Observacoes { get; set; } = "";
    public bool Obrigatoria { get; set; }

    /// <summary>
    /// Gera o código de verificação: os 10 primeiros dígitos hexadecimais do SHA-256
    /// de todos os campos do relatório. Qualquer alteração posterior invalida o código.
    /// </summary>
    public string GerarCodigo()
    {
        var conteudo = string.Join("|",
            DataHora.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
            Mentor, MatriculaAluno, AnaliseTecnica, IndicacaoEstudo, Observacoes,
            Obrigatoria ? "1" : "0");
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(conteudo));
        return Convert.ToHexString(hash)[..10];
    }

    [JsonIgnore]
    public bool Integro => Codigo == GerarCodigo();

    [JsonIgnore]
    public string Resumo
    {
        get
        {
            var texto = new StringBuilder();
            texto.AppendLine($"{DataHora:dd/MM/yyyy HH:mm} — {Mentor}{(Obrigatoria ? " (obrigatória)" : "")}");
            texto.AppendLine();
            texto.AppendLine("Análise Técnica:");
            texto.AppendLine(Vazio(AnaliseTecnica));
            texto.AppendLine();
            texto.AppendLine("Indicação de Estudo:");
            texto.AppendLine(Vazio(IndicacaoEstudo));
            texto.AppendLine();
            texto.AppendLine("Observações:");
            texto.AppendLine(Vazio(Observacoes));
            texto.AppendLine();
            texto.Append($"Código {Codigo} — {(Integro ? "relatório íntegro" : "relatório alterado!")}");
            return texto.ToString();
        }
    }

    private static string Vazio(string texto) => string.IsNullOrWhiteSpace(texto) ? "—" : texto;
}
