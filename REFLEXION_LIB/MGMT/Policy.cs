using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using REFLEXION_LIB.Object;
using System.Drawing;

using System.Reflection;

namespace REFLEXION_LIB
{
    public static class Policy
    {
        public static string REFLEXION_GAME_NAME = "Reflexion";
        public static string REFLEXION_GAME_AUTHOR = "Morteza Raeisi Vanani";
        public static string REFLEXION_GAME_AUTHOR_EMAIL = "ma.rvgroup@gmail.com";

        public static string REFLEXION_GAME_FILE_EXTENSION = ".xion";
        public static string DEFAULT_PAGE_NAME = "first_page";
        public static string DEFAULT_STARTPOINT_NAME = "start_point";
        public static string DEFAULT_BALL_NAME = "ball";
        public static Int32 MAX_NAME_LENGTH = 25;
        public static Point DEFAULT_SCREEN_SIZE { get { return new Point(15, 10); } }
        public static Point MAX_SCREEN_SIZE { get { return new Point(30, 20); } }
        public static Point MIN_SCREEN_SIZE { get { return new Point(5, 5); } }
        public static Size DEFAULT_CELL_SIZE { get { return new Size(30, 30); } }
        public static Size MAX_CELL_SIZE { get { return new Size(45, 45); } }
        public static Size MIN_CELL_SIZE { get { return new Size(30, 30); } }
        public static Int32 DEFAULT_BACKCOLOR { get { return Color.White.ToArgb(); } }

        public static string SuggestObjectNameId(Type tpObj, Page pg)
        {
            string ok = tpObj.Name;
            int index = 0;
            do
            {
                ok = tpObj.Name.ToLower() + (++index).ToString();
            }
            while (pg.Find(ok) != null);
            return ok;
        }
        public static Point ConvertLocationToShortCircuite(Point location, Page pg)
        {
            return location.rToLoc(pg);
        }
        public static Point ConvertShortCircuiteLocToLong(Point location, Page pg)
        {
            return location.rToLocation(pg);
        }

        #region Checking policy

        public static void CheckScreenRange(Point value) { CheckScreenSizeRange(value, MIN_SCREEN_SIZE, MAX_SCREEN_SIZE); }
        public static void CheckScreenSizeRange(Point pnt, Point min, Point max)
        {
            if ((pnt.X < min.X || pnt.X > max.X)
                ||
                (pnt.Y < min.Y || pnt.Y > max.Y))
                throw new ArgumentOutOfRangeException("CheckScreenSizeRange: Out of range.");
        }
        public static bool CheckScreenSizeRangeTry(Point pnt, Point min, Point max)
        {
            try
            {
                CheckScreenSizeRange(pnt, min, max);
                return true;
            }
            catch { ;}
            return false;
        }

        public static void CheckCellSizeRange(Size value) { CheckCellSizeRange(value, MIN_CELL_SIZE, MAX_CELL_SIZE); }
        public static void CheckCellSizeRange(Size pnt, Size min, Size max)
        {
            if ((pnt.Width < min.Width || pnt.Width > max.Width)
                ||
                (pnt.Height < min.Height || pnt.Height > max.Height))
                throw new ArgumentOutOfRangeException("CheckCellSizeRange: Out of range.");
        }
        public static bool CheckCellSizeRangeTry(Size pnt, Size min, Size max)
        {
            try
            {
                CheckCellSizeRange(pnt, min, max);
                return true;
            }
            catch { ;}
            return false;
        }

        public static bool CheckObjectNameSyntaxTry(string name, out string report)
        {
            try
            {
                CheckObjectNameSyntax(name);
                report = null;
                return true;
            }
            catch (Exception ex)
            {
                report = ex.Message;
            }
            return false;
        }
        public static bool CheckObjectNameSyntax(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("CheckObjectNameSyntax: Empty or null");
            if (name.Length > MAX_NAME_LENGTH) throw new Exception("CheckObjectNameSyntax: Too long length ,max " + MAX_NAME_LENGTH);
            if (char.IsDigit(name[0])) throw new Exception("CheckObjectNameSyntax: Cannot start with digit");
            foreach (char c in name)
                if (!(char.IsLetterOrDigit(c) || c == '_'))
                    throw new Exception(string.Format("CheckObjectNameSyntax: Not valid char '{0}'", c == ' ' ? "space" : c.ToString()));

            return true;
        }

        #endregion

        public static BaseObject CreateInstance(Type type)
        {
            return (BaseObject)type.GetConstructors()[0].Invoke(new object[] { (type as Type).Name });
        }
        public static bool IsProgrammable(Type type)
        {
            return IsProgrammable(type, typeof(ProgrammableAttribute));
        }
        private static bool IsProgrammable(Type type, Type typeOfProgrammable)
        {
            return
                ((ProgrammableAttribute)System.Attribute.GetCustomAttribute(type, typeOfProgrammable)).Can;
        }
        public static IEnumerable<Type> GetRegisteredProgrammableObject()
        {
            Type atr = typeof(ProgrammableAttribute);
            Type ass = typeof(BaseObject);
            foreach (var t in System.Reflection.Assembly.GetAssembly(ass).GetTypes())
                if (System.Attribute.IsDefined(t, atr))
                   // && IsProgrammable(t, atr))

                    yield return t;
        }
        public static bool GetExplanationAttribute(Type type, out string name, out string exp)
        {
            ExplanationAttribute x = (ExplanationAttribute)System.Attribute.
                GetCustomAttribute(type, typeof(ExplanationAttribute));
            if (x == null)
            {
                name = exp = string.Empty;
                return false;
            }
            name = x.Name;
            exp = x.Explanation;
            return true;
        }

        internal static ProgrammableAttribute IsProgrammableMethod(MethodInfo m)
        {
            foreach (var t in m.GetCustomAttributes(typeof(ProgrammableAttribute), true))
            {
                ProgrammableAttribute atr = t as ProgrammableAttribute;
                if (atr != null) return atr;
            }//endeach
            return null;
        }
     };
}
