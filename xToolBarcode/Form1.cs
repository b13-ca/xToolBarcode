using System.Text.Json;

namespace xToolBarcode;

public partial class Form1 : Form {
    private List<UnprintedCard> _unprintedCards = [];

    public Form1() {
        InitializeComponent();
        cmdProcessList.Click += this.CmdProcessList_Click;
    }

    private void CmdProcessList_Click(object? sender, EventArgs e) {
        var importPath = Path.Combine(AppContext.BaseDirectory, "import", "unprinted-cards.json");
        var json = File.ReadAllText(importPath);
        _unprintedCards = JsonSerializer.Deserialize<List<UnprintedCard>>(json) ?? [];

        var exportDirectory = Path.Combine(AppContext.BaseDirectory, "export");
        Directory.CreateDirectory(exportDirectory);

        foreach (var card in _unprintedCards) {
            var filePath = Path.Combine(exportDirectory, $"{card.CardNumber}.png");
            Code128Png.Save(card.CardNumber.ToString(), filePath);
        }
    }
}

internal sealed class UnprintedCard {
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public string CVC { get; set; } = "";
}
