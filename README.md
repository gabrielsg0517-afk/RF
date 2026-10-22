# Mentora — Mentoria de Estudos

Interface desktop (WPF · .NET 8 · C#) para o mentor acompanhar o desempenho do aluno nos
simulados e registrar mentorias.

## Layout

```
┌──────────────────────────────── Mentora (faixa #003333) ───────────────────────────────┐
│ Data │ Mentor │ Matrícula 🔍 │ Nome do aluno │ Sala                                     │
├──────────────────────────────────────────────────────┬─────────────────────────────────┤
│ Desempenho nos simulados                              │ Registro da mentoria            │
│ Simulado │ Linguagens │ Humanas │ Natureza │ Matem. │  │  Análise Técnica                │
│          │ Média Fáceis Médias Difíceis (×4)      │  │  Indicação de Estudo            │
│          │ … Redação │ Média Final │ Classif.      │  │  Observações                    │
│                                                      │  ☐ Mentoria obrigatória  [💾]   │
│                                                      ├─────────────────────────────────┤
│                                                      │ Histórico de mentorias          │
│                                                      │ Data/Hora │ Mentor │ Indicação  │
│                                                      │ (obrigatórias em amarelo pastel)│
└──────────────────────────────────────────────────────┴─────────────────────────────────┘
```

- **Planilha de simulados**: uma linha por simulado (`Simulado Ciclo 05 - 1ª Série - 2026`), com
  um grupo de colunas colorido por área (Média `000,00`, Fáceis, Médias, Difíceis), Redação,
  Média Final e Classificação — todos os valores vêm da base de dados.
- **Registro da mentoria**: três caixas de texto livres, a caixa *Mentoria obrigatória* e o botão
  com disquete. Ao salvar, o relatório recebe data/hora e um **código de verificação** (SHA-256
  do conteúdo) e entra no topo do histórico.
- **Histórico de mentorias**: do mais recente para o mais antigo; mentorias obrigatórias em
  amarelo pastel. Passe o mouse sobre uma linha para ver o relatório completo e se o código
  confere com o conteúdo.

## Identidade visual

Tema claro acadêmico: fundo off-white `#F6F4EF`, cor primária verde-petróleo `#003333`, acento
âmbar `#E0A526`, cartões arredondados. Cada área do conhecimento tem sua cor (Linguagens azul,
Humanas terracota, Natureza verde, Matemática roxo). As cores ficam em
`src/MentoriaEstudos/Themes/Colors.xaml`; os estilos dos controles em `Themes/Controls.xaml`.

## Como executar (Windows)

```
dotnet run --project src/MentoriaEstudos
```

Ou abra `MentoriaEstudos.sln` no Visual Studio 2022. Dados de exemplo: matrículas **20481** e
**20517** (digite e pressione Enter).

## Base de dados

A interface depende apenas de `Data/IMentoriaRepository.cs`. Hoje a implementação é
`JsonMentoriaRepository`, que lê `Data/Sample/dados-exemplo.json` e grava as mentorias em
`%AppData%\MentoriaEstudos\mentorias.json`. Para usar a base real, crie outra implementação da
interface (SQL Server, SQLite, API…) e troque-a em `MainWindow.xaml.cs`.
