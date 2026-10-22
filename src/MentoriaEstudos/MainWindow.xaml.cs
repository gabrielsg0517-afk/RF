using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using MentoriaEstudos.Data;
using MentoriaEstudos.Models;
using MentoriaEstudos.ViewModels;

namespace MentoriaEstudos;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(JsonMentoriaRepository.CriarPadrao());
        MontarColunasSimulados();
    }

    /// <summary>
    /// Monta a planilha de simulados: título, um grupo de 4 colunas por área do conhecimento
    /// (Média · Fáceis · Médias · Difíceis), Redação, Média Final e Classificação.
    /// </summary>
    private void MontarColunasSimulados()
    {
        var colunas = GradeSimulados.Columns;

        colunas.Add(new DataGridTextColumn
        {
            Header = Cabecalho(null, null, "Simulado"),
            Binding = new Binding(nameof(ResultadoSimulado.Titulo)),
            Width = 250,
            HeaderStyle = EstiloCabecalho(null, alinharEsquerda: true),
            ElementStyle = EstiloCelula(FontWeights.SemiBold, null, alinharEsquerda: true),
        });

        AdicionarArea(nameof(ResultadoSimulado.Linguagens), "Linguagens", "AreaLinguagens");
        AdicionarArea(nameof(ResultadoSimulado.Humanas), "Humanas", "AreaHumanas");
        AdicionarArea(nameof(ResultadoSimulado.Natureza), "Natureza", "AreaNatureza");
        AdicionarArea(nameof(ResultadoSimulado.Matematica), "Matemática", "AreaMatematica");

        var primaria = (Brush)FindResource("Primaria");
        colunas.Add(Coluna(Cabecalho(null, null, "Redação"), nameof(ResultadoSimulado.Redacao), "{0:0.##}", 72,
            EstiloCabecalho(null), EstiloCelula(FontWeights.Normal, null)));
        colunas.Add(Coluna(Cabecalho(null, null, "Média Final"), nameof(ResultadoSimulado.MediaFinal), "{0:000.00}", 88,
            EstiloCabecalho((Brush)FindResource("Acento")), EstiloCelula(FontWeights.Bold, primaria)));
        colunas.Add(Coluna(Cabecalho(null, null, "Classif."), nameof(ResultadoSimulado.Classificacao), "{0}º", 70,
            EstiloCabecalho(null), EstiloCelula(FontWeights.SemiBold, primaria)));
    }

    private void AdicionarArea(string propriedade, string nome, string chaveCor)
    {
        var cor = (Brush)FindResource(chaveCor);
        var fundo = (Brush)FindResource(chaveCor + "Suave");
        var cabecalho = EstiloCabecalho(cor, fundo);
        var normal = EstiloCelula(FontWeights.Normal, null);

        var colunas = GradeSimulados.Columns;
        colunas.Add(Coluna(Cabecalho(nome, cor, "Média"), $"{propriedade}.{nameof(DesempenhoArea.Media)}", "{0:000.00}", 68,
            cabecalho, EstiloCelula(FontWeights.SemiBold, cor)));
        colunas.Add(Coluna(Cabecalho(null, null, "Fáceis"), $"{propriedade}.{nameof(DesempenhoArea.Faceis)}", null, 56, cabecalho, normal));
        colunas.Add(Coluna(Cabecalho(null, null, "Médias"), $"{propriedade}.{nameof(DesempenhoArea.Medias)}", null, 58, cabecalho, normal));
        colunas.Add(Coluna(Cabecalho(null, null, "Difíceis"), $"{propriedade}.{nameof(DesempenhoArea.Dificeis)}", null, 60, cabecalho, normal));
    }

    private static DataGridTextColumn Coluna(object cabecalho, string caminho, string? formato, double largura,
                                             Style estiloCabecalho, Style estiloCelula) =>
        new()
        {
            Header = cabecalho,
            Binding = new Binding(caminho) { StringFormat = formato },
            Width = largura,
            HeaderStyle = estiloCabecalho,
            ElementStyle = estiloCelula,
        };

    /// <summary>Cabeçalho em duas linhas: nome da área (só na primeira coluna do grupo) e rótulo.</summary>
    private static StackPanel Cabecalho(string? area, Brush? corArea, string rotulo)
    {
        var painel = new StackPanel();
        painel.Children.Add(new TextBlock
        {
            Text = area ?? " ",
            Foreground = corArea ?? Brushes.Transparent,
            FontSize = 11,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 2),
        });
        painel.Children.Add(new TextBlock { Text = rotulo });
        return painel;
    }

    private Style EstiloCabecalho(Brush? faixa, Brush? fundo = null, bool alinharEsquerda = false)
    {
        var estilo = new Style(typeof(DataGridColumnHeader), (Style)FindResource(typeof(DataGridColumnHeader)));
        if (faixa is not null)
            estilo.Setters.Add(new Setter(TagProperty, faixa));
        if (fundo is not null)
            estilo.Setters.Add(new Setter(BackgroundProperty, fundo));
        if (alinharEsquerda)
            estilo.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Left));
        return estilo;
    }

    private static Style EstiloCelula(FontWeight peso, Brush? cor, bool alinharEsquerda = false)
    {
        var estilo = new Style(typeof(TextBlock));
        estilo.Setters.Add(new Setter(TextBlock.FontWeightProperty, peso));
        estilo.Setters.Add(new Setter(HorizontalAlignmentProperty,
            alinharEsquerda ? HorizontalAlignment.Left : HorizontalAlignment.Center));
        if (cor is not null)
            estilo.Setters.Add(new Setter(TextBlock.ForegroundProperty, cor));
        return estilo;
    }
}
