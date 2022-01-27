using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FuncMaker.Properties;
using org.mariuszgromada.math.mxparser;
using GeoFilter;

namespace FuncMaker
{
    public partial class FuncForm : Form
    {
        private TransformMatrix _tm;
        
        public FuncForm()
        {
            int range = Settings.Default.range;
            InitializeComponent();
            textBoxRange.Text = range.ToString();
            buttonGo.Enabled = false;
            checkBoxRadX.Enabled = false;
            checkBoxRadY.Enabled = false;
            radioStandard.Checked = true;
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            Image imghappy = Image.FromFile(@"Happy.jpg");
            Image imgsad = Image.FromFile(@"Sad.jpg");

            string sfunc = textBoxFunc.Text;
            Function func = new Function(sfunc);
            double test = func.calculate(0.5464, -0.3454454);
          
            if (double.IsNaN(test) == false)
            {
                pictureBoxOutcome.Image = imghappy;
                buttonGo.Enabled = true;
                checkBoxRadX.Enabled = true;
                checkBoxRadY.Enabled = true;
                buttonView.Enabled = true;
            }
            else
            {
                pictureBoxOutcome.Image = imgsad;
                buttonGo.Enabled = false;
                checkBoxRadX.Enabled = false;
                checkBoxRadY.Enabled = false;
                buttonView.Enabled = false;
            }
        }

        private void textBoxRange_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar));   
        }

        private void textBoxRange_TextChanged(object sender, EventArgs e)
        {

        }

        private double Pulse()
        {
            int Progress = Convert.ToInt32(Math.Floor(_tm.Progress() * 100 + 0.5));
            progressBar.Value = Progress;
            return _tm.Progress();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            double r, g, b, oob;
            string sfunc = textBoxFunc.Text;
            Function func = new Function(sfunc);
            int range = Convert.ToInt32(textBoxRange.Text);
            progressBar.Value = 0;
            _tm = new TransformMatrix(range, 0);
            _tm.Pulse += Pulse;
            _tm.Set(func, checkBoxRadX.Checked, checkBoxRadY.Checked);
            labelMin.Text = _tm.minimum.ToString();
            labelMax.Text = _tm.maximum.ToString();
            labelRange.Text = (_tm.InRangeFactor() * 100).ToString();
            _tm.FlatComposition(out r, out g, out b, out oob);
            labelRed.Text = (r * 100).ToString();
            labelGreen.Text = (g * 100).ToString();
            labelBlue.Text = (b * 100).ToString();
            labelOOB.Text = (oob * 100).ToString();
        }

        private void FuncForm_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            int range = Convert.ToInt32(textBoxRange.Text);
            int size = range * 2 + 1;
            Bitmap b = new Bitmap(size, size);
            BitmapBox bitbox = new BitmapBox(Color.Gray, size, size);
            if (radioStandard.Checked)
            {
                int sel = listBoxOOB.SelectedIndex;
                BitmapBox.OutOfBounds oob = BitmapBox.OutOfBounds.Reject;
                switch (sel)
                {
                    case 0: oob = BitmapBox.OutOfBounds.Reject;break;
                    case 1: oob = BitmapBox.OutOfBounds.Rollover;break;
                    case 2: oob = BitmapBox.OutOfBounds.Stop;break;
                    case 3: oob = BitmapBox.OutOfBounds.Bounce;break;
                }
                bitbox.ApplyMatrix(_tm, size / 2, size / 2, BitmapBox.Colour.Red, oob);
            } 
            else if (radioButtonFlatten.Checked)
            {
                bitbox.ApplyMatrixAndFlatten(_tm, size / 2, size / 2, BitmapBox.OutOfBounds.Reject);
            }
            else if (radioButtonStretch.Checked)
            {
                bitbox.ApplyStretchMatrix(_tm, size / 2, size / 2, BitmapBox.Colour.Red);
            }

            if (size > pictureBox1.Width)
            {
                pictureBox1.Image = bitbox.PeekMap(size / 2, size / 2, pictureBox1.Width / 2);
            }
            else
            {
                pictureBox1.Image = bitbox.bitmap;
            }
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                if (rb.Name == "radioStandard")
                {
                    listBoxOOB.Enabled = true;
                }
                else
                {
                    listBoxOOB.Enabled = false;
                }
            }
        }
    }
}
