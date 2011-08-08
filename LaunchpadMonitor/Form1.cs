using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace LaunchpadMonitor
{
    public partial class Form1 : Form
    {

        Timer updateTimer =new Timer();

        IGathererRenderer gathererRenderer = null;
        ILaunchpad launchpadRenderer = null;

        public Form1()
        {
            InitializeComponent();
            List<CPU> cpus = CPU.CountCPUs();
            UpdateGathererComboBox(cpus);
            updateTimer.Tick += new EventHandler(timerEventHandler);
            updateTimer.Enabled = false;

            launchpadRenderer = new DiffingLaunchpadRenderer();
            gathererRenderer = new LaunchpadGathererRenderer(launchpadRenderer);
            launchpadRenderer.LeftButtonPressDelegate = delegate(bool release) { if (release) gathererRenderer.ZoomOut(); scaleTextBox.Text = gathererRenderer.Scale.ToString(); };
            launchpadRenderer.RightButtonPressDelegate = delegate(bool release) { if (release) gathererRenderer.ZoomIn(); scaleTextBox.Text = gathererRenderer.Scale.ToString(); };

        }

        private void UpdateGathererComboBox(IEnumerable<CPU> cpus)
        {
            processorComboBox.Items.Clear();
            foreach (CPU cpu in cpus)
            {
                foreach (PerfCPU perfCPU in cpu.PerfCPUs)
                {
                    Gatherer gatherer = new Gatherer(perfCPU.ManagementObjectSampler, 1000, perfCPU.ToString());
                    processorComboBox.Items.Add(gatherer);
                }
            }
        }

        private void timerEventHandler(object sender, EventArgs e)
        {
            Gatherer perfCPU = processorComboBox.SelectedItem as Gatherer;

            
            try
            {
                gathererRenderer.Present();
                loadTextBox1.Text = perfCPU.Latest.ToString();
            }
            catch (Exception x)
            {

            }

        }

        private void processorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTimer.Interval = 250;
            updateTimer.Enabled = true;
            gathererRenderer.Gatherer = processorComboBox.SelectedItem as Gatherer;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            launchpadRenderer.Dispose();
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            gathererRenderer.ZoomIn();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            gathererRenderer.ZoomOut();
        }
    }
}
