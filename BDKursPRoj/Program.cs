using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDKursPRoj
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class DET
    {
        #region Поля
        /// <summary>
        /// Код детали.
        /// </summary>
        public int CodeDET;
        /// <summary>
        /// Наименование детали.
        /// </summary>
        public string NumDET;
        /// <summary>
        /// Число зубьев шестерни.
        /// </summary>
        public int z1;
        /// <summary>
        /// Число зубьев колеса.
        /// </summary>
        public int z2;
        /// <summary>
        /// Передаточное число.
        /// </summary>
        public double u;
        /// <summary>
        /// Межосевое расстояние.
        /// </summary>
        public double aw;
        /// <summary>
        /// Угол наклона линии зуба по делительному цилиндру.
        /// </summary>
        public double β;
        /// <summary>
        /// Делительное межосевое расстояние.
        /// </summary>
        public double a;
        /// <summary>
        /// Угол профиля зуба.
        /// </summary>
        public double at;
        /// <summary>
        /// Угол зацепления.
        /// </summary>
        public double atw;
        /// <summary>
        /// Коэффициент смещения шестерни.
        /// </summary>
        public double x1;
        /// <summary>
        /// Коэффициент смещения колеса.
        /// </summary>
        public double x2;
        /// <summary>
        /// Делительный диаметр шестерни.
        /// </summary>
        public double d1;
        /// <summary>
        /// Делительный диаметр колеса.
        /// </summary>
        public double d2;
        /// <summary>
        /// Начальный диаметр шестерни.
        /// </summary>
        public double dw1;
        /// <summary>
        /// Начальный диаметр колеса.
        /// </summary>
        public double dw2;
        /// <summary>
        /// Диаметр вершин зубьев шестерни.
        /// </summary>
        public double da1;
        /// <summary>
        /// Диаметр вершин зубьев колеса.
        /// </summary>
        public double da2;
        /// <summary>
        /// Диаметр впадин шестерни.
        /// </summary>
        public double df1;
        /// <summary>
        /// Диаметр впадин колеса.
        /// </summary>
        public double df2;
        #endregion

        public void InitVal1(int i, int z1, int z2, double β, double m, int Zad, double x1, double x2, double aw, int Pdch)
        {
            CodeDET = i;
            NumDET = CodeDET.ToString();
            this.z1 = z1;
            this.z2 = z2;
            u = (double)z1 / z2;
            this.β = β;
            a = (z1 + z2) * m * 2 * Math.Cos(ToRad(β));
            at = ToDeg(Math.Atan(Math.Tan(ToRad(20)) / Math.Cos(ToRad(β))));
            if(Zad == 0)
            {
                this.x1 = x1;
                this.x2 = x2;
                atw = ATW1();
                this.aw = AW();
            }
            else
            {
                this.aw = aw;
                atw = ATW2();
                double X = XValue();
                this.x1 = X1(X, Pdch);
                this.x2 = X2(X, Pdch);
            }

            d1 = z1 * m / Math.Cos(ToRad(β));
            d2 = z2 * m / Math.Cos(ToRad(β));

            dw1 = 2 * this.aw / (u + 1);
            dw2 = 2 * this.aw * u / (u + 1);

            df1 = d1 - 2 * (1.25 - this.x1) * m;
            df2 = d2 - 2 * (1.25 - this.x2) * m;
        }

        public static double ToRad(double val) => val / 180 * Math.PI;
        public static double ToDeg(double val) => val / Math.PI * 180;

        public double INV(double val) => Math.Tan(ToRad(val)) - ToRad(val);

        public double ATW1()
        {
            double ATWValue = 2 * (x1 + x2) * Math.Tan(ToRad(20)) / (z1 + z2) + INV(at);

            double xn = 1.441 * Math.Pow(ATWValue, 1 / 3) - 0.374 * ATWValue;
            double xp = 0;
            while (Math.Abs(xp - xn) > 0.00001)
            {
                xp = xn;
                xn = xp + (ATWValue - INV(ToDeg(xp))) / Math.Pow(Math.Tan(xp), 2);
            }
            return xn;
        }

        public double AW() => a * Math.Cos(ToRad(at)) / Math.Cos(ToRad(atw));

        public double ATW2()
        {
            if (a / aw * Math.Cos(ToRad(at)) <= 1)
                return ToDeg(Math.Acos(a / aw * Math.Cos(ToRad(at))));
            else
                return at;
        }

        public double XValue()
        {
            if((z1 + z2) * (INV(atw) - INV(at)) <= (2 * Math.Tan(ToRad(20))))
                return (z1 + z2) * (INV(atw) - INV(at)) / (2 * Math.Tan(ToRad(20)));
            else
                return 1;
        }

        public double X1(double X, int Pdch)
        {
            if(Pdch == 0)
            {
                if (X > 0 && X <= 0.5)
                    return X;
                if (X > 0.5 && X <= 1)
                    return 0.5;
            }
            return X;
        }
        public double X2(double X, int Pdch)
        {
            if (Pdch == 0)
            {
                if (X > 0 && X <= 0.5)
                    return 0;
                if (X > 0.5 && X <= 1)
                    return X - 0.5;
            }
            return 0;
        }

        public void InitVal2(double dy, double m)
        {
            da1 = d1 + 2 * (1 + x1 - dy) * m;
            da2 = d2 + 2 * (1 + x2 - dy) * m;
        }

        public void ChangeIndex(int i)
        {
            CodeDET = i;
            NumDET = CodeDET.ToString();
        }
    }

    class SE
    {
        #region Поля
        /// <summary>
        /// Код сборочной единицы.
        /// </summary>
        public int CodeSE;
        /// <summary>
        /// Наименование сборочной единицы.
        /// </summary>
        public string NamSE;
        /// <summary>
        /// Модуль передачи.
        /// </summary>
        public double m;
        /// <summary>
        /// Задано.
        /// </summary>
        public int Zad;
        /// <summary>
        /// Тип передачи.
        /// </summary>
        public int Pdch;
        /// <summary>
        /// Коэффициент суммы смещений.
        /// </summary>
        public double X;
        /// <summary>
        /// Коэффициент воспринимаемого смещения.
        /// </summary>
        public double y;
        /// <summary>
        /// Коэффициент уравнительного смещения.
        /// </summary>
        public double Δy;
        /// <summary>
        /// Детали.
        /// </summary>
        public DET det = new DET();
        #endregion

        public void InitVal(int i, double m, int Zad, int Pdch, int z1, int z2, double β, double x1, double x2, double aw)
        {
            CodeSE = i;
            NamSE = CodeSE.ToString();
            this.m = m;
            this.Zad = Zad;
            this.Pdch = Pdch;
            det.InitVal1(i, z1, z2, β, m, Zad, x1, x2, aw, Pdch);
            X = det.x1 + det.x2;
            y = (det.aw - DET.ToRad(det.a)) / m;
            Δy = X - y;
            det.InitVal2(Δy, m);
        }
        public void ChangeIndex(int i)
        {
            CodeSE = i;
            NamSE = CodeSE.ToString();
            det.ChangeIndex(i);
        }
    }

    class UZ
    {
        #region Поля
        /// <summary>
        /// Код виртуального узла.
        /// </summary>
        public int CodeUZ;
        /// <summary>
        /// Наименование виртуального узла.
        /// </summary>
        public string NamUZ;
        /// <summary>
        /// Количество передач.
        /// </summary>
        public int KOL;
        /// <summary>
        /// Счетчик цикла.
        /// </summary>
        public int i;
        /// <summary>
        /// Массив геометрических параметров для всех цилиндрических передач.
        /// </summary>
        public List<SE> se = new List<SE>();
        #endregion
        public UZ()
        {
            CodeUZ = 0;
            NamUZ = CodeUZ.ToString();
            KOL = 0;
            i = 0;
        }

        public void Add(double m, int Zad, int Pdch, int z1, int z2, double β, double x1, double x2, double aw)
        {
            SE tmp = new SE();
            tmp.InitVal(i++, m, Zad, Pdch, z1, z2, β, x1, x2, aw);
            se.Add(tmp);
            ++KOL;
        }
        public void Change(int index, double m, int Zad, int Pdch, int z1, int z2, double β, double x1, double x2, double aw)
        {
            se[index].InitVal(se[index].CodeSE, m, Zad, Pdch, z1, z2, β, x1, x2, aw);
        }

        public void Delete(int index)
        {
            se.RemoveAt(index);
            --KOL;
        }

        public List<SE> GetSortFiltSEs(int first, int second, int ff, int fz, double fv, int sf, int sz, double sv, int tf, int tz, double tv)
        {
            IEnumerable<SE> tmp = se;
            switch (second)
            {
                case 0: { tmp = se.OrderBy(item => item.CodeSE); break; }
                case 1: { tmp = se.OrderBy(item => item.det.a); break; }
                case 2: { tmp = se.OrderBy(item => item.det.at); break; }
                case 3: { tmp = se.OrderBy(item => item.det.atw); break; }
                case 4: { tmp = se.OrderBy(item => item.X); break; }
                case 5: { tmp = se.OrderBy(item => item.det.u); break; }
                case 6: { tmp = se.OrderBy(item => item.det.dw1); break; }
                case 7: { tmp = se.OrderBy(item => item.det.dw2); break; }
                case 8: { tmp = se.OrderBy(item => item.y); break; }
                case 9: { tmp = se.OrderBy(item => item.Δy); break; }
                case 10: { tmp = se.OrderBy(item => item.det.df1); break; }
                case 11: { tmp = se.OrderBy(item => item.det.df2); break; }
                case 12: { tmp = se.OrderBy(item => item.det.x1); break; }
                case 13: { tmp = se.OrderBy(item => item.det.x2); break; }
                case 14: { tmp = se.OrderBy(item => item.det.aw); break; }
                case 15: { tmp = se.OrderBy(item => item.det.d1); break; }
                case 16: { tmp = se.OrderBy(item => item.det.d2); break; }
                case 17: { tmp = se.OrderBy(item => item.det.da1); break; }
                case 18: { tmp = se.OrderBy(item => item.det.da2); break; }
            }
            switch (first)
            {
                case 0: { tmp = tmp.OrderBy(item => item.CodeSE); break; }
                case 1: { tmp = tmp.OrderBy(item => item.det.a); break; }
                case 2: { tmp = tmp.OrderBy(item => item.det.at); break; }
                case 3: { tmp = tmp.OrderBy(item => item.det.atw); break; }
                case 4: { tmp = tmp.OrderBy(item => item.X); break; }
                case 5: { tmp = tmp.OrderBy(item => item.det.u); break; }
                case 6: { tmp = tmp.OrderBy(item => item.det.dw1); break; }
                case 7: { tmp = tmp.OrderBy(item => item.det.dw2); break; }
                case 8: { tmp = tmp.OrderBy(item => item.y); break; }
                case 9: { tmp = tmp.OrderBy(item => item.Δy); break; }
                case 10: { tmp = tmp.OrderBy(item => item.det.df1); break; }
                case 11: { tmp = tmp.OrderBy(item => item.det.df2); break; }
                case 12: { tmp = tmp.OrderBy(item => item.det.x1); break; }
                case 13: { tmp = tmp.OrderBy(item => item.det.x2); break; }
                case 14: { tmp = tmp.OrderBy(item => item.det.aw); break; }
                case 15: { tmp = tmp.OrderBy(item => item.det.d1); break; }
                case 16: { tmp = tmp.OrderBy(item => item.det.d2); break; }
                case 17: { tmp = tmp.OrderBy(item => item.det.da1); break; }
                case 18: { tmp = tmp.OrderBy(item => item.det.da2); break; }
            }
            switch (ff)
            {
                case 1: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.CodeSE == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.CodeSE != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.CodeSE > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.CodeSE >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.CodeSE < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.CodeSE <= fv);
                        break;}
                case 2: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.a == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.a != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.a > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.a >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.a < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.a <= fv);
                        break;}
                case 3: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.at == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.at != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.at > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.at >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.at < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.at <= fv);
                        break;}
                case 4: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.atw == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.atw != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.atw > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.atw >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.atw < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.atw <= fv);
                        break; }
                case 5: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.X == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.X != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.X > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.X >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.X < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.X <= fv);
                        break; }
                case 6: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.u == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.u != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.u > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.u >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.u < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.u <= fv);
                        break; }
                case 7: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.dw1 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.dw1 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.dw1 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.dw1 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.dw1 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.dw1 <= fv);
                        break; }
                case 8: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.dw2 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.dw2 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.dw2 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.dw2 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.dw2 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.dw2 <= fv);
                        break; }
                case 9: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.y == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.y != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.y > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.y >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.y < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.y <= fv);
                        break; }
                case 10: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.Δy == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.Δy != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.Δy > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.Δy >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.Δy < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.Δy <= fv);
                        break; }
                case 11: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.df1 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.df1 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.df1 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.df1 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.df1 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.df1 <= fv);
                        break; }
                case 12: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.df2 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.df2 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.df2 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.df2 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.df2 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.df2 <= fv);
                        break; }
                case 13: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.x1 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.x1 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.x1 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.x1 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.x1 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.x1 <= fv);
                        break; }
                case 14: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.x2 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.x2 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.x2 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.x2 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.x2 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.x2 <= fv);
                        break; }
                case 15: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.aw == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.aw != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.aw > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.aw >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.aw < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.aw <= fv);
                        break; }
                case 16: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.d1 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.d1 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.d1 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.d1 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.d1 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.d1 <= fv);
                        break; }
                case 17: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.d2 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.d2 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.d2 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.d2 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.d2 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.d2 <= fv);
                        break; }
                case 18: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.da1 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.da1 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.da1 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.da1 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.da1 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.da1 <= fv);
                        break; }
                case 19: {
                        if (fz == 0)
                            tmp = tmp.Where(item => item.det.da2 == fv);
                        if (fz == 1)
                            tmp = tmp.Where(item => item.det.da2 != fv);
                        if (fz == 2)
                            tmp = tmp.Where(item => item.det.da2 > fv);
                        if (fz == 3)
                            tmp = tmp.Where(item => item.det.da2 >= fv);
                        if (fz == 4)
                            tmp = tmp.Where(item => item.det.da2 < fv);
                        if (fz == 5)
                            tmp = tmp.Where(item => item.det.da2 <= fv);
                        break; }
            }
            switch (sf)
            {
                case 1:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.CodeSE == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.CodeSE != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.CodeSE > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.CodeSE >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.CodeSE < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.CodeSE <= sv);
                        break;
                    }
                case 2:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.a == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.a != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.a > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.a >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.a < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.a <= sv);
                        break;
                    }
                case 3:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.at == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.at != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.at > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.at >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.at < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.at <= sv);
                        break;
                    }
                case 4:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.atw == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.atw != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.atw > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.atw >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.atw < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.atw <= sv);
                        break;
                    }
                case 5:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.X == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.X != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.X > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.X >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.X < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.X <= sv);
                        break;
                    }
                case 6:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.u == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.u != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.u > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.u >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.u < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.u <= sv);
                        break;
                    }
                case 7:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.dw1 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.dw1 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.dw1 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.dw1 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.dw1 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.dw1 <= sv);
                        break;
                    }
                case 8:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.dw2 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.dw2 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.dw2 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.dw2 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.dw2 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.dw2 <= sv);
                        break;
                    }
                case 9:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.y == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.y != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.y > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.y >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.y < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.y <= sv);
                        break;
                    }
                case 10:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.Δy == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.Δy != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.Δy > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.Δy >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.Δy < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.Δy <= sv);
                        break;
                    }
                case 11:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.df1 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.df1 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.df1 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.df1 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.df1 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.df1 <= sv);
                        break;
                    }
                case 12:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.df2 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.df2 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.df2 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.df2 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.df2 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.df2 <= sv);
                        break;
                    }
                case 13:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.x1 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.x1 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.x1 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.x1 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.x1 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.x1 <= sv);
                        break;
                    }
                case 14:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.x2 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.x2 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.x2 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.x2 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.x2 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.x2 <= sv);
                        break;
                    }
                case 15:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.aw == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.aw != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.aw > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.aw >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.aw < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.aw <= sv);
                        break;
                    }
                case 16:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.d1 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.d1 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.d1 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.d1 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.d1 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.d1 <= sv);
                        break;
                    }
                case 17:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.d2 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.d2 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.d2 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.d2 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.d2 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.d2 <= sv);
                        break;
                    }
                case 18:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.da1 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.da1 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.da1 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.da1 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.da1 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.da1 <= sv);
                        break;
                    }
                case 19:
                    {
                        if (sz == 0)
                            tmp = tmp.Where(item => item.det.da2 == sv);
                        if (sz == 1)
                            tmp = tmp.Where(item => item.det.da2 != sv);
                        if (sz == 2)
                            tmp = tmp.Where(item => item.det.da2 > sv);
                        if (sz == 3)
                            tmp = tmp.Where(item => item.det.da2 >= sv);
                        if (sz == 4)
                            tmp = tmp.Where(item => item.det.da2 < sv);
                        if (sz == 5)
                            tmp = tmp.Where(item => item.det.da2 <= sv);
                        break;
                    }
            }
            switch (tf)
            {
                case 1:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.CodeSE == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.CodeSE != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.CodeSE > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.CodeSE >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.CodeSE < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.CodeSE <= tv);
                        break;
                    }
                case 2:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.a == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.a != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.a > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.a >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.a < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.a <= tv);
                        break;
                    }
                case 3:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.at == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.at != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.at > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.at >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.at < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.at <= tv);
                        break;
                    }
                case 4:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.atw == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.atw != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.atw > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.atw >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.atw < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.atw <= tv);
                        break;
                    }
                case 5:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.X == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.X != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.X > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.X >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.X < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.X <= tv);
                        break;
                    }
                case 6:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.u == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.u != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.u > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.u >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.u < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.u <= tv);
                        break;
                    }
                case 7:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.dw1 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.dw1 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.dw1 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.dw1 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.dw1 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.dw1 <= tv);
                        break;
                    }
                case 8:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.dw2 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.dw2 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.dw2 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.dw2 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.dw2 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.dw2 <= tv);
                        break;
                    }
                case 9:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.y == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.y != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.y > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.y >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.y < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.y <= tv);
                        break;
                    }
                case 10:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.Δy == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.Δy != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.Δy > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.Δy >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.Δy < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.Δy <= tv);
                        break;
                    }
                case 11:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.df1 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.df1 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.df1 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.df1 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.df1 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.df1 <= tv);
                        break;
                    }
                case 12:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.df2 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.df2 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.df2 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.df2 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.df2 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.df2 <= tv);
                        break;
                    }
                case 13:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.x1 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.x1 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.x1 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.x1 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.x1 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.x1 <= tv);
                        break;
                    }
                case 14:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.x2 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.x2 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.x2 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.x2 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.x2 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.x2 <= tv);
                        break;
                    }
                case 15:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.aw == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.aw != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.aw > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.aw >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.aw < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.aw <= tv);
                        break;
                    }
                case 16:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.d1 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.d1 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.d1 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.d1 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.d1 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.d1 <= tv);
                        break;
                    }
                case 17:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.d2 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.d2 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.d2 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.d2 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.d2 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.d2 <= tv);
                        break;
                    }
                case 18:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.da1 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.da1 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.da1 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.da1 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.da1 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.da1 <= tv);
                        break;
                    }
                case 19:
                    {
                        if (tz == 0)
                            tmp = tmp.Where(item => item.det.da2 == tv);
                        if (tz == 1)
                            tmp = tmp.Where(item => item.det.da2 != tv);
                        if (tz == 2)
                            tmp = tmp.Where(item => item.det.da2 > tv);
                        if (tz == 3)
                            tmp = tmp.Where(item => item.det.da2 >= tv);
                        if (tz == 4)
                            tmp = tmp.Where(item => item.det.da2 < tv);
                        if (tz == 5)
                            tmp = tmp.Where(item => item.det.da2 <= tv);
                        break;
                    }
            }
            return tmp.ToList();
        }
    }
}
