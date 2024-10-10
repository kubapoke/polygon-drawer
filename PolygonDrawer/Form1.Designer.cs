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
            bresenhamRadioButton = new RadioButton();
            libraryRadioButton = new RadioButton();
            newPolyButton = new Button();
            vertexContextMenuStrip = new ContextMenuStrip(components);
            deleteVertexToolStripMenuItem = new ToolStripMenuItem();
            lineContextMenuStrip = new ContextMenuStrip(components);
            addPointToolStripMenuItem = new ToolStripMenuItem();
            forceVerticalToolStripMenuItem = new ToolStripMenuItem();
            forceHorizontalToolStripMenuItem = new ToolStripMenuItem();
            forceLengthToolStripMenuItem = new ToolStripMenuItem();
            setBezierCurveToolStripMenuItem = new ToolStripMenuItem();
            removeBoundsToolStripMenuItem = new ToolStripMenuItem();
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
            mainSplitContainer.Panel2.BackColor = SystemColors.ControlLight;
            mainSplitContainer.Panel2.Controls.Add(bresenhamRadioButton);
            mainSplitContainer.Panel2.Controls.Add(libraryRadioButton);
            mainSplitContainer.Panel2.Controls.Add(newPolyButton);
            mainSplitContainer.Size = new Size(800, 450);
            mainSplitContainer.SplitterDistance = 600;
            mainSplitContainer.TabIndex = 0;
            // 
            // bresenhamRadioButton
            // 
            bresenhamRadioButton.Anchor = AnchorStyles.Top;
            bresenhamRadioButton.AutoSize = true;
            bresenhamRadioButton.Checked = true;
            bresenhamRadioButton.Location = new System.Drawing.Point(10, 100);
            bresenhamRadioButton.Name = "bresenhamRadioButton";
            bresenhamRadioButton.Size = new Size(84, 19);
            bresenhamRadioButton.TabIndex = 2;
            bresenhamRadioButton.TabStop = true;
            bresenhamRadioButton.Text = "Bresenham";
            bresenhamRadioButton.UseVisualStyleBackColor = true;
            bresenhamRadioButton.CheckedChanged += bresenhamRadioButton_CheckedChanged;
            // 
            // libraryRadioButton
            // 
            libraryRadioButton.Anchor = AnchorStyles.Top;
            libraryRadioButton.AutoSize = true;
            libraryRadioButton.Location = new System.Drawing.Point(10, 75);
            libraryRadioButton.Name = "libraryRadioButton";
            libraryRadioButton.Size = new Size(61, 19);
            libraryRadioButton.TabIndex = 1;
            libraryRadioButton.Text = "Library";
            libraryRadioButton.UseVisualStyleBackColor = true;
            libraryRadioButton.CheckedChanged += libraryRadioButton_CheckedChanged;
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
            vertexContextMenuStrip.Size = new Size(138, 26);
            // 
            // deleteVertexToolStripMenuItem
            // 
            deleteVertexToolStripMenuItem.Name = "deleteVertexToolStripMenuItem";
            deleteVertexToolStripMenuItem.Size = new Size(137, 22);
            deleteVertexToolStripMenuItem.Text = "delete point";
            deleteVertexToolStripMenuItem.Click += deleteVertexToolStripMenuItem_Click;
            // 
            // lineContextMenuStrip
            // 
            lineContextMenuStrip.Items.AddRange(new ToolStripItem[] { addPointToolStripMenuItem, forceVerticalToolStripMenuItem, forceHorizontalToolStripMenuItem, forceLengthToolStripMenuItem, setBezierCurveToolStripMenuItem, removeBoundsToolStripMenuItem });
            lineContextMenuStrip.Name = "lineContextMenuStrip";
            lineContextMenuStrip.Size = new Size(181, 158);
            // 
            // addPointToolStripMenuItem
            // 
            addPointToolStripMenuItem.Name = "addPointToolStripMenuItem";
            addPointToolStripMenuItem.Size = new Size(180, 22);
            addPointToolStripMenuItem.Text = "add point";
            addPointToolStripMenuItem.Click += addPointToolStripMenuItem_Click;
            // 
            // forceVerticalToolStripMenuItem
            // 
            forceVerticalToolStripMenuItem.Name = "forceVerticalToolStripMenuItem";
            forceVerticalToolStripMenuItem.Size = new Size(180, 22);
            forceVerticalToolStripMenuItem.Text = "force vertical";
            forceVerticalToolStripMenuItem.Click += forceVerticalToolStripMenuItem_Click;
            // 
            // forceHorizontalToolStripMenuItem
            // 
            forceHorizontalToolStripMenuItem.Name = "forceHorizontalToolStripMenuItem";
            forceHorizontalToolStripMenuItem.Size = new Size(180, 22);
            forceHorizontalToolStripMenuItem.Text = "force horizontal";
            forceHorizontalToolStripMenuItem.Click += forceHorizontalToolStripMenuItem_Click;
            // 
            // forceLengthToolStripMenuItem
            // 
            forceLengthToolStripMenuItem.Name = "forceLengthToolStripMenuItem";
            forceLengthToolStripMenuItem.Size = new Size(180, 22);
            forceLengthToolStripMenuItem.Text = "force length";
            forceLengthToolStripMenuItem.Click += forceLengthToolStripMenuItem_Click;
            // 
            // setBezierCurveToolStripMenuItem
            // 
            setBezierCurveToolStripMenuItem.Name = "setBezierCurveToolStripMenuItem";
            setBezierCurveToolStripMenuItem.Size = new Size(180, 22);
            setBezierCurveToolStripMenuItem.Text = "set Bezier Curve";
            setBezierCurveToolStripMenuItem.Click += setBezierCurveToolStripMenuItem_Click;
            // 
            // removeBoundsToolStripMenuItem
            // 
            removeBoundsToolStripMenuItem.Name = "removeBoundsToolStripMenuItem";
            removeBoundsToolStripMenuItem.Size = new Size(180, 22);
            removeBoundsToolStripMenuItem.Text = "remove bounds";
            removeBoundsToolStripMenuItem.Click += removeBoundsToolStripMenuItem_Click;
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
            mainSplitContainer.Panel2.PerformLayout();
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
        private RadioButton bresenhamRadioButton;
        private RadioButton libraryRadioButton;
        private ToolStripMenuItem forceVerticalToolStripMenuItem;
        private ToolStripMenuItem forceHorizontalToolStripMenuItem;
        private ToolStripMenuItem forceLengthToolStripMenuItem;
        private ToolStripMenuItem setBezierCurveToolStripMenuItem;
        private ToolStripMenuItem removeBoundsToolStripMenuItem;
    }
}
