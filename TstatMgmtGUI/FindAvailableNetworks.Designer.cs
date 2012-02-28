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
            this.SuspendLayout();
            // 
            // lbAvailableTstats
            // 
            this.lbAvailableTstats.FormattingEnabled = true;
            this.lbAvailableTstats.Location = new System.Drawing.Point(12, 66);
            this.lbAvailableTstats.Name = "lbAvailableTstats";
            this.lbAvailableTstats.Size = new System.Drawing.Size(259, 56);
            this.lbAvailableTstats.TabIndex = 2;
            // 
            // btnJoinNetwork
            // 
            this.btnJoinNetwork.Location = new System.Drawing.Point(80, 150);
            this.btnJoinNetwork.Name = "btnJoinNetwork";
            this.btnJoinNetwork.Size = new System.Drawing.Size(100, 23);
            this.btnJoinNetwork.TabIndex = 4;
            this.btnJoinNetwork.Text = "Join Network";
            this.btnJoinNetwork.UseVisualStyleBackColor = true;
            this.btnJoinNetwork.Click += new System.EventHandler(this.btnJoinNetwork_Click);
            // 
            // lblFountTstat
            // 
            this.lblFountTstat.AutoSize = true;
            this.lblFountTstat.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFountTstat.Location = new System.Drawing.Point(12, 26);
            this.lblFountTstat.Name = "lblFountTstat";
            this.lblFountTstat.Size = new System.Drawing.Size(269, 16);
            this.lblFountTstat.TabIndex = 5;
            this.lblFountTstat.Text = "Following is the list of thermostats found ";
            // 
            // FindAvailableNetworks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 204);
            this.Controls.Add(this.lblFountTstat);
            this.Controls.Add(this.btnJoinNetwork);
            this.Controls.Add(this.lbAvailableTstats);
            this.Name = "FindAvailableNetworks";
            this.Text = "FindAvailableNetworks";
            this.Load += new System.EventHandler(this.FindAvailableNetworks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAvailableTstats;
        private System.Windows.Forms.Button btnJoinNetwork;
        private System.Windows.Forms.Label lblFountTstat;
    }
}