namespace Steam_Authenticator.Forms
{
    partial class Inventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inventory));
            inventoryPages = new TabControl();
            defaultApp = new TabPage();
            defaultInventory = new Panel();
            SteamId = new Label();
            UserName = new Label();
            inventoryPages.SuspendLayout();
            defaultApp.SuspendLayout();
            SuspendLayout();
            // 
            // inventoryPages
            // 
            inventoryPages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            inventoryPages.Controls.Add(defaultApp);
            inventoryPages.Location = new Point(12, 38);
            inventoryPages.Name = "inventoryPages";
            inventoryPages.SelectedIndex = 0;
            inventoryPages.Size = new Size(776, 411);
            inventoryPages.TabIndex = 0;
            inventoryPages.SelectedIndexChanged += InventoryPages_SelectedIndexChanged;
            // 
            // defaultApp
            // 
            defaultApp.Controls.Add(defaultInventory);
            defaultApp.Location = new Point(4, 26);
            defaultApp.Name = "defaultApp";
            defaultApp.Padding = new Padding(3);
            defaultApp.Size = new Size(768, 381);
            defaultApp.TabIndex = 0;
            defaultApp.Text = "Steam";
            defaultApp.UseVisualStyleBackColor = true;
            // 
            // defaultInventory
            // 
            defaultInventory.AutoScroll = true;
            defaultInventory.Dock = DockStyle.Fill;
            defaultInventory.Location = new Point(3, 3);
            defaultInventory.Name = "defaultInventory";
            defaultInventory.Size = new Size(762, 375);
            defaultInventory.TabIndex = 0;
            // 
            // SteamId
            // 
            SteamId.AutoEllipsis = true;
            SteamId.ForeColor = Color.FromArgb(0, 0, 238);
            SteamId.Location = new Point(16, 9);
            SteamId.Name = "SteamId";
            SteamId.Size = new Size(133, 18);
            SteamId.TabIndex = 20;
            SteamId.Text = "---";
            SteamId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UserName
            // 
            UserName.AutoEllipsis = true;
            UserName.ForeColor = Color.Green;
            UserName.Location = new Point(160, 9);
            UserName.Name = "UserName";
            UserName.Size = new Size(300, 18);
            UserName.TabIndex = 19;
            UserName.Text = "---";
            UserName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Inventory
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(SteamId);
            Controls.Add(UserName);
            Controls.Add(inventoryPages);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Inventory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Steam库存";
            Load += Inventory_Load;
            inventoryPages.ResumeLayout(false);
            defaultApp.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl inventoryPages;
        private TabPage defaultApp;
        private Label SteamId;
        private Label UserName;
        private Panel defaultInventory;
    }
}