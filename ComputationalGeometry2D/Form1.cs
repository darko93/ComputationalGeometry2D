using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ComputationalGeometry2D
{ 
    public partial class Form1 : Form
    {
        Graphics gb, gt;

        List<System.Drawing.Point> p = new List<System.Drawing.Point>();
        List<LineSegment> segments = new List<LineSegment>();
        List<ComputationalGeometry2D.Point> points = new List<ComputationalGeometry2D.Point>();
        System.Drawing.Point p1;

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

            //AlgorithmsTester.Instance.HalfPlaneAngularSort();
            //AlgorithmsTester.Instance.AllPlaneAngularSort();

            //LineSegment s = new LineSegment
            //    (
            //        new Point(0.0, 0.0),
            //        new Point(-1E-9, 1.0)
            //    );
            //Point testP = new Point(1.0000001, -1000000000);
            //Point testP2 = new Point(0.124, -125000000);
            //OrientationTestResult orientation = testP.OrientationTest(s);
            //OrientationTestResult orientation2 = testP2.OrientationTest(s);

            //LineSegment s2 = new LineSegment
            //(
            //    new Point(0.0, 0.0),
            //    new Point(-1E-10, 1.0)
            //);
            //Point testP3 = new Point(0.125, -1250000000);
            //Point testP4 = new Point(0.124, -1250000000);
            //OrientationTestResult orientation3 = testP3.OrientationTest(s2);
            //OrientationTestResult orientation4 = testP4.OrientationTest(s2);

            //LineSegment s3 = new LineSegment
            //(
            //   new Point(0.0, 0.0),
            //   new Point(200000.0, 0.0)
            //);

            //Point testP5 = new Point(-10000.0, 0.000000000000001); // collinear, ale mniejszy promień więc mniejszy?
            //OrientationTestResult orientation5 = testP5.OrientationTest(s3);

            //bool xx = true;

            //Point p1 = new Point(0, 0);
            //Point p2 = new Point(1, 0);
            //Point p3 = new Point(3, 0);
            //Point p4 = new Point(0.5, 0);
            //Point p5 = new Point(2.5, 3);
            //Point p6 = new Point(-1, 0);
            //Point p7 = new Point(4, 0);
            //Point p8 = new Point(-2, 0);
            //Point p9 = new Point(5, 0);
            //Point p10 = new Point(-3, 0);
            //Point p11 = new Point(6, 0);
            //Point p12 = new Point(-4, 0);
            //Point p13 = new Point(7, 0);
            //Point p14 = new Point(-5, 0);
            //Point p15 = new Point(8, 0);
            //Point p16 = new Point(-6, 0);
            //Point p17 = new Point(9, 0);
            //Point p18 = new Point(8, 0); // już taki jest
            //List<Point> lp = new List<Point>() { p16, p14, p12, p12, p12, p12, p10, p8, p6, p1, p4, p2, p5, p3, p7, p9, p11, p13, p15, p17, p18 };
            //ClosestPointsPairResult result = geometry.ClosestPairRecursive(lp, PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult);
            ////List<Point> lp2 = lp.Distinct(new PointsXYEqualityComparer()).ToList();
            ////List<Point> lp3 = lp.Distinct().ToList();

            bialy.Image = new Bitmap(600, 600);
            trans.Image = new Bitmap(600, 600);
            gb = Graphics.FromImage(bialy.Image);
            gt = Graphics.FromImage(trans.Image);
            trans.Parent = bialy;

            //uklad wspolrzednych
            gt.DrawLine(Pens.Black, bialy.Location.X + 300, bialy.Location.Y + 0, bialy.Location.X + 300, bialy.Location.Y + 600);
            gt.DrawLine(Pens.Black, bialy.Location.X + 0, bialy.Location.Y + 300, bialy.Location.X + 600, bialy.Location.Y + 300);
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

                ComputationalGeometry2D.Point currentPoint = new ComputationalGeometry2D.Point(e.X - 300, -e.Y + 300);
                
                if (!p.Last().Equals(e.Location)) // jeśli wykliknęliśmy w innym miejscu
                {
                    ComputationalGeometry2D.Point last = new ComputationalGeometry2D.Point(p.Last().X - 300, -p.Last().Y + 300);
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

        private List<ComputationalGeometry2D.Point> GetRandomPoints()
        {
            List<ComputationalGeometry2D.Point> randomPoints = new List<ComputationalGeometry2D.Point>();
            for (int i = 0; i < 1000000; i++)
            {
                randomPoints.Add(new ComputationalGeometry2D.Point(RandomNumber(-3000000, 3000000), RandomNumber(-3000000, 3000000)));
            }
            return randomPoints;
        }

        private void minDistPair_btn_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            List<ComputationalGeometry2D.Point> randomPoints = GetRandomPoints();
            sw.Start();

            ////ClosestPointsPairResult minDistPair1 = geometry.ClosestPairBruteForce(points);
            //ClosestPointsPairResult minDistPair1 = geometry.ClosestPairBruteForce(randomPoints);
            //long time1 = sw.ElapsedMilliseconds;
            //sw.Restart();

            //ClosestPointsPairResult minDistPair0 = geometry.ClosestPairRecursive(points, PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult);
            ClosestPointsPairResult minDistPair0 = geometry.ClosestPairRecursive(randomPoints, PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult);
            long time0 = sw.ElapsedMilliseconds;
            sw.Restart();
            //ClosestPointsPairResult minDistPair2 = geometry.ClosestPairRecursive(points, PointsCoordDuplicatesMode.ContainedInListButNotAllowedInResult);
            ClosestPointsPairResult minDistPair2 = geometry.ClosestPairRecursive(randomPoints, PointsCoordDuplicatesMode.NotContainedInList);
            long time2 = sw.ElapsedMilliseconds;

            sw.Restart();
            //ClosestPointsPairResult minDistPair3 = geometry.ClosestPairSweepLine(points, PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult);
            ClosestPointsPairResult minDistPair3 = geometry.ClosestPairSweepLine(randomPoints, PointsCoordDuplicatesMode.ContainedInListAndAllowedInResult);
            long time3 = sw.ElapsedMilliseconds;

            //MessageBox.Show(geometry.Counter.ToString());
            //geometry.Counter = 0;

            sw.Restart();
            //ClosestPointsPairResult minDistPair4 = geometry.ClosestPairSweepLine(points, PointsCoordDuplicatesMode.ContainedInListButNotAllowedInResult);
            ClosestPointsPairResult minDistPair4 = geometry.ClosestPairSweepLine(randomPoints, PointsCoordDuplicatesMode.ContainedInListButNotAllowedInResult);
            long time4 = sw.ElapsedMilliseconds;
            sw.Stop();
            
            //MessageBox.Show($"{{time1}\n{minDistPair1.MinDist}\ntime2}\n{minDistPair2.MinDist}\n{time3}\n{minDistPair3.MinDist}\n{time4}\n{minDistPair4.MinDist}");
            //MessageBox.Show($"{time0}\n{minDistPair0.MinDist}\n{time2}\n{minDistPair2.MinDist}\n{time3}\n{minDistPair3.MinDist}\n{time4}\n{minDistPair4.MinDist}");

            foreach (UnorderedPointsPair pair in minDistPair2.PointsPairs)
            {
                ComputationalGeometry2D.Point p1 = pair.First;
                p1 = p1.GetReflectedAboutTheXAxis();
                p1.Translate(300.0, 300.0);
                ComputationalGeometry2D.Point p2 = pair.Second;
                p2 = p2.GetReflectedAboutTheXAxis();
                p2.Translate(300.0, 300.0);

                gt.DrawLine(Pens.Black, (System.Drawing.PointF)p1, (System.Drawing.PointF)p2);
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
            gt.DrawLine(Pens.Black, bialy.Location.X + 300, bialy.Location.Y + 0, bialy.Location.X + 300, bialy.Location.Y + 600);
            gt.DrawLine(Pens.Black, bialy.Location.X + 0, bialy.Location.Y + 300, bialy.Location.X + 600, bialy.Location.Y + 300);
            trans.Refresh();
        }

        private void convexHullGrahamScan_btn_Click(object sender, EventArgs e)
        {
            Stack<Point> convexHull = geometry.ConvexHullGrahamScan(points);
            StringBuilder convexHullSb = new StringBuilder();
            foreach (Point p in convexHull)
                convexHullSb.AppendLine(p.ToString());
            MessageBox.Show(convexHullSb.ToString());
        }

        private void membershipTest_btn_Click(object sender, EventArgs e)
        {
            ComputationalGeometry2D.Point point = points[points_lb.SelectedIndex];
            LineSegment segment = segments[segments_lb.SelectedIndex];
            MessageBox.Show(point.LiesOn(segment).ToString());
        }
    }
}