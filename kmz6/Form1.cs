using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kmz6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random rnd = new Random();
        string Crypt(long n, long exp, string text)
        {
            double block = (double)(text.Length*3)/ n.ToString().Length;
            block = Math.Ceiling(block);
            byte[] By = Encoding.GetEncoding(1251).GetBytes(text);
            string[] ByStr = new string[By.Length];
            for (int z = 0; z < By.Length; z++)
            {
                ByStr[z] = By[z].ToString();
            }
            for (int z = 0; z < By.Length; z++)
            {
                while(ByStr[z].Length < 3)
                    ByStr[z] = "0" + By[z].ToString();
            }
            string txt = "";
            for (int z = 0; z < ByStr.Length; z++)
            {
                txt += ByStr[z];
            }
            List<long> B = new List<long>();
            string str = "";
            for (int i = 0; i < ByStr.Length; i++)
            {
                if ((str + ByStr[i]).Length > n.ToString().Length)
                {
                    B.Add(Convert.ToInt64(str));
                    str = ByStr[i];
                }
                else
                {
                    str += ByStr[i];
                    if (i == ByStr.Length - 1)
                        B.Add(Convert.ToInt64(str));
                }
            }
            //int t = 0;
            //for (int i = 0; i < txt.Length; i++)
            //{
            //    if (Convert.ToInt64(str + txt[i]) >= n || (str + txt[i]).Length > n.ToString().Length)
            //    {
            //        B.Add(Convert.ToInt64(str));
            //        t++;
            //        str = txt[i].ToString();
            //    }
            //    else
            //    {
            //        str += txt[i];
            //        if (i == txt.Length - 1)
            //            B.Add(Convert.ToInt64(str));
            //    }
            //}
            
            long[] M = new long[B.Count];
            for (int z = 0; z < M.Length; z++)
            {
                M[z] = (long) BigInteger.ModPow(B[z],exp,n);
            }
            string shifr = "";
            for (int z = 0; z < M.Length; z++)
            {
                shifr += M[z].ToString("x");
                if (z != M.Length - 1)
                    shifr += "-";
            }
            return shifr;
        }
        string Decrypt(long n, long d, string shifr)
        {
            string [] Btxt = shifr.Split('-');
            //long [] M = new string[Btxt.Length];
            string text = "";
            //string str = "";
            string str1 = "";
            for (int i = 0; i < Btxt.Length; i++)
            {
                string str = BigInteger.ModPow(Convert.ToInt64(Btxt[i],16),d,n).ToString();
                if (str.Length % 3 != 0)
                    str = "0" + str;
                str1 += str;
            }
            //for (int i = 0; i < M.Length; i++)
            //{
            //    str += M[i].ToString();
            //    if(i != M.Length - 1)
            //        if (str.Length < n.ToString().Length && Convert.ToInt64(str+M[i+1].ToString()[0]) < n)
            //            str = "0" + str;
            //    str1 += str;
            //    str = "";
            //}
            //int t = 0;
            byte[] by = new byte[str1.Length/3];
            for (int i = 0; i < by.Length; i++)
            {
                //string s = "";
                //int a = 3;
                //if (Convert.ToInt32(Convert.ToString(str1[i * 3 - t]) + Convert.ToString(str1[i * 3 - t + 1]) + Convert.ToString(str1[i * 3 - t + 2])) > 255)
                //{ a = 2; t++; }
                //for (int j = 0; j < a; j++)
                //{
                //    s += str1[(i*3)-t+j];
                //}
                by[i] = Convert.ToByte(str1.Substring(i*3,3));
            }
            text = Encoding.GetEncoding(1251).GetString(by);
            return text;
        }
        void Key(int bit)
        {
            long p = JustGen(bit);
            long q = JustGen(bit);
            long n = p * q;
            long fn = eiler(p, q);
            long exp = RandomExp(fn);
            long d =  Obratn(exp,fn);
            textBox3.Text = p.ToString();
            textBox4.Text = q.ToString();
            textBox5.Text = d.ToString();
            textBox6.Text = exp.ToString();
            textBox7.Text = n.ToString();
            textBox8.Text = n.ToString();
        }
        long Obratn(long a, long m)
        {
            long x, y;
            long g = GCD(a, m, out x, out y);
            if (g != 1)
                throw new ArgumentException();
            return (x % m + m) % m;
        }
        long GCD(long a, long b, out long x, out long y) // Для нахождения обратного числа по модулю
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            long x1, y1;
            long d = GCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }
        long RandomExp(long x)
        {
            string str = x.ToString();
            string str1;
            long d;
            do
            {
                str1 = "";
                for (int i = 0; i < str.Length; i++)
                    str1 += rnd.Next(0, 10).ToString();
                d = Convert.ToInt64(str1);
            }
            while (d >= x || NOD(d, x) != 1);
            return d;
        }
        long JustGen(int bit) // Генерация бит для простого числа
        {
            string proBin = "";
            for (int i = 0; i < bit; i++)
            {
                proBin += rnd.Next(0, 2);
            }
            proBin = "1" + proBin;
            long x = Convert.ToInt64(proBin, 2);
            if (x % 2 == 0)
                x++;
            x = just(x);
            return x;
        }
        long just(long x) // Проверка числа на простоту и 
        {
            int[] er = Eratosfen(256);
            for (int i = 1; i < er.Length; i++)
            {
                if (x % er[i] == 0)
                {
                    x += 2;
                    i = 0;
                }
            }
            bool b = MilRab(x);
            while (b == false)
            {
                x++;
                b = MilRab(x);
            }
            return x;
        }
        bool MilRab(long n) // Тест Миллера-Рабина на простое число
        {
            long a, s = 0;
            long k = Convert.ToInt32(Math.Log(n,2));
            long t = n - 1;// Convert.ToInt32((n - 1) / (Math.Pow(2,s)));
            while (t % 2 == 0)
            {
                t /= 2;
                s++;
            }
            for (int i = 0; i < k; i++)
            {
                a = rnd.Next(2,(int) n-1);
                BigInteger o = BigInteger.Pow(48,47);
                o = BigInteger.ModPow(20*o,1,53);
                long x = (long) BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int j = 0; j < s-1; j++)
                {
                    x = (long) BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if(x != n-1)
                    return false;
            }
            return true;
        }
        string SpM(int a, int t, int n) // Возведение в степень по модулю
        {
            //double tmp = a;
            //string step = Convert.ToString(t, 2)+"1";
            //int[] arr = new int[step.Length-2];
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    if (step[i+1].ToString() == "0")
            //        tmp = Math.Pow(tmp, 2)%n;
            //    else
            //        tmp = (Math.Pow(tmp, 2)*a)%n;
            //}
            return "";
        }
        long NOD(long a, long b)
        {
            while (b != 0)
                b = a % (a = b);
            return a;
        }
        long eiler(long p, long q)
        {
            long f = (p - 1) * (q - 1);
            return f;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            int bit = rnd.Next(23,27);
            Key(bit);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            long n = Convert.ToInt64(textBox7.Text);
            long exp = Convert.ToInt64(textBox5.Text);
            string text = textBox1.Text;
            textBox2.Text = Crypt(n,exp,text);
        }


        int [] Eratosfen(int n) // решето Эратосфена: выводит массив простых чисел до n
        {
            bool[] A = new bool[n+1];
            int z = 0;
            for (int i = 2; (i * i) <= n; i++)
                if (A[i]==false)
                    for (int j = i * i; j <= n; j += i)
                    {
                        if (A[j] == false)
                        {
                            A[j] = true;
                            z++;
                        }
                    }
            int[] B = new int[n-z-1];
            z = 0;
            for (int i = 2; i < n+1; i++)
            {
                if (A[i]==false)
                {
                    B[z] = i;
                    z++;
                }
            }
            return B;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            long n = Convert.ToInt64(textBox8.Text);
            long d = Convert.ToInt64(textBox6.Text);
            string shifr = textBox1.Text;
            textBox2.Text = Decrypt(n, d, shifr);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
            textBox2.Clear();
        }
    }
}
