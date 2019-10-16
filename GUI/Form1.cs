//2019 Eric Johnson

using System;
using System.Windows.Forms;

namespace GPU_DLL_Demo
{
    public partial class Form1 : Form
    {
        public static Compute compute = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            compute = new Compute(2); //set up for 2 elements
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] input_array = new int[2];
            input_array[0] = (int)numericUpDown1.Value;
            input_array[1] = (int)numericUpDown2.Value;
            compute.LoadArrayGPU(input_array);
            int[] output_array = compute.GetComputed((int)numericUpDown5.Value);

            numericUpDown3.Value = output_array[0];
            numericUpDown4.Value = output_array[1];
        }
    }
}