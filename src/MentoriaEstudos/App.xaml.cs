using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace MentoriaEstudos;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Datas e números no padrão brasileiro (ex.: 529,56 e 22/10/2026) em toda a interface.
        var cultura = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;
        Thread.CurrentThread.CurrentCulture = cultura;
        Thread.CurrentThread.CurrentUICulture = cultura;
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(cultura.IetfLanguageTag)));

        base.OnStartup(e);
    }
}
