using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using ComputationalGeometry2D;

namespace PrimitiveComputationalGeometry2DVisualiser
{ 
    public partial class Form1 : Form
    {
        Graphics gb, gt;

        List<System.Drawing.Point> p = new List<System.Drawing.Point>();
        List<LineSegment> segments = new List<LineSegment>();
        List<ComputationalGeometry2D.Point> points = new List<ComputationalGeometry2D.Point>();
        System.Drawing.Point p1;

        private System.Drawing.Point center = new System.Drawing.Point(300, 300);


        private GeometricAlgorithms geometry = new GeometricAlgorithms();

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        private static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        public Form1()
        {
            InitializeComponent();

            bialy.Image = new Bitmap(600, 600);
            trans.Image = new Bitmap(600, 600);
            gb = Graphics.FromImage(bialy.Image);
            gt = Graphics.FromImage(trans.Image);
            trans.Parent = bialy;

            //uklad wspolrzednych
            gt.DrawLine(Pens.Black, bialy.Location.X + center.X, bialy.Location.Y + 0, bialy.Location.X + center.X, bialy.Location.Y + 2 * center.Y);
            gt.DrawLine(Pens.Black, bialy.Location.X + 0, bialy.Location.Y + center.Y, bialy.Location.X + 2 * center.X, bialy.Location.Y + center.Y);
        }

        private void trans_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                p.Add(e.Location);
                p1 = e.Location;
                
