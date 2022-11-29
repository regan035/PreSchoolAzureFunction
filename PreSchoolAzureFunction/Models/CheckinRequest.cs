namespace PreSchoolAzureFunction.Models
{
    public class CheckinRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Permission { get; set; }
        public string Status { get; set; }
    }
}
