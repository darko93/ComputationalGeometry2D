namespace ComputationalGeometry2D
{
    partial class Form1
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
            this.bialy = new System.Windows.Forms.PictureBox();
            this.trans = new System.Windows.Forms.PictureBox();
            this.menu_pnl = new System.Windows.Forms.Panel();
            this.clear_btn = new System.Windows.Forms.Button();
            this.pointY_txtB = new System.Windows.Forms.TextBox();
            this.pointX_txtB = new System.Windows.Forms.TextBox();
            this.addPoint_btn = new System.Windows.Forms.Button();
            this.minDistPair_btn = new System.Windows.Forms.Button();
            this.rectangularBoundsIntersectionTest_btn = new System.Windows.Forms.Button();
            this.segmentsIntersectionsTest_btn = new System.Windows.Forms.Button();
            this.membershipTest_btn = new System.Windows.Forms.Button();
            this.points_lb2 = new System.Windows.Forms.ListBox();
            this.segments_lb2 = new System.Windows.Forms.ListBox();
            this.orientationTest_btn = new System.Windows.Forms.Button();
            this.points_lb = new System.Windows.Forms.ListBox();
            this.segments_lb = new System.Windows.Forms.ListBox();
            this.convexHullGrahamScan_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bialy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trans)).BeginInit();
            this.menu_pnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // bialy
            // 
            this.bialy.BackColor = System.Drawing.Color.White;
            this.bialy.Location = new System.Drawing.Point(0, 0);
            this.bialy.Name = "bialy";
            this.bialy.Size = new System.Drawing.Size(600, 600);
            this.bialy.TabIndex = 0;
            this.bialy.TabStop = false;
            // 
            // trans
            // 
            this.trans.BackColor = System.Drawing.Color.Transparent;
            this.trans.Location = new System.Drawing.Point(0, 0);
            this.trans.Name = "trans";
            this.trans.Size = new System.Drawing.Size(600, 600);
            this.trans.TabIndex = 1;
            this.trans.TabStop = false;
            this.trans.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trans_MouseDown);
            this.trans.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trans_MouseMove);
            this.trans.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trans_MouseUp);
            // 
            // menu_pnl
            // 
            this.menu_pnl.Controls.Add(this.convexHullGrahamScan_btn);
            this.menu_pnl.Controls.Add(this.clear_btn);
            this.menu_pnl.Controls.Add(this.pointY_txtB);
            this.menu_pnl.Controls.Add(this.pointX_txtB);
            this.menu_pnl.Controls.Add(this.addPoint_btn);
            this.menu_pnl.Controls.Add(this.minDistPair_btn);
            this.menu_pnl.Controls.Add(this.rectangularBoundsIntersectionTest_btn);
            this.menu_pnl.Controls.Add(this.segmentsIntersectionsTest_btn);
            this.menu_pnl.Controls.Add(this.membershipTest_btn);
            this.menu_pnl.Controls.Add(this.points_lb2);
            this.menu_pnl.Controls.Add(this.segments_lb2);
            this.menu_pnl.Controls.Add(this.orientationTest_btn);
            this.menu_pnl.Controls.Add(this.points_lb);
            this.menu_pnl.Controls.Add(this.segments_lb);
            this.menu_pnl.Location = new System.Drawing.Point(606, 0);
            this.menu_pnl.Name = "menu_pnl";
            this.menu_pnl.Size = new System.Drawing.Size(410, 600);
            this.menu_pnl.TabIndex = 2;
            // 
            // clear_btn
            // 
            this.clear_btn.Location = new System.Drawing.Point(22, 531);
            this.clear_btn.Name = "clear_btn";
            this.clear_btn.Size = new System.Drawing.Size(75, 23);
            this.clear_btn.TabIndex = 12;
            this.clear_btn.Text = "Clear";
            this.clear_btn.UseVisualStyleBackColor = true;
            this.clear_btn.Click += new System.EventHandler(this.clear_btn_Click);
            // 
            // pointY_txtB
            // 
            this.pointY_txtB.Location = new System.Drawing.Point(89, 249);
            this.pointY_txtB.Name = "pointY_txtB";
            this.pointY_txtB.Size = new System.Drawing.Size(51, 20);
            this.pointY_txtB.TabIndex = 11;
            // 
            // pointX_txtB
            // 
            this.pointX_txtB.Location = new System.Drawing.Point(22, 249);
            this.pointX_txtB.Name = "pointX_txtB";
            this.pointX_txtB.Size = new System.Drawing.Size(48, 20);
            this.pointX_txtB.TabIndex = 10;
            // 
            // addPoint_btn
            // 
            this.addPoint_btn.Location = new System.Drawing.Point(153, 249);
            this.addPoint_btn.Name = "addPoint_btn";
            this.addPoint_btn.Size = new System.Drawing.Size(75, 23);
            this.addPoint_btn.TabIndex = 9;
            this.addPoint_btn.Text = "Add point";
            this.addPoint_btn.UseVisualStyleBackColor = true;
            this.addPoint_btn.Click += new System.EventHandler(this.addPoint_btn_Click);
            // 
            // minDistPair_btn
            // 
            this.minDistPair_btn.Location = new System.Drawing.Point(22, 414);
            this.minDistPair_btn.Name = "minDistPair_btn";
            this.minDistPair_btn.Size = new System.Drawing.Size(75, 23);
            this.minDistPair_btn.TabIndex = 8;
            this.minDistPair_btn.Text = "Min dist pair";
            this.minDistPair_btn.UseVisualStyleBackColor = true;
            this.minDistPair_btn.Click += new System.EventHandler(this.minDistPair_btn_Click);
            // 
            // rectangularBoundsIntersectionTest_btn
            // 
            this.rectangularBoundsIntersectionTest_btn.Location = new System.Drawing.Point(99, 345);
            this.rectangularBoundsIntersectionTest_btn.Name = "rectangularBoundsIntersectionTest_btn";
            this.rectangularBoundsIntersectionTest_btn.Size = new System.Drawing.Size(198, 23);
            this.rectangularBoundsIntersectionTest_btn.TabIndex = 7;
            this.rectangularBoundsIntersectionTest_btn.Text = "Rectangular bounds intersection test";
            this.rectangularBoundsIntersectionTest_btn.UseVisualStyleBackColor = true;
            this.rectangularBoundsIntersectionTest_btn.Click += new System.EventHandler(this.rectangularBoundsIntersectionTest_btn_Click);
            // 
            // segmentsIntersectionsTest_btn
            // 
            this.segmentsIntersectionsTest_btn.Location = new System.Drawing.Point(221, 316);
            this.segmentsIntersectionsTest_btn.Name = "segmentsIntersectionsTest_btn";
            this.segmentsIntersectionsTest_btn.Size = new System.Drawing.Size(143, 23);
            this.segmentsIntersectionsTest_btn.TabIndex = 6;
            this.segmentsIntersectionsTest_btn.Text = "Segments intersection test";
            this.segmentsIntersectionsTest_btn.UseVisualStyleBackColor = true;
            this.segmentsIntersectionsTest_btn.Click += new System.EventHandler(this.segmentsIntersectionsTest_btn_Click);
            // 
            // membershipTest_btn
            // 
            this.membershipTest_btn.Location = new System.Drawing.Point(114, 316);
            this.membershipTest_btn.Name = "membershipTest_btn";
            this.membershipTest_btn.Size = new System.Drawing.Size(101, 23);
            this.membershipTest_btn.TabIndex = 5;
            this.membershipTest_btn.Text = "Membership test";
            this.membershipTest_btn.UseVisualStyleBackColor = true;
            this.membershipTest_btn.Click += new System.EventHandler(this.membershipTest_btn_Click);
            // 
            // points_lb2
            // 
            this.points_lb2.FormattingEnabled = true;
            this.points_lb2.Location = new System.Drawing.Point(211, 126);
            this.points_lb2.Name = "points_lb2";
            this.points_lb2.Size = new System.Drawing.Size(127, 108);
            this.points_lb2.TabIndex = 4;
            // 
            // segments_lb2
            // 
            this.segments_lb2.FormattingEnabled = true;
            this.segments_lb2.Location = new System.Drawing.Point(211, 12);
            this.segments_lb2.Name = "segments_lb2";
            this.segments_lb2.Size = new System.Drawing.Size(176, 108);
            this.segments_lb2.TabIndex = 3;
            // 
            // orientationTest_btn
            // 
            this.orientationTest_btn.Location = new System.Drawing.Point(22, 316);
            this.orientationTest_btn.Name = "orientationTest_btn";
            this.orientationTest_btn.Size = new System.Drawing.Size(86, 23);
            this.orientationTest_btn.TabIndex = 2;
            this.orientationTest_btn.Text = "Orientation test";
            this.orientationTest_btn.UseVisualStyleBackColor = true;
            this.orientationTest_btn.Click += new System.EventHandler(this.orientationTest_btn_Click);
            // 
            // points_lb
            // 
            this.points_lb.FormattingEnabled = true;
            this.points_lb.Location = new System.Drawing.Point(22, 126);
            this.points_lb.Name = "points_lb";
            this.points_lb.Size = new System.Drawing.Size(118, 108);
            this.points_lb.TabIndex = 1;
            // 
            // segments_lb
            // 
            this.segments_lb.FormattingEnabled = true;
            this.segments_lb.Location = new System.Drawing.Point(22, 12);
            this.segments_lb.Name = "segments_lb";
            this.segments_lb.Size = new System.Drawing.Size(183, 108);
            this.segments_lb.TabIndex = 0;
            // 
            // convexHullGrahamScan_btn
            // 
            this.convexHullGrahamScan_btn.Location = new System.Drawing.Point(22, 466);
            this.convexHullGrahamScan_btn.Name = "convexHullGrahamScan_btn";
            this.convexHullGrahamScan_btn.Size = new System.Drawing.Size(142, 23);
            this.convexHullGrahamScan_btn.TabIndex = 13;
            this.convexHullGrahamScan_btn.Text = "Convex Hull Graham Scan";
            this.convexHullGrahamScan_btn.UseVisualStyleBackColor = true;
            this.convexHullGrahamScan_btn.Click += new System.EventHandler(this.convexHullGrahamScan_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 600);
            this.Controls.Add(this.menu_pnl);
            this.Controls.Add(this.trans);
            this.Controls.Add(this.bialy);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bialy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trans)).EndInit();
            this.menu_pnl.ResumeLayout(false);
            this.menu_pnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox bialy;
        private System.Windows.Forms.PictureBox trans;
        private System.Windows.Forms.Panel menu_pnl;
        private System.Windows.Forms.ListBox segments_lb;
        private System.Windows.Forms.ListBox points_lb;
        private System.Windows.Forms.Button orientationTest_btn;
        private System.Windows.Forms.ListBox segments_lb2;
        private System.Windows.Forms.ListBox points_lb2;
        private System.Windows.Forms.Button membershipTest_btn;
        private System.Windows.Forms.Button segmentsIntersectionsTest_btn;
        private System.Windows.Forms.Button rectangularBoundsIntersectionTest_btn;
        private System.Windows.Forms.Button minDistPair_btn;
        private System.Windows.Forms.TextBox pointY_txtB;
        private System.Windows.Forms.TextBox pointX_txtB;
        private System.Windows.Forms.Button addPoint_btn;
        private System.Windows.Forms.Button clear_btn;
        private System.Windows.Forms.Button convexHullGrahamScan_btn;
    }
}