                //p1 = new System.Drawing.Point(e.X - 300, -e.Y + 300); //zmiana współrzędnych
            }
        }

        private void trans_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                gb.DrawLine(Pens.White, p.Last(), p1); // zamazanie poprzedniego odcinka
                gb.DrawLine(Pens.Black, p.Last(), e.Location);
                p1 = e.Location;
                bialy.Refresh();
            }
        }

        private void trans_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                gt.DrawLine(Pens.Black, p.Last(), e.Location);

                ComputationalGeometry2D.Point currentPoint = new ComputationalGeometry2D.Point(e.X - center.X, -e.Y + center.Y);
                
                if (!p.Last().Equals(e.Location)) // jeśli wykliknęliśmy w innym miejscu
                {
                    ComputationalGeometry2D.Point last = new ComputationalGeometry2D.Point(p.Last().X - center.X, -p.Last().Y + center.Y);
                    segments.Add(new LineSegment(last, currentPoint));
                    UpdateSegmentsList();
                }
                else
                {
                    gt.DrawEllipse(Pens.Black, e.Location.X, e.Location.Y, 3, 3);
                    points.Add(currentPoint);
                    UpdatePointsList();
                }

                p.Add(e.Location);
                gb.Clear(Color.White);
                bialy.Refresh();
                trans.Refresh();
                
            }
        }

        private void UpdateSegmentsList()
        {
            segments_lb.Items.Add(segments.Last());
            segments_lb2.Items.Add(segments.Last());
        }

        private void UpdatePointsList()
        {
            points_lb.Items.Add(points.Last());
            points_lb2.Items.Add(points.Last());
        }

        private void orientationTest_btn_Click(object sender, EventArgs e)
        {
            LineSegment testSegment = segments[segments_lb.SelectedIndex];
            ComputationalGeometry2D.Point testPoint = points[points_lb.SelectedIndex];
            OrientationTestResult result = testPoint.OrientationTest(testSegment);
            MessageBox.Show(result.ToString());
        }

        private void segmentsIntersectionsTest_btn_Click(object sender, EventArgs e)
        {
            LineSegment segment1 = segments[segments_lb.SelectedIndex];
            LineSegment segment2 = segments[segments_lb2.SelectedIndex];
            MessageBox.Show(segment1.IntersectsWith(segment2).ToString());
        }

        private void rectangularBoundsIntersectionTest_btn_Click(object sender, EventArgs e)
        {
            LineSegment segment1 = segments[segments_lb.SelectedIndex];
            LineSegment segment2 = segments[segments_lb2.SelectedIndex];
            MessageBox.Show(segment1.RectBoundIntersectsWithRectBoundOf(segment2).ToString());
        }

        private void minDistPair_btn_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ////ClosestPointsPairResult minDistPair0 = geometry.ClosestPairBruteForce(points);
            //long time0 = sw.ElapsedMilliseconds;
            //sw.Restart();

            ClosestPointsPairResult minDistPair1 = geometry.ClosestPairSweepLine(points);
            long time1 = sw.ElapsedMilliseconds;
            sw.Restart();

            ClosestPointsPairResult minDistPair2 = geometry.ClosestPairRecursive(points);
            long time2 = sw.ElapsedMilliseconds;
            sw.Stop();
            
            MessageBox.Show($"{time1}\n{minDistPair1.MinDist}\n{time2}\n{minDistPair2.MinDist}");

            foreach (UnorderedPointsPair pair in minDistPair2.ClosestPairs)
            {
                ComputationalGeometry2D.Point p1 = pair.First;
                PointF drawableP1 = new PointF((float)p1.X + center.X, (float)-p1.Y + center.Y);
                ComputationalGeometry2D.Point p2 = pair.Second;
                PointF drawableP2 = new PointF((float)p2.X + center.X, (float)-p2.Y + center.Y);
                gt.DrawLine(Pens.Black, drawableP1, drawableP2);
            }
            trans.Refresh();
        }

        private void addPoint_btn_Click(object sender, EventArgs e)
        {
            points.Add(new ComputationalGeometry2D.Point(Double.Parse(pointX_txtB.Text), Double.Parse(pointY_txtB.Text)));
            UpdatePointsList();
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            p.Clear();
            points.Clear();
            segments.Clear();
            points_lb.Items.Clear();
            segments_lb.Items.Clear();
            points_lb2.Items.Clear();
            segments_lb2.Items.Clear();

            gt.Clear(Color.White);
            //uklad wspolrzednych
            gt.DrawLine(Pens.Black, bialy.Location.X + center.X, bialy.Location.Y + 0, bialy.Location.X + center.X, bialy.Location.Y +2 * center.Y);
            gt.DrawLine(Pens.Black, bialy.Location.X + 0, bialy.Location.Y + center.X, bialy.Location.X + 2 * center.X, bialy.Location.Y + center.Y);
            trans.Refresh();
        }

        private void convexHullGrahamScan_btn_Click(object sender, EventArgs e)
        {
            Stack<ComputationalGeometry2D.Point> convexHull = geometry.ConvexHullGrahamScan(points);
            StringBuilder convexHullSb = new StringBuilder();
            foreach (ComputationalGeometry2D.Point p in convexHull)
                convexHullSb.AppendLine(p.ToString());
            MessageBox.Show(convexHullSb.ToString());
        }

        private void convexHullJarvis_btn_Click(object sender, EventArgs e)
        {
            Stack<ComputationalGeometry2D.Point> convexHull = geometry.ConvexHullJarvis(points);
            StringBuilder convexHullSb = new StringBuilder();
            foreach (ComputationalGeometry2D.Point p in convexHull)
                convexHullSb.AppendLine(p.ToString());
            MessageBox.Show(convexHullSb.ToString());
        }

        private void segmentIntersection_btn_Click(object sender, EventArgs e)
        {
            List<Intersection> intersections = geometry.SegmentIntersection(segments);
            Size pointSize = new Size(6, 6);

            foreach (Intersection intersection in intersections)
            {
                ComputationalGeometry2D.Point intersectionPoint = intersection.Point;
                PointF drawableIntersectionPoint = new PointF((float)(intersectionPoint.X + center.X - 0.5*pointSize.Width), (float)(-intersectionPoint.Y + center.Y - 0.5*pointSize.Height));
                gt.DrawEllipse(Pens.Red, new RectangleF(drawableIntersectionPoint, pointSize));
            }
            trans.Refresh();
        }

        private void membershipTest_btn_Click(object sender, EventArgs e)
        {
            ComputationalGeometry2D.Point point = points[points_lb.SelectedIndex];
            LineSegment segment = segments[segments_lb.SelectedIndex];
            MessageBox.Show(point.LiesOn(segment).ToString());
        }
    }
}