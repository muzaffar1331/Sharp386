using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Sharp386
{
    public partial class Form1 : Form
    {
        Sharp386 p;

        public Form1()
        {
            InitializeComponent();
            p = new Sharp386();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            p.Load("test.bin");

            new Thread(new ThreadStart(() =>
            {
                p.Start();
            })).Start();


            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 100;

            t.Elapsed += (s, a) =>
              {
                  Invoke(new Action(() =>
                  {
                      LabelEIP.Text = "EIP: 0x" + p.EIP.ToString("X8");
                      LabelEAX.Text = "EAX: 0x" + p.EAX.ToString("X8") + " " + p.EAX.ToString();
                      LabelEBX.Text = "EBX: 0x" + p.EBX.ToString("X8") + " " + p.EBX.ToString();
                      LabelECX.Text = "ECX: 0x" + p.ECX.ToString("X8") + " " + p.ECX.ToString();
                      LabelEDX.Text = "EDX: 0x" + p.EDX.ToString("X8") + " " + p.EDX.ToString();

                      this.Text = "x86 Virtual Machine: " + p.InstructionsExecutedCount_Delta.ToString("N3").Replace(".000", "") + " IPS @ " + p.ClockCount_Delta.ToString("N3").Replace(".000", "") + " Hz";
                  }));
              };

            t.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            p.Stop();
        }
    }
}
