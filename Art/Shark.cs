using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Art
{
    public static class Shark
    {
        public static Dictionary<string, Point> Points = new Dictionary<string, Point>();
        public static List<BezierBase> Beziers = new List<BezierBase>();

        static Shark()
        {
            GeneratePoints();
        }

        public static void GeneratePoints()
        {
            Points = new Dictionary<string, Point>();
            Points["A"] = new Point(414, 294);
            Points["B"] = new Point(369, 175);
            Points["C"] = new Point(383, 144);
            Points["D"] = new Point(422, 150);
            Points["E"] = new Point(489, 270);
            Points["F"] = new Point(676, 114);
            Points["G"] = new Point(686, 173);
            Points["H"] = new Point(651, 210);
            Points["I"] = new Point(633, 320);
            Points["J"] = new Point(646, 324);
            Points["K"] = new Point(660, 329);
            Points["L"] = new Point(739, 232);
            Points["M"] = new Point(745, 286);
            Points["N"] = new Point(730, 297);
            Points["O"] = new Point(720, 355);
            Points["P"] = new Point(748, 371);
            Points["Q"] = new Point(782, 374);
            Points["R"] = new Point(904, 344);
            Points["S"] = new Point(948, 214);
            Points["T"] = new Point(958, 198);
            Points["U"] = new Point(960, 212);
            Points["V"] = new Point(950, 303);
            Points["W"] = new Point(886, 394);
            Points["X"] = new Point(823, 498);
            Points["Y"] = new Point(917, 554);
            Points["Z"] = new Point(936, 579);
            Points["A1"] = new Point(907, 572);
            Points["B1"] = new Point(874, 561);
            Points["C1"] = new Point(836, 520);
            Points["D1"] = new Point(800, 476);
            Points["E1"] = new Point(792, 435);
            Points["F1"] = new Point(766, 437);
            Points["G1"] = new Point(731, 449);
            Points["H1"] = new Point(745, 481);
            Points["I1"] = new Point(730, 490);
            Points["J1"] = new Point(706, 485);
            Points["K1"] = new Point(690, 458);
            Points["L1"] = new Point(643, 462);
            Points["M1"] = new Point(586, 477);
            Points["N1"] = new Point(630, 610);
            Points["O1"] = new Point(609, 640);
            Points["P1"] = new Point(567, 630);
            Points["Q1"] = new Point(509, 503);
            Points["R1"] = new Point(280, 487);
            Points["S1"] = new Point(257, 398);
            Points["T1"] = new Point(289, 302);
        }

        public static void Build()
        {
            Beziers = new List<BezierBase>();

            Beziers.Add(new Bezier(Points["A"], Points["B"], Points["C"]));
            Beziers.Add(new Bezier(Points["C"], Points["D"], Points["E"]));
            Beziers.Add(new Bezier(Points["E"], Points["F"], Points["G"]));
            Beziers.Add(new Bezier(Points["G"], Points["H"], Points["I"]));
            Beziers.Add(new Bezier(Points["I"], Points["J"], Points["K"]));
            Beziers.Add(new Bezier(Points["K"], Points["L"], Points["M"]));
            Beziers.Add(new Bezier(Points["M"], Points["N"], Points["O"]));
            Beziers.Add(new Bezier(Points["O"], Points["P"], Points["Q"]));
            Beziers.Add(new Bezier(Points["Q"], Points["R"], Points["S"]));
            Beziers.Add(new Bezier(Points["S"], Points["T"], Points["U"]));

            var smooth1 = new BezierSmooth(Points["U"], Points["V"], Points["W"]);
            var smooth2 = new BezierSmooth(Points["W"], Points["X"], Points["Y"]);
            smooth1.Right = smooth2;
            smooth2.Left = smooth1;
            Beziers.Add(smooth1);
            Beziers.Add(smooth2);

            Beziers.Add(new Bezier(Points["Y"], Points["Z"], Points["A1"]));
            Beziers.Add(new Bezier(Points["A1"], Points["B1"], Points["C1"]));
            Beziers.Add(new Bezier(Points["C1"], Points["D1"], Points["E1"]));
            Beziers.Add(new Bezier(Points["E1"], Points["F1"], Points["G1"]));
            Beziers.Add(new Bezier(Points["G1"], Points["H1"], Points["I1"]));
            Beziers.Add(new Bezier(Points["I1"], Points["J1"], Points["K1"]));
            Beziers.Add(new Bezier(Points["K1"], Points["L1"], Points["M1"]));
            Beziers.Add(new Bezier(Points["M1"], Points["N1"], Points["O1"]));
            Beziers.Add(new Bezier(Points["O1"], Points["P1"], Points["Q1"]));
            Beziers.Add(new Bezier(Points["Q1"], Points["R1"], Points["S1"]));
            Beziers.Add(new Bezier(Points["S1"], Points["T1"], Points["A"]));
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
            Beziers.Add(new Bezier(points["M1"], points["N1"], points["O1"]));
            Beziers.Add(new Bezier(points["O1"], points["P1"], points["Q1"]));
            Beziers.Add(new Bezier(points["Q1"], points["R1"], points["S1"]));
            Beziers.Add(new Bezier(points["S1"], points["T1"], points["A"]));
        }
    }
}
