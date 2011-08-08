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
        List<CPU> cpus = new List<CPU>();
        List<Gatherer> gatherers = new List<Gatherer>();

        Timer updateTimer =new Timer();

        IGathererRenderer gathererRenderer = null;
        ILaunchpad launchpadRenderer = null;

        public Form1()
        {
            InitializeComponent();
            CountCPUs();
            UpdateCPUComboBox();
            updateTimer.Tick += new EventHandler(timerEventHandler);
            updateTimer.Enabled = false;

            launchpadRenderer = new SimpleLaunchpadRenderer();
            gathererRenderer = new LaunchpadGathererRenderer(launchpadRenderer);
        }

        private void CountCPUs()
        {
            SelectQuery selectQuery = new SelectQuery("Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            CPU cpu = null;
            uint socket = 1;
            foreach(ManagementObject processor in searcher.Get())
            {
                cpu = new CPU(socket,
                    (uint)processor["NumberOfCores"],
                    (uint)processor["NumberOfLogicalProcessors"]);
                cpus.Add(cpu);

                socket++;
            }
        }

        private void UpdateCPUComboBox()
        {
            processorComboBox.Items.Clear();
            foreach (CPU cpu in cpus)
            {
                foreach (PerfCPU perfCPU in cpu.PerfCPUs)
                {
                    Gatherer gatherer = new Gatherer(perfCPU.ManagementObjectSampler, 1000, perfCPU.ToString());
                    gatherers.Add(gatherer);
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
    }
}
