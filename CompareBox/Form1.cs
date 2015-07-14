using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CompareBox
{
    public partial class Form1 : Form
    {
        private string fname1 = string.Empty;
        private string fname2 = string.Empty;
        private int count1 = 0;
        
        private int count2 = 0;
        private Bitmap img1;
        private Bitmap img2;
        private Bitmap buttonImage;
        private bool flag = true;
        private int x=0, y=0, i=0, j=0;
        private double total = 0.0;
        private int myProgress1 = 0, myProgress2 = 0;
        private int count = 0;

        Color firstImageColor = Color.Empty;
        Color secondImageColor = Color.Empty;

        double resultNew = 0.0, newMaxResult = 0.0, newMinResult = 0.0;
        double oldResult = 0.0, oldMinResult = 0.0, oldMaxResult = 0.0;

        int[] firstImageMatrix = new int[0];
        int[] secondImageMatrix = new int[0];
        int k = 0;
        int l = 0;
        double redTintDifference = 0.0;
        List<int> myList1 = new List<int>();
        List<int> myList2 = new List<int>();

        public Form1()
        {
            InitializeComponent();
            InitializeButton();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar2.Visible = false;
            progressBar2.ForeColor = Color.Orange;
            progressBar3.Visible = false;
            progressBar3.ForeColor = Color.Orange;

            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void InitializeButton()
        {
            //button4.Image = CompareBox.Properties.Resources.
            buttonImage = new Bitmap(button4.Image, button4.Width, button4.Height);
            button4.Image = buttonImage;
            button4.BackColor = Color.Transparent;
            button4.ForeColor = System.Drawing.Color.White;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatAppearance.BorderSize = 0;
        }
        
        //mouse enteres over the button
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            //change the backgroudn color of the image
            button4.UseVisualStyleBackColor = false;
            button4.BackColor = Color.GhostWhite;
        }

        //mouse leave after hover over
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.Transparent;
        }

        //mouse clicked over the button
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            button4.BackColor = Color.Blue;   
        }

        //mouse clicked and goes up over the button ;)
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            button4.BackColor = Color.Transparent;
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            //convert two images pixel by pixel taking eaxh pixel's red color value.
            /*
             * store 1st image's each pixel's red color value in a variable of Color type.
             * 
             * two images can be same and diff in these aspects:
             * 
             * YES/NO
             * 
             * 1. two same-color images with same size [done]
             * 2. two same-color images with diff size
             * 3. two diff-color images with same size [done]
             * 4. two diff-color images with diff size [done]
             * 
             * 0 -100
             * 
             * 1. two same-color images with same size 
             * 2. two same-color images with diff size
             * 3. two diff-color images with same size
             * 4. two diff-color images with diff size 
             * 
             * */
            CompareImages();  
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
            
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //richTextBox2.AppendText("\nLoading image..");
            int a, b;
            try
            {
                pictureBox1.Load(textBox1.Text);
                pictureBox1.Visible = true;
                //pictureBox1.ImageLocation = textBox1.Text;    //alternative

                img1 = new Bitmap(pictureBox1.Image);
                progressBar2.Visible = true;
                progressBar2.Maximum = img1.Width;

                //iterate through each pixel of first image
                for (a = 0; a < img1.Width; a++)
                {
                    for (b = 0; b < img1.Height; b++)
                    {
                        //firstImageColor = img1.GetPixel(a, b);
                        try
                        {
                            myList1.Add(img1.GetPixel(a, b).R);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.ToString(), "Error");
                        }
                    }
                    progressBar2.Value++;
                }

                richTextBox2.AppendText("\nImage loaded");
            }
            catch (FileNotFoundException fe)
            {
                richTextBox2.AppendText("\nPlease paste a valid URL");
            }
            catch (ArgumentException argumentE)
            {
                richTextBox2.AppendText("\nIncorrect URL");
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //richTextBox2.AppendText("\nLoading image..");
            int c, d;
            try
            {
                pictureBox2.Load(textBox2.Text);
                pictureBox2.Visible = true;
                //pictureBox2.ImageLocation = textBox2.Text;    //alternative

                img2 = new Bitmap(pictureBox2.Image);
                progressBar3.Visible = true;
                progressBar3.Maximum = img2.Width;

                //iterate through each pixel of second image
                for (c = 0; c < img2.Width; c++)
                {
                    for (d = 0; d < img2.Height; d++)
                    {
                        //secondImageColor = img2.GetPixel(c, d);
                        try
                        {
                            myList2.Add(img2.GetPixel(c, d).R);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.ToString(), "Error");
                        }
                    }
                    progressBar3.Value++;
                }
                richTextBox2.AppendText("\nImage loaded");
            }
            catch (FileNotFoundException fe)
            {
                richTextBox2.AppendText("\nPlease paste a valid URL");
            }
            catch (ArgumentException argumentE)
            {
                richTextBox2.AppendText("\nIncorrect URL");
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void CompareImages()
        {
            if ((string.IsNullOrWhiteSpace(textBox1.Text)) || (string.IsNullOrWhiteSpace(textBox2.Text)))
            {
                MessageBox.Show("Please enter image links","Image links");
            }

            //converting our list to array
            firstImageMatrix = myList1.ToArray();
            secondImageMatrix = myList2.ToArray();

            //next we go for comparing the red tint of each pixel of each image using mean square method
            while ((i < firstImageMatrix.Length) && (j < secondImageMatrix.Length))
            {
                //perform some operations
                redTintDifference += System.Math.Pow((firstImageMatrix[i] - secondImageMatrix[j]), 2);
                i++;
                j++;
            }

            string result = null;
            try
            {
                double sizeMultiplied = (img1.Width * img1.Height) + (img2.Width * img2.Height);
                result = (redTintDifference / sizeMultiplied).ToString();
            }
            catch (NullReferenceException ne)
            {
                //MessageBox.Show("Please try again","Try Again");
                richTextBox2.AppendText("\nPlease try again");
            }
            
            oldResult = Convert.ToDouble(result);
            oldMaxResult = 10000;
            oldMinResult = 0;
            newMinResult = 0;
            newMaxResult = 100;
            resultNew = (((oldResult - oldMinResult) * (newMaxResult - newMinResult)) / (oldMaxResult - oldMinResult)) + newMinResult;
            int resultNewInt = Convert.ToInt16(resultNew);
            //MessageBox.Show(resultNewInt.ToString(),"Here i am");

            if (!((string.IsNullOrWhiteSpace(textBox1.Text)) || (string.IsNullOrWhiteSpace(textBox2.Text))))
            {
                resultNewInt = 100 - resultNewInt;
                richTextBox2.AppendText("\n\n\nResult: " + resultNewInt);
            }

            if ((resultNewInt == 100) && !((string.IsNullOrWhiteSpace(textBox1.Text)) || (string.IsNullOrWhiteSpace(textBox2.Text))) )
            {
                richTextBox2.AppendText("\n\nExactly same images!");
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.SelectionStart = richTextBox2.Text.Length;
            richTextBox2.ScrollToCaret();
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            richTextBox2.Text = String.Empty;
            //progressBar2.Value = 0;
            //progressBar3.Value = 0;
        }

    }
}
