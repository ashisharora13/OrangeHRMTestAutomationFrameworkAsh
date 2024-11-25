using OrangeHRM.Automation.Framework.TestData;
using Bogus;

namespace OrangeHRM.Automation.Framework.TestData
{
    public class EmployeeDataGenerator
    {
        private readonly Faker<EmployeeModel> _faker;

        public EmployeeDataGenerator()
        {
            _faker = new Faker<EmployeeModel>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.EmployeeId, f => f.Random.Replace("EMP###"))
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(30));
        }

        public EmployeeModel GenerateEmployee()
        {
            return _faker.Generate();
        }

        public List<EmployeeModel> GenerateEmployees(int count)
        {
            return _faker.Generate(count);
        }
    }
}