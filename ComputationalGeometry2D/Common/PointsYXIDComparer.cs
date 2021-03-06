﻿using System.Collections.Generic;

using ComparingDoubles;

namespace ComputationalGeometry2D.Common
{
    class PointsYXIDComparer : Comparer<Point>
    {
        private int idOrderMultiplier = 1;

        public PointsYXIDComparer(PointsIDOrder pointsIDOrder = PointsIDOrder.Ascending)
        {
            SetIDOrder(pointsIDOrder);
        }

        public void SetIDOrder(PointsIDOrder pointsIDOrder)
        {
            if (pointsIDOrder == PointsIDOrder.Ascending)
                idOrderMultiplier = 1;
            else // if (pointsIDOrder == PointsIDOrder.Descending)
                idOrderMultiplier = -1;
        }

        public override int Compare(Point p1, Point p2)
        {
            if (p1.Y.IsAlmostEqualTo(p2.Y))
            {
                if (p1.X.IsAlmostEqualTo(p2.X))
                    return p1.ID.CompareTo(p2.ID) * idOrderMultiplier;
                else return p1.X.CompareTo(p2.X);
            }
            else return p1.Y.CompareTo(p2.Y);
        }
    }
}
