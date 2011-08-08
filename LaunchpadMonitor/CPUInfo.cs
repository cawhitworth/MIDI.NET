using System.Collections.Generic;
using System.Management;
using System;

namespace LaunchpadMonitor
{
    class CPU
    {
        public uint Cores { private set; get; }
        public uint LogicalProcessors { private set; get; }
        public bool Hyperthreaded { private set; get; }
        public uint Socket { private set; get; }

        List<PerfCPU> perfCPUs = new List<PerfCPU>();

        public List<PerfCPU> PerfCPUs { get { return perfCPUs; } }

        public CPU(uint Socket, uint Cores, uint LogicalProcessors)
        {
            this.Socket = Socket;
            this.Cores = Cores;
            this.LogicalProcessors = LogicalProcessors;
            this.Hyperthreaded = LogicalProcessors > Cores;

            CountPerfCPUs();
        }

        public void CountPerfCPUs()
        {
            SelectQuery cpuQuery = new SelectQuery("Win32_PerfFormattedData_PerfOS_Processor");

            ManagementObjectSearcher perfCPUSearcher = new ManagementObjectSearcher(cpuQuery);
            uint core = 1, htmodule = 1;
            ManagementObjectCollection managementObjectPerfProcessors = perfCPUSearcher.Get();
            uint maxHTModule = (uint)(managementObjectPerfProcessors.Count - 1) / Cores;
            PerfCPU perfCPU = null;
            foreach (ManagementObject managementObjectPerfProcessor in managementObjectPerfProcessors)
            {
                if (managementObjectPerfProcessor["Name"] as string != "_Total")
                {
                    if (Hyperthreaded)
                    {
                        perfCPU = new PerfCPU( managementObjectPerfProcessor, Socket, core, htmodule );
                    }
                    else
                    {
                        perfCPU = new PerfCPU( managementObjectPerfProcessor, Socket, core );
                    }
                    if (Hyperthreaded)
                    {
                        htmodule++;
                        if (htmodule > maxHTModule)
                        {
                            htmodule = 1;
                            core++;
                        }
                    }
                    else
                    {
                        core++;
                    }

                    perfCPUs.Add(perfCPU);
                }
                else
                {
                    perfCPU = new PerfCPU(managementObjectPerfProcessor, Socket);
                    PerfCPUs.Add(perfCPU);
                }
            }
        }
    
        internal static List<CPU> CountCPUs()
        {
            List<CPU> cpus = new List<CPU>();
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
            return cpus;
        }
    }

    class PerfCPU
    {
        public string Name { get; private set; }
        public uint Core { get; private set; }
        public uint Socket { get; private set; }
        public uint HTModule { get; private set; }
        public bool HTEnabled { get; private set; }

        public bool Total { get; private set; }

        public ManagementObjectSampler ManagementObjectSampler { get; private set; }

        public override string ToString()
        {

            string str;

            if (!Total)
            {
                str = "CPU " + Name + ": Socket " + Socket + " Core " + Core;
                if (HTEnabled)
                {
                    str = str + " HT Module " + HTModule;
                }
            }
            else
            {
                str = "CPU Total: Socket " + Socket;
            }

            return str;
        }

        public PerfCPU(ManagementObject managementObject, uint Socket)
        {
            this.ManagementObjectSampler = new ManagementObjectSampler(managementObject, "PercentProcessorTime");
            this.Name = managementObject["Name"] as string;
            this.Socket = Socket;
            this.Total = true;
        }

        public PerfCPU(ManagementObject managementObject, uint Core, uint Socket)
        {
            this.ManagementObjectSampler = new ManagementObjectSampler(managementObject, "PercentProcessorTime");
            this.Name = managementObject["Name"] as string;
            this.Core = Core;
            this.Socket = Socket;
            this.HTEnabled = false;
            this.Total = false;
        }

        public PerfCPU(ManagementObject managementObject, uint Core, uint Socket, uint HTModule)
        {
            this.ManagementObjectSampler = new ManagementObjectSampler(managementObject, "PercentProcessorTime");
            this.Name = managementObject["Name"] as string;
            this.Core = Core;
            this.Socket = Socket;
            this.HTModule = HTModule;
            this.HTEnabled = true;
            this.Total = false;
        }
    }

}