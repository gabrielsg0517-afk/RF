using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MentoriaEstudos.Data;
using MentoriaEstudos.Models;

namespace MentoriaEstudos.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMentoriaRepository _repositorio;

    public MainViewModel(IMentoriaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    // Identificação
    [ObservableProperty]
    private DateTime? _data = DateTime.Today;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarMentoriaCommand))]
    private string _mentor = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuscarAlunoCommand))]
    private string _matricula = "";

    [ObservableProperty]
    private string _nomeAluno = "";

    [ObservableProperty]
    private string _sala = "";

    /// <summary>Matrícula do aluno cujos dados estão exibidos nas planilhas.</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarMentoriaCommand))]
    private string? _matriculaCarregada;

    // Registro da mentoria
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarMentoriaCommand))]
    private string _analiseTecnica = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarMentoriaCommand))]
    private string _indicacaoEstudo = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SalvarMentoriaCommand))]
    private string _observacoes = "";

    [ObservableProperty]
    private bool _mentoriaObrigatoria;

    [ObservableProperty]
    private string _mensagemStatus = "";

    public ObservableCollection<ResultadoSimulado> Simulados { get; } = new();

    /// <summary>Histórico de mentorias, do mais recente para o mais antigo.</summary>
    public ObservableCollection<RegistroMentoria> Historico { get; } = new();

    private bool PodeBuscar() => !string.IsNullOrWhiteSpace(Matricula);

    [RelayCommand(CanExecute = nameof(PodeBuscar))]
    private void BuscarAluno()
    {
        var matricula = Matricula.Trim();
        var aluno = _repositorio.ObterAluno(matricula);

        Simulados.Clear();
        Historico.Clear();

        if (aluno is null)
        {
            MatriculaCarregada = null;
            NomeAluno = "";
            Sala = "";
            MensagemStatus = $"Matrícula {matricula} não encontrada.";
            return;
        }

        MatriculaCarregada = aluno.Matricula;
        NomeAluno = aluno.Nome;
        Sala = aluno.Sala;

        foreach (var simulado in _repositorio.ObterSimulados(matricula))
            Simulados.Add(simulado);
        foreach (var registro in _repositorio.ObterHistorico(matricula))
            Historico.Add(registro);

        MensagemStatus = "";
    }

    private bool PodeSalvar() =>
        MatriculaCarregada is not null
        && !string.IsNullOrWhiteSpace(Mentor)
        && !(string.IsNullOrWhiteSpace(AnaliseTecnica)
             && string.IsNullOrWhiteSpace(IndicacaoEstudo)
             && string.IsNullOrWhiteSpace(Observacoes));

    [RelayCommand(CanExecute = nameof(PodeSalvar))]
    private void SalvarMentoria()
    {
        var agora = DateTime.Now;
        var dia = Data ?? agora.Date;
        var registro = new RegistroMentoria
        {
            // Precisão de segundos para que o código de verificação seja reprodutível.
            DataHora = new DateTime(dia.Year, dia.Month, dia.Day, agora.Hour, agora.Minute, agora.Second),
            Mentor = Mentor.Trim(),
            MatriculaAluno = MatriculaCarregada!,
            AnaliseTecnica = AnaliseTecnica.Trim(),
            IndicacaoEstudo = IndicacaoEstudo.Trim(),
            Observacoes = Observacoes.Trim(),
            Obrigatoria = MentoriaObrigatoria,
        };
        registro.Codigo = registro.GerarCodigo();

        try
        {
            _repositorio.SalvarMentoria(registro);
        }
        catch (Exception ex)
        {
            MensagemStatus = $"Não foi possível salvar: {ex.Message}";
            return;
        }

        var posicao = 0;
        while (posicao < Historico.Count && Historico[posicao].DataHora > registro.DataHora)
            posicao++;
        Historico.Insert(posicao, registro);

        AnaliseTecnica = "";
        IndicacaoEstudo = "";
        Observacoes = "";
        MentoriaObrigatoria = false;
        MensagemStatus = $"Mentoria salva às {registro.DataHora:HH:mm} · código {registro.Codigo}";
    }
}
