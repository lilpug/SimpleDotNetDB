
namespace SimpleDotNetDB.Core.Business
{
    public class WrapperAdditionalConfig
    {
        public string ParameterBindingStartValue { get; set; }
        public int? MaxDeadlockRetry { get; set; }
        public int? MaxDeadlockMSDelay { get; set; }
        public int? ConnectionTimeout { get; set; }
        public string DatabaseVersion { get; set; }
    }
}
