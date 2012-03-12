namespace TstatMgmtGUI
{
    partial class FindAvailableNetworks
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
            this.lbAvailableTstats = new System.Windows.Forms.ListBox();
            this.btnJoinNetwork = new System.Windows.Forms.Button();
            this.lblFountTstat = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbAvailableTstats
            // 
            this.lbAvailableTstats.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAvailableTstats.FormattingEnabled = true;
            this.lbAvailableTstats.ItemHeight = 20;
            this.lbAvailableTstats.Location = new System.Drawing.Point(271, 204);
            this.lbAvailableTstats.Name = "lbAvailableTstats";
            this.lbAvailableTstats.Size = new System.Drawing.Size(259, 104);
            this.lbAvailableTstats.TabIndex = 2;
            // 
            // btnJoinNetwork
            // 
            this.btnJoinNetwork.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJoinNetwork.Location = new System.Drawing.Point(390, 333);
            this.btnJoinNetwork.Name = "btnJoinNetwork";
            this.btnJoinNetwork.Size = new System.Drawing.Size(127, 30);
            this.btnJoinNetwork.TabIndex = 4;
            this.btnJoinNetwork.Text = "Join Network";
            this.btnJoinNetwork.UseVisualStyleBackColor = true;
            this.btnJoinNetwork.Click += new System.EventHandler(this.btnJoinNetwork_Click);
            // 
            // lblFountTstat
            // 
            this.lblFountTstat.AutoSize = true;
            this.lblFountTstat.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFountTstat.Location = new System.Drawing.Point(227, 163);
            this.lblFountTstat.Name = "lblFountTstat";
            this.lblFountTstat.Size = new System.Drawing.Size(346, 20);
            this.lblFountTstat.TabIndex = 5;
            this.lblFountTstat.Text = "Following is the list of thermostats found :";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TstatMgmtGUI.Properties.Resources.transition;
            this.pictureBox1.Location = new System.Drawing.Point(146, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(509, 121);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(284, 333);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FindAvailableNetworks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 474);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblFountTstat);
            this.Controls.Add(this.btnJoinNetwork);
            this.Controls.Add(this.lbAvailableTstats);
            this.Name = "FindAvailableNetworks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Available Thermostats";
            this.Load += new System.EventHandler(this.FindAvailableNetworks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAvailableTstats;
        private System.Windows.Forms.Button btnJoinNetwork;
        private System.Windows.Forms.Label lblFountTstat;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRefresh;
    }
}