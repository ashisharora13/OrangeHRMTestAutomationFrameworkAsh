using System;

namespace OrangeHRM.Automation.Framework.Core.Models
{
    public class TestResult
    {
        public string Outcome { get; set; }
        public string Message { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public string State { get; set; } = "Completed";
    }
}
