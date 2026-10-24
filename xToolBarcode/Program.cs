namespace xToolBarcode;

internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var exportDirectory = Path.Combine(AppContext.BaseDirectory, "export");
        if (!Directory.Exists(exportDirectory)) {
            Directory.CreateDirectory(exportDirectory);
        }
        Application.Run(new Form1());
    }
}