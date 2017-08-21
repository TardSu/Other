using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.IO;

namespace GEN_SSD1306
{
    public partial class Form1 : Form
    {
        Bitmap pic;
        private int color8 = 128;
        private char[] hex_table = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        string file_full_path;
        string file_path;
        string file_name;

        int pic_Height;
        int pic_Width;

        public Form1()
        {
            InitializeComponent();
        }

        private string byte2hex(byte a)
        {
            string hex = "0x";
            hex = hex + hex_table[(byte)(a / 16)];
            hex = hex + hex_table[(byte)(a % 16)];
            return hex;
        }

        private bool check_black_pixel(Color c)
        {
            int r = c.R;
            int g = c.G;
            int b = c.B;
            if (System.Math.Sqrt(r * r + g * g + b * b) < color8)
            {
                return true;
            }
            else
                return false;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.bmp)|*.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pic = new Bitmap(open.FileName);
                pictureBox1.Image = pic;

                pic_Height = pic.Height;
                pic_Width = pic.Width;

                file_full_path = open.FileName;
                file_name = Path.GetFileName(file_full_path);
                file_path = Path.GetDirectoryName(file_full_path);

                int dot;
                string file_ext = ".bmp";
                string file_search = file_name.ToUpper();

                dot = file_search.IndexOf(file_ext.ToUpper());

                file_name = file_search.Substring(0, dot).ToLower();
                toolStripLabel3.Enabled = true;

            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            byte x;
            byte code;
            int i, j, k;

            /*Write the string to a file.*/
            System.IO.StreamWriter file = new System.IO.StreamWriter(file_path + "\\" + file_name + ".h");

            file.WriteLine("const unsigned char " + file_name + "[] =");
            file.Write("{");

            for (j = 0; j < pic_Height; j += 8)
            {
                for (i = 0; i < pic_Width; i++)
                {
                    x = 0x01;
                    code = 0;
                    for (k = 0; k < 8; k++)
                    {
                        if (check_black_pixel(pic.GetPixel(i, j + k)))
                        {
                            code += x;
                        }
                        x = (byte)(x * 2);
                    }

                    if (i % 8 == 0)
                        file.Write("\r\n    ");

                    file.Write(byte2hex(code) + ",");
                }
            }
            file.WriteLine("\r\n};");
            file.Close();

            MessageBox.Show("Generate complete.");
        }
    }
}
