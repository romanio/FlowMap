using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Media.Animation;

namespace FlowMapApp
{
    public struct XY
    {

        public double x;
        public double y;
    }

    public class FlowMap
    {
        public double DIA = 0.025;
        public double DENG = 1.168;
        public double DENL = 997.0474354;
        public double VISL = 0.890082518;
        public double VISG = 0.01848;
        public double ANGLE = 90;

        public List<XY> SS = new List<XY>();
        public List<XY> DB = new List<XY>();
        public List<XY> SW = new List<XY>();
        public List<XY> AD = new List<XY>();


        double fr(double U, double D, double V)
        {
            double RE = U * D / V;
            double C = 0.046;
            double X = 0.2;


            if (RE <= 1500)
            {
                C = 16.0;
                X = 1;
            }

            return C * Math.Pow(D * U / V, -X);
        }

        double FINDULS(double UGS, double ULS0)
        {
            double HL = AL / (AL + AG);
            double UG = UGS / (1 - HL);
            double FG = fr(UG, DG * DIA, VG);
            double TAUWG = FG * DENG * UG * UG / 2;
            double B = DIA * (DENL - DENG) * 9.81 * Math.Sin(ARAD);
            double LHS = TAUWG * (SG / AG + SI / AL + SI / AG) + B;

            double ULS = ULS0;
            double EPS = 1e10;

            // Нулевая итерация

            double UL = ULS / HL;
            double FL = fr(UL, DL * DIA, VL);
            double TAUWL = FL * DENL * UL * UL / 2;
            double RHS = TAUWL * SL / AL;
            double Y1 = LHS - RHS;
            double DU = 0.001;

            ULS = ULS + DU;

            do
            {

                UL = ULS / HL;
                FL = fr(UL, DL * DIA, VL);
                TAUWL = FL * DENL * UL * UL / 2;
                RHS = TAUWL * SL / AL;
                double Y2 = LHS - RHS;
                double DY = (Y2 - Y1);
                double ULS1 = ULS;

                ULS = ULS1 - Y2 / (DY / DU);
                DU = ULS - ULS1;
                Y1 = Y2;
                EPS = Math.Abs(RHS - LHS);
            }
            while (EPS > 1e-5);

            return ULS;
        }

        double FINDUGS(double ULS, double UGS0)
        {
            double HL = AL / (AL + AG);
            double UL = ULS / (HL);
            double FL = fr(UL, DL * DIA, VL);
            double TAUWL = FL * DENL * UL * UL / 2;
            double B = DIA * (DENL - DENG) * 9.81 * Math.Sin(ARAD);
            double LHS = TAUWL * SL / AL - B;

            double UGS = UGS0;
            double EPS = 1e10;

            // Нулевая итерация
            double UG = UGS / (1 - HL);
            double FG = fr(UG, DG * DIA, VG);
            double TAUWG = FG * DENG * UG * UG / 2;
            double RHS = TAUWG * (SG / AG + SI / AL + SI / AG);
            double Y1 = LHS - RHS;
            double DU = 0.001;

            UGS = UGS + DU;

            do
            {

                UG = UGS / (1 - HL);
                FG = fr(UG, DG * DIA, VG);
                TAUWG = FG * DENG * UG * UG / 2;
                RHS = TAUWG * (SG / AG + SI / AL + SI / AG);
                double Y2 = LHS - RHS;
                double DY = (Y2 - Y1);
                double UGS1 = UGS;

                UGS = UGS1 - Y2 / (DY / DU);
                DU = UGS - UGS1;
                Y1 = Y2;
                EPS = Math.Abs(RHS - LHS);
            }
            while (EPS > 1e-5);

            return UGS;
        }

        double HD;
        double HLD;
        double SI;
        double AL;
        double A;
        double AG;
        double SG;
        double SL;
        double UL;
        double UG;
        double DL;
        double DG;
        double VL;
        double VG;
        double ARAD;

