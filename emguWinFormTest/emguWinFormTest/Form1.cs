using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace emguWinFormTest
{
    public partial class Form1 : Form
    {

        private Image<Bgr, byte> imgInput;


        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(ofd.FileName);
                    //Image<Gray, byte> canny = new Image<Gray, byte>(imgInput.Width,imgInput.Height,new Gray(0)); 
                    //canny = imgInput.Canny(50, 20);

                    
                    pictureBox1.Image = imgInput.Bitmap;
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (imgInput == null)
            {
                return;
            }

            try
            {
                var temp = imgInput.SmoothGaussian(5).Convert<Gray, byte>().ThresholdBinary(new Gray(11), new Gray(255));

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                Mat m = new Mat();
                
                Rectangle roi = new Rectangle(imgInput.Width/2-200, 30, 425, 100);
                imgInput.ROI = roi;
                temp.ROI = roi;

                VectorOfPoint points = new VectorOfPoint();
                CvInvoke.FindContours(temp, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                for (int i = 0; i < contours.Size-1; i++)
                {
                    double perimeter = CvInvoke.ArcLength(contours[i], true);
                    VectorOfPoint approx = new VectorOfPoint();
                    //CvInvoke.ApproxPolyDP(contours[i], approx, 0.01 * perimeter, false);

                   
                    CvInvoke.DrawContours(imgInput, contours, i, new MCvScalar(0, 0, 222), 2);

                    //moments  center of the shape
                    points.Push(contours[i]);
                    var moments = CvInvoke.Moments(contours[i]);
                    int x = (int)(moments.M10 / moments.M00);
                    int y = (int)(moments.M01 / moments.M00);



                    CvInvoke.Circle(imgInput, new Point(imgInput.Width/2, imgInput.Height/2), 2, new MCvScalar(255, 66, 55));
                    if (approx.Size > 6)
                    {
                       
                           
                    }

                    pictureBox1.Image = imgInput.Bitmap;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
