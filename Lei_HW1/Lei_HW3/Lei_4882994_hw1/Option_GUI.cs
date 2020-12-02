using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Lei_4882994_hw1
{
    public partial class Option_GUI : Form
    {
        public Option_GUI()
        {
            InitializeComponent();
        }

        private void radioCall_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioPut_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textK_TextChanged(object sender, EventArgs e)
        {
            double qwe;
            if (!double.TryParse(textK.Text, out qwe) || Convert.ToDouble(textK.Text) < 0)
            {
                errorProvider1.SetError(textK, "please enter a non-negative number");
            }
            else
            {
                errorProvider1.SetError(textK, string.Empty);
            }
        }

        private void textS_TextChanged(object sender, EventArgs e)
        {
            double qwe;
            if (!double.TryParse(textS.Text, out qwe) || Convert.ToDouble(textS.Text) < 0)
            {
                errorProvider1.SetError(textS, "please enter a non-negative number");
            }
            else
            {
                errorProvider1.SetError(textS, string.Empty);
            }
        }

        private void textT_TextChanged(object sender, EventArgs e)
        {
            double qwe;
            if (!double.TryParse(textT.Text, out qwe) || Convert.ToDouble(textT.Text) <= 0)
            {
                errorProvider1.SetError(textT, "please enter a positive number");
            }
            else
            {
                errorProvider1.SetError(textT, string.Empty);
            }
        }

        private void textR_TextChanged(object sender, EventArgs e)
        {
            double qwe;
            if (!double.TryParse(textR.Text, out qwe))
            {
                errorProvider1.SetError(textR, "please enter a numerical number");
            }
            else
            {
                errorProvider1.SetError(textR, string.Empty);
            }
        }

        private void textVol_TextChanged(object sender, EventArgs e)
        {
            double qwe;
            if (!double.TryParse(textVol.Text, out qwe) || Convert.ToDouble(textVol.Text) < 0)
            {
                errorProvider1.SetError(textVol, "please enter a non-negative number");
            }
            else
            {
                errorProvider1.SetError(textVol, string.Empty);
            }
        }

        private void textTrial_TextChanged(object sender, EventArgs e)
        {
            int qwe;
            if (!int.TryParse(textTrial.Text, out qwe) || Convert.ToInt64(textTrial.Text) <= 0)
            {
                errorProvider1.SetError(textTrial, "please enter a positive integer");
            }
            else
            {
                errorProvider1.SetError(textTrial, string.Empty);
            }
        }

        private void textstps_TextChanged(object sender, EventArgs e)
        {
            int qwe;
            if (!int.TryParse(textstps.Text, out qwe) || Convert.ToInt16(textstps.Text) <= 0)
            {
                errorProvider1.SetError(textstps, "please enter a positive integer");
            }
            else
            {
                errorProvider1.SetError(textstps, string.Empty);
            }
        }
        private void textOpt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textDelta_TextChanged(object sender, EventArgs e)
        {

        }

        private void textGamma_TextChanged(object sender, EventArgs e)
        {

        }

        private void textVega_TextChanged(object sender, EventArgs e)
        {

        }

        private void textTheta_TextChanged(object sender, EventArgs e)
        {

        }

        private void textRho_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
        private void Option_GUI_Load(object sender, EventArgs e)
        {
            errorProvider1.SetIconAlignment(textK, ErrorIconAlignment.MiddleRight);
            errorProvider1.SetIconPadding(textK, 4);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stpwc = new Stopwatch();
            stpwc.Start();
            double k, s, t, r, vol = new double();
            int tril, stps = new int();
            bool boo;
            bool anti;
            bool CV1;
            if (!radioCall.Checked && !radioPut.Checked)
            {
                textSys.Text = "Error: must choose Call Option or Put Option!";
            }
            else
            {
                if (radioCall.Checked)
                {
                    boo = true;
                }
                else
                {
                    boo = false;
                }

                try
                {
                    k = Convert.ToDouble(textK.Text);
                    s = Convert.ToDouble(textS.Text);
                    t = Convert.ToDouble(textT.Text);
                    r = Convert.ToDouble(textR.Text);
                    vol = Convert.ToDouble(textVol.Text);
                    tril = Convert.ToInt32(textTrial.Text);
                    stps = Convert.ToInt32(textstps.Text);
                    EuroOption europ = new EuroOption(boo, k, s, t, r, vol);//construct EuroOption instance
                    textSys.Text = "Computation in process...";//heavy computation load stop this message from displaying
                    randnum radmx = new randnum();
                    double[,] rndmatx;
                    if (checkAVR.Checked)
                    {
                        anti = true;
                        rndmatx = radmx.AVRrndmtx(tril, stps);
                    }
                    else
                    {
                        anti = false;
                        rndmatx = radmx.rndmtx(tril, stps);
                    }
                    if (checkDeltaCV.Checked)
                    {
                        CV1 = true;
                    }
                    else
                    {
                        CV1 = false;
                    }

                    double[] grks = europ.greeks(europ, rndmatx, anti, CV1);
                    textOpt.Text = Convert.ToString(grks[0]);
                    textSE.Text = Convert.ToString(grks[1]);
                    textDelta.Text = Convert.ToString(grks[2]);
                    textGamma.Text = Convert.ToString(grks[3]);
                    textVega.Text = Convert.ToString(grks[4]);
                    textTheta.Text = Convert.ToString(grks[5]);
                    textRho.Text = Convert.ToString(grks[6]);
                    textSys.Text = "Computation is done";
                }
                catch (Exception f)
                {
                    textSys.Text = "Error: check your input. trial or steps might be too large.";
                }
            }
            stpwc.Stop();
            TimeSpan ts = stpwc.Elapsed;
            textBoxruntime.Text = Convert.ToString(ts.TotalSeconds);
        }

        private void textSys_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSE_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSys1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkAVR_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxruntime_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkDeltaCV_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
