using OpenQA.Selenium;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;

namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class LoginPage : BasePage
    {
        private readonly By _usernameInput = By.Name("username");
        private readonly By _passwordInput = By.Name("password");
        private readonly By _loginButton = By.CssSelector("button[type='submit']");
        private readonly By _errorMessage = By.CssSelector(".oxd-text.oxd-text--p.oxd-alert-content-text");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void EnterUsername(string _username) 
        {
            WaitAndSendKeys(_usernameInput, _username);
        }

        public void EnterPassword(string _password)
        {
            WaitAndSendKeys(_passwordInput, _password);
        }

        public DashboardPage ClickLogin()
        {
            WaitAndClick(_loginButton);
            return new DashboardPage(Driver);
        }

        public DashboardPage Login(string username, string password)
        {
            WaitAndSendKeys(_usernameInput, username);
            WaitAndSendKeys(_passwordInput, password);
            WaitAndClick(_loginButton);
            return new DashboardPage(Driver);
        }

        public string GetErrorMessage() => WaitAndGetText(_errorMessage);
        public bool IsDisplayed() => IsElementDisplayed(_usernameInput);
    }
}
