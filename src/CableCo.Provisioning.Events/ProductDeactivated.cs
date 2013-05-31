namespace CableCo.Provisioning.Events
{
    public class ProductDeactivated
    {
        public string AccountCode { get; set; }

        public string ProductCode { get; set; }
    }
}