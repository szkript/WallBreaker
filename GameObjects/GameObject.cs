using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WallBreaker.GameObjects
{

    public abstract class GameObject
    {
        public Vector2 Position;
        public Dictionary<Side, List<int>> sides = new Dictionary<Side, List<int>>();
        public double Width { get; set; }
        public double Height { get; set; } 
        public GameObject()
        {

        }
        internal void CalculateSides()
        {
            List<int> topSide = Enumerable.Range((int)Position.X, (int)Width + 1).ToList();
            List<int> rightSide = Enumerable.Range((int)Position.Y + (int)Width + 1, (int)Height).ToList();
            List<int> bottomSide = Enumerable.Range((int)Position.X + (int)Height + 1, (int)Width).ToList();
            List<int> leftSide = Enumerable.Range((int)Position.Y, (int)Height + 1).ToList();

            sides.Add(Side.Top, topSide);
            sides.Add(Side.Right, rightSide);
            sides.Add(Side.Bottom, bottomSide);
            sides.Add(Side.Left, leftSide);

        }
    }
    public enum Axis
    {
        X,
        Y
    }
    public enum Side
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
