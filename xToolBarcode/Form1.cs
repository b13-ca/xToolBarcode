using System.Text.Json;

namespace xToolBarcode;

public partial class Form1 : Form {
    private List<UnprintedCard> _unprintedCards = [];

    public Form1() {
        InitializeComponent();
        cmdProcessList.Click += this.CmdProcessList_Click;
    }

    private void CmdProcessList_Click(object? sender, EventArgs e) {
        var path = Path.Combine(AppContext.BaseDirectory, "import", "unprinted-cards.json");
        var json = File.ReadAllText(path);
        _unprintedCards = JsonSerializer.Deserialize<List<UnprintedCard>>(json) ?? [];
    }
}

internal sealed class UnprintedCard {
    public int Id { get; set; }
    public long CardNumber { get; set; }
    public string CVC { get; set; } = "";
}