        public void Generate()
        {

            SS.Clear();
            SW.Clear();
            AD.Clear();
            DB.Clear();

            HD = 0;
            ARAD = Math.Abs((90 - ANGLE) * Math.PI / 180);
            VL = VISL * 0.001 / DENL;
            VG = VISG * 0.001 / DENG;

            do
            {
                System.Diagnostics.Debug.WriteLine("HD = " + HD);
                HD = HD + 0.025;
                HLD = 2 * HD - 1;
                SI = Math.Sqrt(1 - HLD * HLD);
                AL = 0.25 * (Math.PI - Math.Acos(HLD) + HLD * SI);
                A = Math.PI / 4;
                AG = A - AL;
                SG = Math.Acos(HLD);
                SL = Math.PI - SG;
                UL = A / AL;
                UG = A / AG;
                DL = 4 * AL / SL;
                DG = 4 * AG / (SG + SI);

                // Calculate SS line

                double C2 = 1 - HD;
                double UGS = Math.Sqrt(C2 * C2 * AG * (DENL - DENG) * DIA * 9.81 * Math.Cos(ARAD) / (UG * UG * SI * DENG));
                double ULS = FINDULS(UGS, 1);

                if (UGS >= 0.03)
                {
                    SS.Add(new XY { x = UGS, y = ULS });
                }

                // DB Line

                double CL = 0.046;
                double N = 0.2;

                ULS = Math.Pow(4 * AG * DIA * 9.81 * Math.Cos(ARAD) * (1 - DENG / DENL) / (SI * CL * UL * UL * Math.Pow(DL * DIA * UL / VL, -N)), 1 / (2 - N));
                UGS = FINDUGS(ULS, 1);
                if (HD > 0.5)
                {
                    DB.Add(new XY { x = UGS, y = ULS });
                }

                // SW Line
                double SSMAX = SS.Max(c => c.y);

                ULS = 1;
                double EPS = 1e10;
                int it = 0;
                do
                {
                    UGS = Math.Sqrt(4 * VL * 9.81 * (DENL - DENG) * Math.Cos(ARAD) / (0.01 * DENG * ULS * UL * UG * UG));
                    double ULS1 = ULS;
                    ULS = FINDULS(UGS, 1);
                    EPS = Math.Abs(ULS - ULS1);
                    it++;
                }
                while (EPS > 1e-5 && it < 100);

                if (ULS >= 0.003)
                    if (ULS <= SSMAX)
                {
                    SW.Add(new XY { x = UGS, y = ULS });
                }

            }
            while (HD < 1.0);

            HD = 0.35;
            HLD = 2 * HD - 1;
            SI = Math.Sqrt(1 - HLD * HLD);
            AL = 0.25 * (Math.PI - Math.Acos(HLD) + HLD * SI);
            A = Math.PI / 4;
            AG = A - AL;
            SG = Math.Acos(HLD);
            SL = Math.PI - SG;
            UL = A / AL;
            UG = A / AG;
            DL = 4 * AL / SL;
            DG = 4 * AG / (SG + SI);

            double UGSS = Math.Sqrt((1 - HD) * (1 - HD) * AG * (DENL - DENG) * DIA * 9.81 * Math.Cos(ARAD) / (UG * UG * SI * DENG));
            double ULSS = FINDULS(UGSS, 1);


            double ULDB = Math.Pow(4 * AG * DIA * 9.81 * Math.Cos(ARAD) * (1 - DENG / DENL) / (SI * 0.046 * UL * UL * Math.Pow(DL * DIA * UL / VL, -0.2)), 1 / (2 - 0.2));
            double UGDB = FINDUGS(ULDB, 1);


            AD.Add(new XY { x = UGDB, y = ULDB });

            foreach (var item in DB)
            {
                if (item.x > UGSS)
                //    if (item.x < UGDB)
                    {
                        double ULS = FINDULS(item.x, 1);
                        AD.Add(new XY { x = item.x, y = ULS });
                    }
            }

             AD.Add(new XY { x = UGSS, y = ULSS });

      
        }
    }
}
