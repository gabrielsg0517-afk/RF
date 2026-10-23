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

Dados de exemplo: matrículas **20481** e **20517** (digite e pressione Enter).

### Opção A — executável pronto (não precisa instalar nada)

1. Descompacte `Mentora-win-x64.zip` em uma pasta.
2. Dê duplo clique em `MentoriaEstudos.exe` (mantenha a pasta `Data` ao lado dele).
3. Se o Windows SmartScreen avisar, clique em **Mais informações → Executar assim mesmo**.

### Opção B — a partir do código-fonte

O comando `dotnet run` exige o **.NET 8 SDK**. Ter apenas o runtime do .NET instalado causa o
erro *"No .NET SDKs were found"*.

1. Instale o SDK, pelo cmd/PowerShell:
   ```
   winget install Microsoft.DotNet.SDK.8
   ```
   ou baixe em https://dotnet.microsoft.com/download/dotnet/8.0 (coluna **SDK**, Windows x64).
2. Feche e reabra o cmd e confira se aparece uma versão 8.x:
   ```
   dotnet --list-sdks
   ```
3. Entre na pasta do repositório e execute:
   ```
   cd caminho\para\RF
   dotnet run --project src/MentoriaEstudos
   ```

Também é possível abrir `MentoriaEstudos.sln` no Visual Studio 2022, com a carga de trabalho
**Desenvolvimento para desktop com .NET** instalada, e pressionar F5.

Para gerar o executável da Opção A:

```
dotnet publish src/MentoriaEstudos -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish
```

## Base de dados

A interface depende apenas de `Data/IMentoriaRepository.cs`. Hoje a implementação é
`JsonMentoriaRepository`, que lê `Data/Sample/dados-exemplo.json` e grava as mentorias em
`%AppData%\MentoriaEstudos\mentorias.json`. Para usar a base real, crie outra implementação da
interface (SQL Server, SQLite, API…) e troque-a em `MainWindow.xaml.cs`.
