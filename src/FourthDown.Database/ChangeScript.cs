using System;

namespace FourthDown.Database
{
    public class ChangeScript
    {
        public int ChangeScriptId { get; set; }
        public string ChangeScriptName { get; set; }
        public DateTime ChangeScriptDeployStart { get; set; }
        public DateTime ChangeScriptDeployEnd { get; set; }
        public DateTime ChangeScriptDeployDuration { get; set; }
        public bool ChangeScriptDeploySuccess { get; set; }
    }
}