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

            //List<Tuple<SomePoint, SomePoint>> tupleList = new Geometry().MinDistPairAllowDuplicates<SomePoint>(list);

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
            OrientationTestResult result = testSegment.OrientationTest(testPoint);
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
            for (int i = 0; i < 200000; i++)
            {
                randomPoints.Add(new ComputationalGeometry2D.Point(RandomNumber(-3000, 3000), RandomNumber(-3000, 3000)));
            }
            return randomPoints;
        }

        private void minDistPair_btn_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            List<ComputationalGeometry2D.Point> randomPoints = GetRandomPoints();
            sw.Start();
            //ClosestPointsPairResult minDistPair = geometry.ClosestPairRightDuplicates(points);
            ClosestPointsPairResult minDistPair = geometry.ClosestPairRightDuplicates(randomPoints);
            long time1 = sw.ElapsedMilliseconds;
            sw.Restart();
            //ClosestPointsPairResult minDistPair2 = geometry.ClosestPairNoDuplicates(points);
            ClosestPointsPairResult minDistPair2 = geometry.ClosestPairNoDuplicates(randomPoints);
            long time2 = sw.ElapsedMilliseconds;
            sw.Restart();
            ////ClosestPointsPairResult minDistPair3 = geometry.ClosestPairBruteForce(points);
            //ClosestPointsPairResult minDistPair3 = geometry.ClosestPairBruteForce(randomPoints);
            //long time3 = sw.ElapsedMilliseconds;
            //sw.Restart();
            //ClosestPointsPairResult minDistPair4 = geometry.ClosestPairIterative(points);
            ClosestPointsPairResult minDistPair4 = geometry.ClosestPairIterative(randomPoints);
            long time4 = sw.ElapsedMilliseconds;
            sw.Restart();
            //ClosestPointsPairResult minDistPair5 = geometry.ClosestPairRecursive(points);
            ClosestPointsPairResult minDistPair5 = geometry.ClosestPairRecursive(randomPoints);
            long time5 = sw.ElapsedMilliseconds;
            sw.Stop();

            //List<PointsPair> ppp = minDistPair5.PointsPair.Where(p => p.First.Equals(p.Second)).ToList();

            //MessageBox.Show($"{time1}\n{minDistPair.MinDist}\n{time2}\n{minDistPair2.MinDist}\n{time3}\n{minDistPair3.MinDist}\n{time4}\n{minDistPair4.MinDist}\n{time5}\n{minDistPair5.MinDist}");
            MessageBox.Show($"{time1}\n{minDistPair.MinDist}\n{time2}\n{minDistPair2.MinDist}\n{time4}\n{minDistPair4.MinDist}\n{time5}\n{minDistPair5.MinDist}");
            //MessageBox.Show($"{time1}\n{minDistPair.MinDist}\n{time2}\n{minDistPair2.MinDist}\n{time3}\n{minDistPair3.MinDist}\n{time4}\n{minDistPair4.MinDist}");

            foreach (PointsPair pair in minDistPair4.PointsPairs)
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

        private void membershipTest_btn_Click(object sender, EventArgs e)
        {
            ComputationalGeometry2D.Point point = points[points_lb.SelectedIndex];
            LineSegment segment = segments[segments_lb.SelectedIndex];
            MessageBox.Show(point.LiesOn(segment).ToString());
        }
    }
}