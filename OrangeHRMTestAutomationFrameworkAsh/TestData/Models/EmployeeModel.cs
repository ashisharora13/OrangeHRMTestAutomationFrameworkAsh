using Bogus;

namespace OrangeHRM.Automation.Framework.TestData
{
    public class EmployeeModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeId { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
