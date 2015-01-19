using System;
using System.Drawing;

namespace REFLEXION_LIB
{
    public static class ExtensionMethods
    {
        public static bool RaiseEvent(this GameStateChanged del, Game g, GameStateChangedEventArgs e)
        {
            GameStateChanged tmp = System.Threading.Interlocked.CompareExchange<GameStateChanged>(ref del, null, null);
            if (tmp != null) { tmp(g, e); return true; }
            return false;
        }
        public static bool RaiseEvent(this Messaging del, Game g, MessagingEventArgs e)
        {
            Messaging tmp = System.Threading.Interlocked.CompareExchange<Messaging>(ref del, null, null);
            if (tmp != null) { tmp(g, e); return true; }
            return false;
        }
        public static bool RaiseEvent(this PaintedEvent del, Game g, PaintedEventArgs e)
        {
            PaintedEvent tmp = System.Threading.Interlocked.CompareExchange<PaintedEvent>(ref del, null, null);
            if (tmp != null) { tmp(g, e); return true; }
            return false;
        }
        public static bool RaiseEvent(this PageChangedEvent del, Game g, PageChangedEventArgs e)
        {
            PageChangedEvent tmp = System.Threading.Interlocked.CompareExchange<PageChangedEvent>(ref del, null, null);
            if (tmp != null) { tmp(g, e); return true; }
            return false;
        }
        public static bool RaiseEvent(this StateChanged del, Object.BaseObject obj, StateChangedArgs e)
        {
            StateChanged tmp = System.Threading.Interlocked.CompareExchange<StateChanged>(ref del, null, null);
            if (tmp != null) { tmp(obj, e); return true; }
            return false;
        }

        public static Borders rToBorder(this Direction dir)
        {
            if (dir == Direction.Right) return Borders.Right;
            if (dir == Direction.Left) return Borders.Left;
            if (dir == Direction.Up) return Borders.Top;
            return Borders.Bottom;
        }
        public static Direction rReverse(this Direction dir)
        {
            if (dir == Direction.Right) return Direction.Left;
            if (dir == Direction.Left) return Direction.Right;
            if (dir == Direction.Up) return Direction.Down;
            return Direction.Up;
        }

        /// <summary>
        /// Convert Point to "X,Y" string
        /// </summary>
        /// <param name="pnt"></param>
        /// <returns></returns>
        public static string rToString(this Point pnt)
        {
            return string.Format("{0},{1}", pnt.X, pnt.Y);            
        }
        /// <summary>
        /// Convert "X,Y" string to Point type
        /// </summary>
        /// <param name="strPnt"></param>
        /// <returns></returns>
        public static Point rToPoint(this string strPnt)
        {
            try
            {
                string[] s = strPnt.Split(',');
                return new Point(int.Parse(s[0]), int.Parse(s[1]));
            }
            catch
            {
                throw new FormatException("string to point failure");
            }
        }

        /// <summary>
        /// Convert short-circute loc[ation] to location
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cellSize"></param>
        /// <returns></returns>
        public static Point rToLocation(this Point p, Page pg)
        {
            return new Point(p.X * pg.GetCellSize().Width, p.Y * pg.GetCellSize().Height);
        }
        /// <summary>
        /// Convert Location to short-circute loc[ation]
        /// </summary>
        /// <param name="location"></param>
        /// <param name="cellSize"></param>
        /// <returns></returns>
        public static Point rToLoc(this Point location, Page pg)
        {
            return new Point(location.X / pg.GetCellSize().Width, location.Y / pg.GetCellSize().Height);
        }

    };
}
