namespace PolygonDrawer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainSplitContainer = new SplitContainer();
            newPolyButton = new Button();
            vertexContextMenuStrip = new ContextMenuStrip(components);
            deleteVertexToolStripMenuItem = new ToolStripMenuItem();
            lineContextMenuStrip = new ContextMenuStrip(components);
            addPointToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            vertexContextMenuStrip.SuspendLayout();
            lineContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.IsSplitterFixed = true;
            mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.MouseClick += mainSplitContainer_Panel1_MouseClick;
            mainSplitContainer.Panel1.MouseDown += mainSplitContainer_Panel1_MouseDown;
            mainSplitContainer.Panel1.MouseMove += mainSplitContainer_Panel1_MouseMove;
            mainSplitContainer.Panel1.MouseUp += mainSplitContainer_Panel1_MouseUp;
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(newPolyButton);
            mainSplitContainer.Size = new Size(800, 450);
            mainSplitContainer.SplitterDistance = 600;
            mainSplitContainer.TabIndex = 0;
            // 
            // newPolyButton
            // 
            newPolyButton.Anchor = AnchorStyles.Top;
            newPolyButton.Location = new System.Drawing.Point(10, 12);
            newPolyButton.Name = "newPolyButton";
            newPolyButton.Size = new Size(174, 57);
            newPolyButton.TabIndex = 0;
            newPolyButton.Text = "New Polygon";
            newPolyButton.UseVisualStyleBackColor = true;
            newPolyButton.Click += newPolyButton_Click;
            // 
            // vertexContextMenuStrip
            // 
            vertexContextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteVertexToolStripMenuItem });
            vertexContextMenuStrip.Name = "contextMenuStrip1";
            vertexContextMenuStrip.Size = new Size(107, 26);
            // 
            // deleteVertexToolStripMenuItem
            // 
            deleteVertexToolStripMenuItem.Name = "deleteVertexToolStripMenuItem";
            deleteVertexToolStripMenuItem.Size = new Size(106, 22);
            deleteVertexToolStripMenuItem.Text = "delete";
            deleteVertexToolStripMenuItem.Click += deleteVertexToolStripMenuItem_Click;
            // 
            // lineContextMenuStrip
            // 
            lineContextMenuStrip.Items.AddRange(new ToolStripItem[] { addPointToolStripMenuItem });
            lineContextMenuStrip.Name = "lineContextMenuStrip";
            lineContextMenuStrip.Size = new Size(181, 48);
            // 
            // addPointToolStripMenuItem
            // 
            addPointToolStripMenuItem.Name = "addPointToolStripMenuItem";
            addPointToolStripMenuItem.Size = new Size(180, 22);
            addPointToolStripMenuItem.Text = "add point";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(mainSplitContainer);
            MinimumSize = new Size(816, 489);
            Name = "Form1";
            Text = "Form1";
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            vertexContextMenuStrip.ResumeLayout(false);
            lineContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer mainSplitContainer;
        private Button newPolyButton;
        private ContextMenuStrip vertexContextMenuStrip;
        private ToolStripMenuItem deleteVertexToolStripMenuItem;
        private ContextMenuStrip lineContextMenuStrip;
        private ToolStripMenuItem addPointToolStripMenuItem;
    }
}
