using System.Globalization;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace KompilatorF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }        

        private void button1_Click(object sender, EventArgs e)
        {
            string vvod = richTextBox1.Text;
            var komp = new Kompilator(vvod);            
            richTextBox1.Clear();                        
            richTextBox3.Clear();
            if (komp.N == 0)
            {
                richTextBox1.Text = komp.s;
                foreach (KeyValuePair<string, float> per in komp.Peremennue)
                {
                    richTextBox3.Text += $"�������� {per.Key} : {per.Value}\n";
                    //richTextBox3.Text += $"�������� {per.Key} � ����������������� �������: {komp.FloatToHex(per.Value)}\n";
                }
                richTextBox3.Text += $"��� ������: {komp.N}\n";
            }
            else
            {
                richTextBox1.Text = komp.s;
                richTextBox3.Text += komp.Oshibki[komp.N] + "\n";
                richTextBox3.Text += $"��� ������: {komp.N}\n";
                richTextBox3.Text += $"������: { komp.kursor}\n";
                richTextBox1.SelectionStart = komp.kursor;
                richTextBox1.SelectionLength = 1;
                richTextBox1.SelectionBackColor = Color.Red;
            }            
        }
    }


    public class Kompilator
    {
        public int kursor = 0;
        public int N = 0;
        public int G = 0;
        public string s = "";
        public string S = "";

        void Probely(ref string s, ref int k)
        {
            while (s[k] == ' ' | s[k] == '\n')
            {
                k++;
            }
        }
        public /*static*/ string FloatToHex(float num)
        {
            int whole = (int)num;
            float frac = num - whole;
            string wholeHex = whole.ToString("X");
            string fracHex = "";
            if (frac > 0)
            {
                fracHex = ".";
                while (frac > 0)
                {
                    frac *= 16;
                    int digit = (int)frac;
                    frac -= digit;
                    fracHex += digit.ToString("X");
                }
            }
            else
            {
                fracHex = ".0";
            }

            return wholeHex + fracHex;
        }

        public Dictionary<int, string> Oshibki = new Dictionary<int, string>()
        {
            { 0, "��� ������"},
            { 1, "������ � ���������, ������������ ������"},
            { 2, "������ � ��������� �����, ������������ ������"},
            { 3, "������ � ��������� �����, ������������ ������"},
            { 4, "������ � ��������� �����, ��������� ������ ����� �����"},
            { 5, "������, ���������� ������ ���������� � �����"},
            { 6, "������ � ��������� �����, ������������ ������" },
            { 7, "������, ��������� ���������� ������� ����������� 3 � ������� '['" },
            { 8, "������, �������������� ������ ������ ')'" },
            { 9, "������, �������������� ������ ������ ']'" },
            { 10, "������, �� ����� ���� 2 ����� ������" },
            { 11, "������, ����� ��������� �� ����������" },
            { 12, "������ � ��������� ����������, ������������ ������"},
            { 13, "������, �� ���� ������ ������"},
            { 14, "������, ��������� '=', �������� �������� ������ � ����������"},
            { 15, "������, ��������� ������ ���������� �� ���� 'First' ��� 'Second'"},
            { 16, "������ � ����� 'First' ��� 'Second'"},
            { 17, "������, ����������� ������ ���������� ���� �� ����� 'real', ���� 'integer'"},
            { 18, "������ ���� � ����� 'real', ���� 'integer'"},
            { 19, "������, ����������� ������ ':'"},
            { 20, "������, ��� ������������� ������ 'END'"},
            { 21, "������, ����� ����� �� ����� ���� ������ ')'"},
            { 22, "������, ����� ����� �� ����� ���� ������ ']'"},
            { 23, "������, ������ �������� ���������� ������ ���� �����"},
            { 24, "������, �������� ������ ';'"},
            { 25, "������ � ���������, ������ ���� ����� �����" },
            { 26, "������, ��� ���������� ������ 'BEGIN'"},
            { 27, "������, ������� ����� ��������� ����"},
        };
        public Dictionary<string, float> Peremennue = new Dictionary<string, float>()
        {

        };

        char[] alfavit = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                           'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        char[] arifmetica = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        public string Seloe(ref string s, ref int k)
        {
            string r = "";
            int i;
            for (i = k; i < s.Length; i++)
            {
                int flag = 0;
                for (int j = 0; j < arifmetica.Length; j++)
                {
                    if (s[i] == arifmetica[j])
                    {
                        flag = 1;
                        break;
                    }
                }

                if (flag == 1)
                {
                    r += s[i];
                }
                else
                {
                    break;
                }
            }
            k += (i - k);
            return r;
        }

        public float Vezhestvennoe(ref string s, ref int k, ref int N)
        {
            int g = k;
            var p = Seloe(ref s, ref k);
            if (p != "")
            {
                if (s[k] == '.')
                {
                    k++;
                    var v = Seloe(ref s, ref k);
                    if (v != "")
                    {
                        // ����������� ����� �����
                        int integerPart = int.Parse(p, NumberStyles.HexNumber);

                        // ����������� ������� �����
                        double fractionalPart = 0;
                        int digit = int.Parse(v, NumberStyles.HexNumber);
                        fractionalPart += digit / Math.Pow(16, v.Length);


                        // ���������� ����� � ������� �����
                        float number = (float)integerPart + (float)fractionalPart;
                        float result = number;
                        N = 0;
                        return result;
                    }
                    else
                    {
                        if (s[k] == ':')
                        {
                            N = 23;
                            return 0;
                        }
                        N = 6;
                        return 0;
                    }
                }
                else
                {
                    if (s[k] == ':')
                    {
                        N = 23;
                        return 0;
                    }
                    N = 2;
                    return 0;
                }
            }
            else
            {
                if (s[k] == ':')
                {
                    N = 23;
                    return 0;
                }
                N = 1;
                return 0;

            }
        }

        /*public string Metka(ref string s, ref int k, ref int N)
        {
            int nach = k;
            var p = Seloe(ref s, ref k);
            Probely(ref s, ref k);
            if (alfavit.Contains(s[k]))
            {
                return "";
            }
            else if (s[k] != ':' ^ p == "")
            {
                N = 19;
                return "error";
            }
            else if (arifmetica.Contains(s[k + 1]))
            {
                N = 3;
                return "error";
            }
            else if (s[k] != ':')
            {
                N = 4;
                return "error";
            }
            else
            {
                k++;
                Probely(ref s, ref k);
                N = 0;
                string rez = s.Substring(nach, k);
                return rez;
            }
        }*/

        public string Peremennaa(ref string s, ref int k, ref int N)
        {

            int flag = 0;
            string nazvanie = string.Empty;
            for (int j = 0; j < alfavit.Length; j++)
            {
                if (s[k] == alfavit[j])
                {
                    flag = 1;
                    nazvanie += s[k];
                    k++;
                    break;
                }
            }
            if(flag == 0)
            {
                N = 5;
                return "error";
            }
            flag = 0;
            for (int j = 0; j < arifmetica.Length; j++)
            {
                if (s[k] == arifmetica[j])
                {
                    flag = 1;
                    nazvanie += s[k];
                    k++;
                    break;
                }
            }
            if (flag == 0)
            {
                N = 23;
                return "error";
            }
            int i;
            for (i = k; i < s.Length; i++)
            {
                flag = 0;
                for (int j = 0; j < arifmetica.Length; j++)
                {
                    if (s[i] == arifmetica[j])
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    nazvanie += s[i];
                    continue;
                }
                flag = 0;
                for (int j = 0; j < alfavit.Length; j++)
                {
                    if (s[i] == alfavit[j])
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    nazvanie += s[i];
                    continue;
                }
                else
                {
                    break;
                }
            }
            k += (i - k);               
            N = 0;
            return nazvanie;
        }            
        

        public float Blok3(ref string s, ref int k, ref int N)
        {
            float rez = 0;
            int g = k;
            switch (s[k])
            {
                case '(':
                    k++;
                    Probely(ref s, ref k);
                    var pr = PravayaChast(ref s, ref k, ref N);
                    if (N == 0)
                    {
                        rez = pr;
                        Probely(ref s, ref k);
                        if (s[k] != ')')
                        {
                            N = 8;
                            return rez;
                        }
                        k++;
                        Probely(ref s, ref k);
                        return rez;
                    }
                    else
                    {
                        rez = 0;
                        return rez;
                    }
                case '[':
                    G++;
                    if (G > 3)
                    {
                        N = 7;
                        rez = 0;
                        return rez;
                    }
                    else
                    {
                        k++;
                        Probely(ref s, ref k);
                        var pr1 = PravayaChast(ref s, ref k, ref N);
                        if (N == 0)
                        {
                            rez = pr1;
                            Probely(ref s, ref k);
                            if (s[k] != ']')
                            {
                                N = 9;
                                return rez;
                            }
                            G--;
                            k++;
                            Probely(ref s, ref k);
                            return rez;
                        }
                        else
                        {
                            rez = 0;
                            return rez;
                        }
                    }
            }
            var per = Peremennaa(ref s, ref k, ref N);
            if (N == 0)
            {
                if (Peremennue.ContainsKey(per))
                {
                    rez = Peremennue[per];
                    Probely(ref s, ref k);
                    return rez;
                }
                else
                {
                    if (per == "������")
                    {
                        N = 23;
                        k = g;
                        return 0;
                    }
                    Probely(ref s, ref k);
                    if (s[k] == '=')
                    {
                        N = 23;
                        k = g;
                        return 0;
                    }
                    N = 11;
                    rez = 0;
                    return rez;
                }
            }
            var vezh = Vezhestvennoe(ref s, ref k, ref N);
            if (N == 0)
            {
                rez = vezh;
                Probely(ref s, ref k);
                return rez;
            }
            if (N == 2 | N == 6 | N == 5)
            {
                return rez;
            }
            else
            {
                switch (s[k])
                {
                    case '+':
                        N = 10;
                        return rez;

                    case '-':
                        N = 10;
                        return rez;

                    case ')':
                        N = 21;
                        return rez;

                    case ']':
                        N = 22;
                        return rez;
                }

                if (s[k] == ':')
                {
                    k = g;
                    N = 23;
                    return 0;
                }
                N = 1;
                return rez;
            }
        }

        public float Blok2(ref string s, ref int k, ref int N)
        {
            var rez = Blok3(ref s, ref k, ref N);
            if (N != 0)
            {
                return rez;
            }
            Probely(ref s, ref k);
            while (s[k] == '^')
            {
                k++;
                Probely(ref s, ref k);
                var rez2 = Blok3(ref s, ref k, ref N);
                if (N != 0)
                {
                    return rez;
                }
                rez = (float)Math.Pow(rez, rez2);
                Probely(ref s, ref k);
            }
            return rez;
        }

        public float Blok1(ref string s, ref int k, ref int N)
        {
            var rez = Blok2(ref s, ref k, ref N);
            if (N != 0)
            {
                return rez;
            }
            Probely(ref s, ref k);
            while (s[k] == '*' | s[k] == '/')
            {
                switch (s[k])
                {
                    case '*':
                        k++;
                        Probely(ref s, ref k);
                        var rez1 = Blok2(ref s, ref k, ref N);
                        if (N != 0)
                        {
                            return rez;
                        }
                        rez *= rez1;
                        Probely(ref s, ref k);
                        break;

                    case '/':
                        k++;
                        Probely(ref s, ref k);
                        var rez2 = Blok2(ref s, ref k, ref N);
                        if (N != 0)
                        {
                            return rez;
                        }
                        if (rez2 == 0)
                        {
                            N = 13;
                            return rez;
                        }
                        rez /= rez2;
                        Probely(ref s, ref k);
                        break;
                }
            }
            return rez;
        }

        public float PravayaChast(ref string s, ref int k, ref int N)
        {
            float rez = 0;
            int fla = 0;
            int f = 0;
            if (s[k] != '-')
            {
                s = s.Insert(k, "+");
                fla = 1;
                f = k;
            }
            do
            {
                switch (s[k])
                {
                    case '+':
                        k++;
                        Probely(ref s, ref k);
                        var rez1 = Blok1(ref s, ref k, ref N);
                        if (N != 0)
                        {
                            if (fla == 1)
                            {
                                s = s.Remove(f, 1);
                                k--;
                            }
                            return rez;
                        }
                        rez += rez1;
                        Probely(ref s, ref k);
                        break;

                    case '-':
                        k++;
                        Probely(ref s, ref k);
                        var rez2 = Blok1(ref s, ref k, ref N);
                        if (N != 0)
                        {
                            if (fla == 1)
                            {
                                s = s.Remove(f, 1);
                                k--;
                            }
                            return rez;
                        }
                        rez -= rez2;
                        Probely(ref s, ref k);
                        break;
                }                               
            }
            while (s[k] == '+' | s[k] == '-');
            if (fla == 1)
            {
                s = s.Remove(f, 1);
                k--;
            }
            return rez;
        }

        public float Operator(ref string s, ref int k, ref int N)
        {
            float rez = 0;
            //var metka = Metka(ref s, ref k, ref N);
            if (s[k] == ';')
            {
                k++;
            }
            Probely(ref s, ref k);
            string p = Seloe(ref s, ref k);
            if (p == "")
            {
                N = 4;
                return rez;
            }
            Probely(ref s, ref k);
            if (s[k] != ':')
            {
                N = 19;
                return rez;
            }
            k++;
            Probely(ref s, ref k);
            var perem = Peremennaa(ref s, ref k, ref N);
            if (N != 0)
            {
                return rez;
            }
            Probely(ref s, ref k);
            if (s[k] != '=')
            {
                N = 14;
                return rez;
            }
            k++;
            Probely(ref s, ref k);
            var PrCh = PravayaChast(ref s, ref k, ref N);
            if (N != 0)
            {
                return rez;
            }
            if (Peremennue.ContainsKey(perem))
            {
                Peremennue[perem] = PrCh;
                Probely(ref s, ref k);
                return 1;
            }
            Peremennue.Add(perem, PrCh);
            Probely(ref s, ref k);
            return 1;
        }

        public float Mnozhestvo(ref string s, ref int k, ref int N)
        {
            if (arifmetica.Contains(s[k]))
            {
                N = 24;
                return 0; ;
            }           
            if (s[k] != 'F' && s[k] != 'S')
            {
                N = 15;
                return 0;
            }
            if (s.Substring(k, 5) != "First" && s.Substring(k, 6) != "Second")
            {
                N = 16;
                return 0;
            }
            if (s.Substring(k, 5) == "First")
            {
                k += 4;
                do
                {
                    k++;
                    Probely(ref s, ref k);
                    string p = Seloe(ref s, ref k);
                    if (p == "")
                    {
                        N = 25;
                        return 0;
                    }
                    Probely(ref s, ref k);
                }
                while (s[k] == ',');
            }
            if (s.Substring(k, 6) == "Second")
            {
                k += 5;
                do
                {
                    k++;
                    Probely(ref s, ref k);
                    Vezhestvennoe(ref s, ref k, ref N);
                    if (N != 0)
                    {
                        return 0;
                    }
                    Probely(ref s, ref k);
                }
                while (s[k] == ';');
            }
            return 1;
        }

        public float Opredelenie(ref string s, ref int k, ref int N)
        {
            if (s[k] != 'r' && s[k] != 'i')
            {
                N = 17;                
                return 0;
            }            
            if (s.Substring(k, 4) != "real" && s.Substring(k, 7) != "integer")
            {
                N = 18;
                return 0;
            }
            if (s.Substring(k, 4) == "real")
            {
                k += 3;
                do
                {
                    k++;
                    Probely(ref s, ref k);
                    Vezhestvennoe(ref s, ref k, ref N);
                    if (N != 0)
                    {
                        return 0;
                    }
                    Probely(ref s, ref k);
                }
                while (s[k] == ';');
            }
            if (s.Substring(k, 7) == "integer")
            {
                k += 6;
                do
                {
                    k++;
                    Probely(ref s, ref k);
                    string p = Seloe(ref s, ref k);
                    if (p == "")
                    {
                        N = 25;
                        return 0;
                    }
                    Probely(ref s, ref k);
                }
                while (s[k] == ',');
            }           
            return 1;
        }

        public bool Kompil()
        {
            s += "      k";
            Probely(ref s, ref kursor);
            if (s.Substring(kursor, 5) != "BEGIN")
            {
                N = 26;
                return false;
            }
            kursor += 5;
            Probely(ref s, ref kursor);

            do
            {
                var Opred = Opredelenie(ref s, ref kursor, ref N);
                if (N != 0)
                {
                    return false;
                }
                Probely(ref s, ref kursor);
            }
            while (s.Substring(kursor, 4) == "real" | s.Substring(kursor, 7) == "integer");

            do
            {
                var Oper = Operator(ref s, ref kursor, ref N);
                if (N != 0)
                {
                    return false;
                }
                Probely(ref s, ref kursor);
            }
            while (s[kursor] == ';');

            do
            {
                var Mnozh = Mnozhestvo(ref s, ref kursor, ref N);
                if (N != 0)
                {
                    return false;
                }
                Probely(ref s, ref kursor);
            }
            while (s.Substring(kursor, 3) != "END");

            if (s.Substring(kursor, 3) != "END")
            {
                N = 20;
                return false;
            }
            kursor += 3;
            Probely(ref s, ref kursor);
            if (s[kursor] != 'k')
            {
                N = 27;
                return false;
            }
            return true;

        }

        public Kompilator(string s)
        {
            this.S = s;
            this.s = s;
            var Komp = Kompil();
            this.s = this.s[..^7];            

            /*if (N == 0)
            {
                Console.Clear();
                Console.WriteLine(this.s);
                foreach (KeyValuePair<string, float> per in Peremennue)
                {
                    Console.WriteLine($"�������� {per.Key} � ���������� �������: {per.Value}");
                    Console.WriteLine($"�������� {per.Key} � ����������������� �������: {FloatToHex(per.Value)}");
                }
                Console.WriteLine($"������: {kursor} ");
            }
            else
            {
                Console.Clear();
                Console.WriteLine(this.s);
                Console.WriteLine(Oshibki[N]);
                Console.WriteLine($"��� ������: {N}");
                Console.WriteLine($"������: {kursor} {this.s[kursor - 1]} {this.s[kursor]} {this.s[kursor + 1]}");
            }*/
        }
    }
}


