﻿namespace PolygonDrawer
{
    partial class MainWindowForm
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
            captionButton = new Button();
            richTextBox1 = new RichTextBox();
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
            currentLengthToolStripMenuItem = new ToolStripMenuItem();
            setLengthToolStripMenuItem = new ToolStripMenuItem();
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
            mainSplitContainer.FixedPanel = FixedPanel.Panel2;
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
            mainSplitContainer.Panel2.Controls.Add(captionButton);
            mainSplitContainer.Panel2.Controls.Add(richTextBox1);
            mainSplitContainer.Panel2.Controls.Add(bresenhamRadioButton);
            mainSplitContainer.Panel2.Controls.Add(libraryRadioButton);
            mainSplitContainer.Panel2.Controls.Add(newPolyButton);
            mainSplitContainer.Size = new Size(800, 450);
            mainSplitContainer.SplitterDistance = 600;
            mainSplitContainer.TabIndex = 0;
            // 
            // captionButton
            // 
            captionButton.Location = new System.Drawing.Point(10, 47);
            captionButton.Name = "captionButton";
            captionButton.Size = new Size(174, 29);
            captionButton.TabIndex = 3;
            captionButton.Text = "Captions: Enabled";
            captionButton.UseVisualStyleBackColor = true;
            captionButton.Click += captionButton_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            richTextBox1.Enabled = false;
            richTextBox1.Location = new System.Drawing.Point(3, 125);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(190, 322);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
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
            newPolyButton.Size = new Size(174, 29);
            newPolyButton.TabIndex = 0;
            newPolyButton.Text = "New Polygon";
            newPolyButton.UseVisualStyleBackColor = true;
            newPolyButton.Click += newPolyButton_Click;
            // 
            // vertexContextMenuStrip
            // 
            vertexContextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteVertexToolStripMenuItem });
            vertexContextMenuStrip.Name = "contextMenuStrip1";
            vertexContextMenuStrip.Size = new Size(139, 26);
            // 
            // deleteVertexToolStripMenuItem
            // 
            deleteVertexToolStripMenuItem.Name = "deleteVertexToolStripMenuItem";
            deleteVertexToolStripMenuItem.Size = new Size(138, 22);
            deleteVertexToolStripMenuItem.Text = "Delete point";
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
            addPointToolStripMenuItem.Text = "Add point";
            addPointToolStripMenuItem.Click += addPointToolStripMenuItem_Click;
            // 
            // forceVerticalToolStripMenuItem
            // 
            forceVerticalToolStripMenuItem.Name = "forceVerticalToolStripMenuItem";
            forceVerticalToolStripMenuItem.Size = new Size(180, 22);
            forceVerticalToolStripMenuItem.Text = "Make vertical";
            forceVerticalToolStripMenuItem.Click += forceVerticalToolStripMenuItem_Click;
            // 
            // forceHorizontalToolStripMenuItem
            // 
            forceHorizontalToolStripMenuItem.Name = "forceHorizontalToolStripMenuItem";
            forceHorizontalToolStripMenuItem.Size = new Size(180, 22);
            forceHorizontalToolStripMenuItem.Text = "Make horizontal";
            forceHorizontalToolStripMenuItem.Click += forceHorizontalToolStripMenuItem_Click;
            // 
            // forceLengthToolStripMenuItem
            // 
            forceLengthToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { currentLengthToolStripMenuItem, setLengthToolStripMenuItem });
            forceLengthToolStripMenuItem.Name = "forceLengthToolStripMenuItem";
            forceLengthToolStripMenuItem.Size = new Size(180, 22);
            forceLengthToolStripMenuItem.Text = "Force length...";
            // 
            // currentLengthToolStripMenuItem
            // 
            currentLengthToolStripMenuItem.Name = "currentLengthToolStripMenuItem";
            currentLengthToolStripMenuItem.Size = new Size(180, 22);
            currentLengthToolStripMenuItem.Text = "Current length";
            currentLengthToolStripMenuItem.Click += currentLengthToolStripMenuItem_Click;
            // 
            // setLengthToolStripMenuItem
            // 
            setLengthToolStripMenuItem.Name = "setLengthToolStripMenuItem";
            setLengthToolStripMenuItem.Size = new Size(180, 22);
            setLengthToolStripMenuItem.Text = "Set length";
            setLengthToolStripMenuItem.Click += setLengthToolStripMenuItem_Click;
            // 
            // setBezierCurveToolStripMenuItem
            // 
            setBezierCurveToolStripMenuItem.Name = "setBezierCurveToolStripMenuItem";
            setBezierCurveToolStripMenuItem.Size = new Size(180, 22);
            setBezierCurveToolStripMenuItem.Text = "Create Bezier Curve";
            setBezierCurveToolStripMenuItem.Click += setBezierCurveToolStripMenuItem_Click;
            // 
            // removeBoundsToolStripMenuItem
            // 
            removeBoundsToolStripMenuItem.Name = "removeBoundsToolStripMenuItem";
            removeBoundsToolStripMenuItem.Size = new Size(180, 22);
            removeBoundsToolStripMenuItem.Text = "Remove restrictions";
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
            Text = "Polygon Drawer";
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
        private RichTextBox richTextBox1;
        private Button captionButton;
        private ToolStripMenuItem currentLengthToolStripMenuItem;
        private ToolStripMenuItem setLengthToolStripMenuItem;
    }
}