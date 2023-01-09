using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Art
{
    public static class Plane
    {
        public static Dictionary<string, Point> Points = new Dictionary<string, Point>();
        public static List<BezierBase> Beziers = new List<BezierBase>();

        static Plane()
        {
            GeneratePoints();
        }

        public static void GeneratePoints()
        {
            Points = new Dictionary<string, Point>();
            Points["A"] = new Point(616, 382);
            Points["B"] = new Point(797, 182);
            Points["C"] = new Point(850, 194);
            Points["D"] = new Point(904, 210);
            Points["E"] = new Point(920, 232);
            Points["F"] = new Point(921, 301);
            Points["G"] = new Point(798, 426);
            Points["H"] = new Point(825, 444);
            Points["I"] = new Point(851, 461);
            Points["J"] = new Point(903, 434);
            Points["K"] = new Point(897, 498);
            Points["L"] = new Point(974, 546);
            Points["M"] = new Point(952, 548);
            Points["N"] = new Point(931, 586);
            Points["O"] = new Point(916, 571);
            Points["P"] = new Point(822, 542);
            Points["Q"] = new Point(730, 517);
            Points["R"] = new Point(682, 572);
            Points["S"] = new Point(674, 551);
            Points["T"] = new Point(597, 606);
            Points["U"] = new Point(535, 587);
            Points["V"] = new Point(514, 586);
            Points["W"] = new Point(492, 586);
            Points["X"] = new Point(485, 572);
            Points["Y"] = new Point(446, 560);
            Points["Z"] = new Point(420, 552);
            Points["A1"] = new Point(387, 542);
            Points["B1"] = new Point(399, 509);
            Points["C1"] = new Point(458, 514);
            Points["D1"] = new Point(468, 510);
            Points["E1"] = new Point(424, 454);
            Points["F1"] = new Point(274, 441);
            Points["G1"] = new Point(266, 450);
            Points["H1"] = new Point(253, 437);
            Points["I1"] = new Point(290, 402);
            Points["J1"] = new Point(378, 384);
            Points["K1"] = new Point(515, 379);
            Points["L1"] = new Point(563, 313);
            Points["M1"] = new Point(582, 382);
            Points["N1"] = new Point(598, 382);
            Points["O1"] = new Point(518, 495);
            Points["P1"] = new Point(529, 486);
            Points["Q1"] = new Point(532, 475);
            Points["R1"] = new Point(517, 470);
            Points["S1"] = new Point(506, 470);
            Points["T1"] = new Point(508, 484);
        }

        public static void Build()
        {
            Beziers = new List<BezierBase>();

            var smooth1 = new BezierSmooth(Points["A"], Points["B"], Points["C"]);
            var smooth2 = new BezierSmooth(Points["C"], Points["D"], Points["E"]);
            smooth1.Right = smooth2;
            smooth2.Left = smooth1;

            Beziers.Add(smooth1);
            Beziers.Add(smooth2);
            Beziers.Add(new Bezier(Points["E"], Points["F"], Points["G"]));
            Beziers.Add(new Bezier(Points["G"], Points["H"], Points["I"]));
            Beziers.Add(new Bezier(Points["I"], Points["J"], Points["K"]));
            Beziers.Add(new Bezier(Points["K"], Points["L"], Points["M"]));
            Beziers.Add(new Bezier(Points["M"], Points["N"], Points["O"]));
            Beziers.Add(new Bezier(Points["O"], Points["P"], Points["Q"]));
            Beziers.Add(new Bezier(Points["Q"], Points["R"], Points["S"]));
            Beziers.Add(new Bezier(Points["S"], Points["T"], Points["U"]));
            Beziers.Add(new Bezier(Points["U"], Points["V"], Points["W"]));
            Beziers.Add(new Bezier(Points["W"], Points["X"], Points["Y"]));
            Beziers.Add(new Bezier(Points["Y"], Points["Z"], Points["A1"]));
            Beziers.Add(new Bezier(Points["A1"], Points["B1"], Points["C1"]));
            Beziers.Add(new Bezier(Points["C1"], Points["D1"], Points["E1"]));
            Beziers.Add(new Bezier(Points["E1"], Points["F1"], Points["G1"]));
            Beziers.Add(new Bezier(Points["G1"], Points["H1"], Points["I1"]));
            Beziers.Add(new Bezier(Points["I1"], Points["J1"], Points["K1"]));
            Beziers.Add(new Bezier(Points["K1"], Points["L1"], Points["M1"]));
            Beziers.Add(new Bezier(Points["M1"], Points["N1"], Points["A"]));
            Beziers.Add(new Bezier(Points["O1"], Points["P1"], Points["Q1"]));
            Beziers.Add(new Bezier(Points["Q1"], Points["R1"], Points["S1"]));
            Beziers.Add(new Bezier(Points["S1"], Points["T1"], Points["O1"]));
        }

        public static void Build(Dictionary<string, Point> points)
        {
            Beziers = new List<BezierBase>();
            Beziers.Add(new Bezier(points["A"], points["B"], points["C"]));
            Beziers.Add(new Bezier(points["C"], points["D"], points["E"]));

            Beziers.Add(new Bezier(points["E"], points["F"], points["G"]));
            Beziers.Add(new Bezier(points["G"], points["H"], points["I"]));
            Beziers.Add(new Bezier(points["I"], points["J"], points["K"]));
            Beziers.Add(new Bezier(points["K"], points["L"], points["M"]));
            Beziers.Add(new Bezier(points["M"], points["N"], points["O"]));
            Beziers.Add(new Bezier(points["O"], points["P"], points["Q"]));
            Beziers.Add(new Bezier(points["Q"], points["R"], points["S"]));
            Beziers.Add(new Bezier(points["S"], points["T"], points["U"]));
            Beziers.Add(new Bezier(points["U"], points["V"], points["W"]));
            Beziers.Add(new Bezier(points["W"], points["X"], points["Y"]));
            Beziers.Add(new Bezier(points["Y"], points["Z"], points["A1"]));
            Beziers.Add(new Bezier(points["A1"], points["B1"], points["C1"]));
            Beziers.Add(new Bezier(points["C1"], points["D1"], points["E1"]));
            Beziers.Add(new Bezier(points["E1"], points["F1"], points["G1"]));
            Beziers.Add(new Bezier(points["G1"], points["H1"], points["I1"]));
            Beziers.Add(new Bezier(points["I1"], points["J1"], points["K1"]));
            Beziers.Add(new Bezier(points["K1"], points["L1"], points["M1"]));
            Beziers.Add(new Bezier(points["M1"], points["N1"], points["A"]));
            Beziers.Add(new Bezier(points["O1"], points["P1"], points["Q1"]));
            Beziers.Add(new Bezier(points["Q1"], points["R1"], points["S1"]));
            Beziers.Add(new Bezier(points["S1"], points["T1"], points["O1"]));
        }
    }
}
