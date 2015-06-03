using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeTimer
{
    // ReSharper disable once UnusedMember.Global
    public static class CodeTimer
    {
        public static void Evaluate(string displayName, Action action,
            int iterationCount, bool isInConsole)
        {
            IOutput output;
            if (isInConsole)
            {
                output = new ConsoleOutput();
            }
            else
            {
                output = new DebugOutput();
            }

            Initialize(output);

            Monitor(displayName, iterationCount, action, output);

            End();
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        private static void Initialize(IOutput output)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            output.WriteLine();

            // Let JIT compile the Monitor method first
            Monitor("", 1, () => { }, output);
        }

        private static void End()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
            GC.Collect();
        }
        
        private static void Monitor(string name, int iteration, Action action, IOutput outPut)
        {
            if (string.IsNullOrEmpty(name)) return;
            
            var currentForeColor = outPut.ForegroundColor;
            outPut.ForegroundColor = ConsoleColor.Yellow;
            outPut.WriteLine(name + "\t==========>");
            
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            var gcCounts = new int[GC.MaxGeneration + 1];
            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            outPut.ForegroundColor = ConsoleColor.Green;
            outPut.WriteLine("Start running...");
            outPut.ForegroundColor = currentForeColor;

            var watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (var i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            outPut.ForegroundColor = ConsoleColor.Green;
            outPut.WriteLine("End running...");

            outPut.WriteLine();
            outPut.ForegroundColor = ConsoleColor.Cyan;
            outPut.WriteLine("\t\t\t-------------------------");
            outPut.WriteLine("\t\t\t|Time Elapsed:\t" 
                + watch.ElapsedMilliseconds.ToString("N0") + "ms\t|");
            outPut.WriteLine("\t\t\t|CPU Cycles:\t" + cpuCycles.ToString("N0") + "\t|");
            
            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                outPut.WriteLine("\t\t\t|Gen " + i + ": \t" + count + "\t|");
            }

            outPut.WriteLine("\t\t\t-------------------------");
            outPut.ForegroundColor = ConsoleColor.Yellow;
            outPut.WriteLine("\t\t\t\t\t\t<==========\t" + name);
            outPut.ForegroundColor = currentForeColor;
            outPut.WriteLine();
        }
    }
}