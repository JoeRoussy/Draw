namespace SimpleSketchPad
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ShapeSelectorCb = new System.Windows.Forms.ComboBox();
            this.ShapeSelectorLbl = new System.Windows.Forms.Label();
            this.ColorSelectorBtn = new System.Windows.Forms.Button();
            this.ToggleModeBtn = new System.Windows.Forms.Button();
            this.ModeLbl = new System.Windows.Forms.Label();
            this.ShowHelpCb = new System.Windows.Forms.CheckBox();
            this.DrawingHelpLbl = new System.Windows.Forms.Label();
            this.ShapesLv = new System.Windows.Forms.ListView();
            this.ShapesLvHiddenCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ShapesLvTypeCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ShapesLvColourCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ShapesLvPositionCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ShapeLvLbl = new System.Windows.Forms.Label();
            this.PasteBtn = new System.Windows.Forms.Button();
            this.CutBtn = new System.Windows.Forms.Button();
            this.CopyBtn = new System.Windows.Forms.Button();
            this.Panel = new SimpleSketchPad.SketchPadPanel();
            this.SuspendLayout();
            // 
            // ShapeSelectorCb
            // 
            this.ShapeSelectorCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ShapeSelectorCb.FormattingEnabled = true;
            this.ShapeSelectorCb.Items.AddRange(new object[] {
            "Rectangle",
            "Ellipse",
            "Line",
            "Polygon",
            "Free-Hand"});
            this.ShapeSelectorCb.Location = new System.Drawing.Point(13, 28);
            this.ShapeSelectorCb.Name = "ShapeSelectorCb";
            this.ShapeSelectorCb.Size = new System.Drawing.Size(121, 24);
            this.ShapeSelectorCb.TabIndex = 1;
            // 
            // ShapeSelectorLbl
            // 
            this.ShapeSelectorLbl.AutoSize = true;
            this.ShapeSelectorLbl.Location = new System.Drawing.Point(22, 9);
            this.ShapeSelectorLbl.Name = "ShapeSelectorLbl";
            this.ShapeSelectorLbl.Size = new System.Drawing.Size(101, 17);
            this.ShapeSelectorLbl.TabIndex = 2;
            this.ShapeSelectorLbl.Text = "Choose Shape";
            // 
            // ColorSelectorBtn
            // 
            this.ColorSelectorBtn.Location = new System.Drawing.Point(184, 12);
            this.ColorSelectorBtn.Name = "ColorSelectorBtn";
            this.ColorSelectorBtn.Size = new System.Drawing.Size(112, 40);
            this.ColorSelectorBtn.TabIndex = 3;
            this.ColorSelectorBtn.Text = "Choose Colour";
            this.ColorSelectorBtn.UseVisualStyleBackColor = true;
            // 
            // ToggleModeBtn
            // 
            this.ToggleModeBtn.Location = new System.Drawing.Point(1778, 12);
            this.ToggleModeBtn.Name = "ToggleModeBtn";
            this.ToggleModeBtn.Size = new System.Drawing.Size(112, 40);
            this.ToggleModeBtn.TabIndex = 4;
            this.ToggleModeBtn.Text = "Toggle Mode";
            this.ToggleModeBtn.UseVisualStyleBackColor = true;
            // 
            // ModeLbl
            // 
            this.ModeLbl.AutoSize = true;
            this.ModeLbl.Location = new System.Drawing.Point(1615, 24);
            this.ModeLbl.Name = "ModeLbl";
            this.ModeLbl.Size = new System.Drawing.Size(0, 17);
            this.ModeLbl.TabIndex = 5;
            this.ModeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShowHelpCb
            // 
            this.ShowHelpCb.AutoSize = true;
            this.ShowHelpCb.Location = new System.Drawing.Point(319, 23);
            this.ShowHelpCb.Name = "ShowHelpCb";
            this.ShowHelpCb.Size = new System.Drawing.Size(97, 21);
            this.ShowHelpCb.TabIndex = 6;
            this.ShowHelpCb.Text = "Show Help";
            this.ShowHelpCb.UseVisualStyleBackColor = true;
            // 
            // DrawingHelpLbl
            // 
            this.DrawingHelpLbl.AutoSize = true;
            this.DrawingHelpLbl.Location = new System.Drawing.Point(433, 23);
            this.DrawingHelpLbl.Name = "DrawingHelpLbl";
            this.DrawingHelpLbl.Size = new System.Drawing.Size(0, 17);
            this.DrawingHelpLbl.TabIndex = 7;
            // 
            // ShapesLv
            // 
            this.ShapesLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ShapesLvHiddenCh,
            this.ShapesLvTypeCh,
            this.ShapesLvColourCh,
            this.ShapesLvPositionCh});
            this.ShapesLv.FullRowSelect = true;
            this.ShapesLv.GridLines = true;
            this.ShapesLv.Location = new System.Drawing.Point(1310, 94);
            this.ShapesLv.Name = "ShapesLv";
            this.ShapesLv.Size = new System.Drawing.Size(580, 927);
            this.ShapesLv.TabIndex = 8;
            this.ShapesLv.UseCompatibleStateImageBehavior = false;
            this.ShapesLv.View = System.Windows.Forms.View.Details;
            // 
            // ShapesLvHiddenCh
            // 
            this.ShapesLvHiddenCh.Text = "Type";
            this.ShapesLvHiddenCh.Width = 0;
            // 
            // ShapesLvTypeCh
            // 
            this.ShapesLvTypeCh.Text = "Type";
            this.ShapesLvTypeCh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ShapesLvTypeCh.Width = 193;
            // 
            // ShapesLvColourCh
            // 
            this.ShapesLvColourCh.Text = "Colour";
            this.ShapesLvColourCh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ShapesLvColourCh.Width = 193;
            // 
            // ShapesLvPositionCh
            // 
            this.ShapesLvPositionCh.Text = "Position";
            this.ShapesLvPositionCh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ShapesLvPositionCh.Width = 192;
            // 
            // ShapeLvLbl
            // 
            this.ShapeLvLbl.AutoSize = true;
            this.ShapeLvLbl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShapeLvLbl.Location = new System.Drawing.Point(1528, 64);
            this.ShapeLvLbl.Name = "ShapeLvLbl";
            this.ShapeLvLbl.Size = new System.Drawing.Size(137, 23);
            this.ShapeLvLbl.TabIndex = 9;
            this.ShapeLvLbl.Text = "Select Shapes";
            this.ShapeLvLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PasteBtn
            // 
            this.PasteBtn.Location = new System.Drawing.Point(1181, 12);
            this.PasteBtn.Name = "PasteBtn";
            this.PasteBtn.Size = new System.Drawing.Size(80, 40);
            this.PasteBtn.TabIndex = 12;
            this.PasteBtn.Text = "Paste";
            this.PasteBtn.UseVisualStyleBackColor = true;
            // 
            // CutBtn
            // 
            this.CutBtn.Location = new System.Drawing.Point(1009, 12);
            this.CutBtn.Name = "CutBtn";
            this.CutBtn.Size = new System.Drawing.Size(80, 40);
            this.CutBtn.TabIndex = 13;
            this.CutBtn.Text = "Cut";
            this.CutBtn.UseVisualStyleBackColor = true;
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(1095, 12);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(80, 40);
            this.CopyBtn.TabIndex = 14;
            this.CopyBtn.Text = "Copy";
            this.CopyBtn.UseVisualStyleBackColor = true;
            // 
            // Panel
            // 
            this.Panel.BackColor = System.Drawing.Color.White;
            this.Panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Panel.Location = new System.Drawing.Point(13, 58);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(1280, 963);
            this.Panel.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.CopyBtn);
            this.Controls.Add(this.CutBtn);
            this.Controls.Add(this.PasteBtn);
            this.Controls.Add(this.ShapeLvLbl);
            this.Controls.Add(this.ShapesLv);
            this.Controls.Add(this.DrawingHelpLbl);
            this.Controls.Add(this.ShowHelpCb);
            this.Controls.Add(this.ModeLbl);
            this.Controls.Add(this.ToggleModeBtn);
            this.Controls.Add(this.ColorSelectorBtn);
            this.Controls.Add(this.ShapeSelectorLbl);
            this.Controls.Add(this.ShapeSelectorCb);
            this.Controls.Add(this.Panel);
            this.KeyPreview = true;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Sketch Pad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SketchPadPanel Panel;
        private System.Windows.Forms.ComboBox ShapeSelectorCb;
        private System.Windows.Forms.Label ShapeSelectorLbl;
        private System.Windows.Forms.Button ColorSelectorBtn;
        private System.Windows.Forms.Button ToggleModeBtn;
        private System.Windows.Forms.Label ModeLbl;
        private System.Windows.Forms.CheckBox ShowHelpCb;
        private System.Windows.Forms.Label DrawingHelpLbl;
        private System.Windows.Forms.ListView ShapesLv;
        private System.Windows.Forms.ColumnHeader ShapesLvHiddenCh;
        private System.Windows.Forms.ColumnHeader ShapesLvTypeCh;
        private System.Windows.Forms.ColumnHeader ShapesLvPositionCh;
        private System.Windows.Forms.Label ShapeLvLbl;
        private System.Windows.Forms.ColumnHeader ShapesLvColourCh;
        private System.Windows.Forms.Button PasteBtn;
        private System.Windows.Forms.Button CutBtn;
        private System.Windows.Forms.Button CopyBtn;
    }
}

