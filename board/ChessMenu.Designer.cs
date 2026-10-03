namespace ChessGame
{
    partial class ChessMenu
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
            components = new System.ComponentModel.Container();
            btnPass = new Button();
            btnOnline = new Button();
            label1 = new Label();
            btnNewGame = new Button();
            btnJoinGame = new Button();
            TBServerName = new TextBox();
            btnEnter = new Button();
            TimerConnect = new System.Windows.Forms.Timer(components);
            TimerCheckEvents = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // btnPass
            // 
            btnPass.Location = new Point(1270, 730);
            btnPass.Margin = new Padding(4, 2, 4, 2);
            btnPass.Name = "btnPass";
            btnPass.Size = new Size(318, 81);
            btnPass.TabIndex = 0;
            btnPass.Text = "Pass And Play";
            btnPass.UseVisualStyleBackColor = true;
            btnPass.Click += btnPass_Click;
            // 
            // btnOnline
            // 
            btnOnline.Location = new Point(1270, 836);
            btnOnline.Margin = new Padding(4, 2, 4, 2);
            btnOnline.Name = "btnOnline";
            btnOnline.Size = new Size(318, 81);
            btnOnline.TabIndex = 1;
            btnOnline.Text = "Online Multiplayer";
            btnOnline.UseVisualStyleBackColor = true;
            btnOnline.Click += btnOnline_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 50F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(891, 489);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(1154, 177);
            label1.TabIndex = 2;
            label1.Text = "Joel's Chess Game";
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new Point(1270, 730);
            btnNewGame.Margin = new Padding(4, 2, 4, 2);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(318, 81);
            btnNewGame.TabIndex = 3;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // btnJoinGame
            // 
            btnJoinGame.Location = new Point(1270, 838);
            btnJoinGame.Margin = new Padding(4, 2, 4, 2);
            btnJoinGame.Name = "btnJoinGame";
            btnJoinGame.Size = new Size(318, 79);
            btnJoinGame.TabIndex = 4;
            btnJoinGame.Text = "Join Game";
            btnJoinGame.UseVisualStyleBackColor = true;
            btnJoinGame.Click += btnJoinGame_Click;
            // 
            // TBServerName
            // 
            TBServerName.Location = new Point(1268, 858);
            TBServerName.Margin = new Padding(4, 2, 4, 2);
            TBServerName.Name = "TBServerName";
            TBServerName.Size = new Size(320, 39);
            TBServerName.TabIndex = 5;
            // 
            // btnEnter
            // 
            btnEnter.Location = new Point(1605, 853);
            btnEnter.Margin = new Padding(4, 2, 4, 2);
            btnEnter.Name = "btnEnter";
            btnEnter.Size = new Size(150, 47);
            btnEnter.TabIndex = 6;
            btnEnter.Text = "Enter";
            btnEnter.UseVisualStyleBackColor = true;
            btnEnter.Click += btnEnter_Click;
            // 
            // TimerConnect
            // 
            // 
            // ChessMenu
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2639, 1630);
            Controls.Add(btnEnter);
            Controls.Add(TBServerName);
            Controls.Add(btnJoinGame);
            Controls.Add(btnNewGame);
            Controls.Add(label1);
            Controls.Add(btnOnline);
            Controls.Add(btnPass);
            Margin = new Padding(4, 2, 4, 2);
            Name = "ChessMenu";
            Text = "ChessMenu";
            Load += ChessMenu_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPass;
        private Button btnOnline;
        private Label label1;
        private Button btnNewGame;
        private Button btnJoinGame;
        private TextBox TBServerName;
        private Button btnEnter;
        private System.Windows.Forms.Timer TimerConnect;
        private Button btnBackToMenu;
        private System.Windows.Forms.Timer TimerCheckEvents;
    }
}