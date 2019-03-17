using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Lab4
{
    public partial class Form1 : Form
    {
        char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И','Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С','Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ','Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7','8', '9', '0' };
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            for (long i=2; i<n; i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }
        private long Calculate_d(long m)
        {
            long d = m - 1;
            for (long i=2; i<=m; i++)
                if ((m%i==0)&&(d%i==0))//если имеются общие делители
                {
                    d--;
                    i = 1;
                }
            return d;
        }
        private long Calculate_e(long d, long m)
        {
            long e = 10;
            while (true)
            {
                if ((e * d) % m == 1) break;
                else e++;
            }
            return e;
        }
        private string perevod(long temp)
        {
            long temp1 = 0;
            int count = 0;
            StringBuilder s = new StringBuilder();
            while (temp > 0)
            {
                temp1 = temp % 2;
                temp = temp / 2;
                s.Append(temp1.ToString());
                count++;
            }
            if (count < 8)
                while (count != 8)
                {
                    s.Append("0");
                    count++;
                }
            string str = s.ToString();
            return str;
        }
        private BigInteger pow1(BigInteger bi, long e, BigInteger n)
        {
            BigInteger y = 1;
            BigInteger s = bi;
            string x = perevod(e);
            for (int i=0; i<x.Length; i++)
            {
                if (x[i] == '1') y = (y * s) % n;
                s = (s * s) % n;
            }
            return y;
        }
        private List<string>RSA_Encode(string s, long e, long n)
        {
            List<string> result = new List<string>();
            BigInteger bi;
            for (int i=0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);
                bi = new BigInteger(index);
                bi = pow1(bi, e, n);
                result.Add(bi.ToString());
            }
            return result;
        }
        private string RSA_Decode(List<string> input, long d, long n)
        {
            string result = "";
            BigInteger bi;
            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToInt64(item));
                bi = pow1(bi, d, n);
                int index = Convert.ToInt32(bi.ToString());
                result += characters[index].ToString();
            }
            return result;
        }
        public Form1()
        {
            InitializeComponent();
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != " " && textBox2.Text != " " && textBox3.Text != " ")
            {
                long p = Convert.ToInt64(textBox2.Text);
                long q = Convert.ToInt64(textBox3.Text);
                if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                {
                    string s = textBox1.Text;
                    s = s.ToUpper();
                    long n = p * q;
                    long m = (p - 1) * (q - 1);
                    long d = Calculate_d(m);
                    long e_ = Calculate_e(d, m);
                    List<string> result = RSA_Encode(s, e_, n);
                    StringBuilder encode = new StringBuilder();
                    foreach (string i in result)
                    {
                        encode.Append(i);
                        encode.Append(" ");
                    }
                    if (textBox4.Text != "") textBox4.Text = "";
                    if (textBox5.Text != "") textBox5.Text = "";
                    if (textBox6.Text != "") textBox6.Text = "";
                    textBox4.Text = encode.ToString();
                    textBox5.Text = d.ToString();
                    textBox6.Text = n.ToString();
                }
                else MessageBox.Show("p и q - не простые чила!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else MessageBox.Show("Чтобы зашифровать текст, все поля должны быть не пустыми!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                long d = Convert.ToInt64(textBox5.Text);
                long n = Convert.ToInt64(textBox6.Text);
                List<string> input = new List<string>();
                string str = textBox4.Text;
                String[] S1 = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in S1)
                    input.Add(s);
                string result = RSA_Decode(input, d, n);
                textBox7.Text = result;
            }
            else MessageBox.Show("Чтобы расшифровать текст, все поля должны быть не пустыми!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
