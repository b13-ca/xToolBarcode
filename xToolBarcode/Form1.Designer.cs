namespace xToolBarcode;

partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.cmdProcessList = new Button();
        this.SuspendLayout();
        // 
        // cmdProcessList
        // 
        this.cmdProcessList.Location = new Point(12, 12);
        this.cmdProcessList.Name = "cmdProcessList";
        this.cmdProcessList.Size = new Size(119, 53);
        this.cmdProcessList.TabIndex = 0;
        this.cmdProcessList.Text = "button1";
        this.cmdProcessList.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        this.AutoScaleMode = AutoScaleMode.None;
        this.ClientSize = new Size(800, 450);
        this.Controls.Add(this.cmdProcessList);
        this.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        this.Name = "Form1";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Form1";
        this.ResumeLayout(false);
    }

    #endregion

    private Button cmdProcessList;
}
