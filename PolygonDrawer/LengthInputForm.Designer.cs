namespace PolygonDrawer
{
    partial class LengthInputForm
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
            inputLabel = new Label();
            lengthTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // inputLabel
            // 
            inputLabel.AutoSize = true;
            inputLabel.Location = new System.Drawing.Point(36, 21);
            inputLabel.Name = "inputLabel";
            inputLabel.Size = new Size(73, 15);
            inputLabel.TabIndex = 0;
            inputLabel.Text = "Edge length:";
            // 
            // lengthTextBox
            // 
            lengthTextBox.Location = new System.Drawing.Point(124, 18);
            lengthTextBox.Name = "lengthTextBox";
            lengthTextBox.Size = new Size(135, 23);
            lengthTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            okButton.Location = new System.Drawing.Point(57, 55);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(159, 55);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // LengthInputForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(288, 90);
            ControlBox = false;
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(lengthTextBox);
            Controls.Add(inputLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "LengthInputForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Input edge length";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label inputLabel;
        private TextBox lengthTextBox;
        private Button okButton;
        private Button cancelButton;
    }
}