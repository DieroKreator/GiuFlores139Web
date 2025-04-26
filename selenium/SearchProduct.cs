using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

[TestFixture] // Configura como uma classe de teste
public class SearchproductTest
{
    private IWebDriver driver;

    [SetUp] // Configura um método para ser executado antes dos testes
    public void Before()
    {
        // Faz o download e instalação da versão mais recente do ChromeDriver
        new DriverManager().SetUpDriver(new ChromeConfig());
        driver = new ChromeDriver(); // Instancia o objeto do Selenium como Chrome
        driver.Manage().Window.Maximize(); // Maximiza a janela do navegador
        // Configura uma espera de 5 segundos para qualquer elemento aparecer
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);
    } // fim do Before

    [TearDown] // Configura um método para ser usado depois dos testes
    public void After()
    {
        driver.Quit(); // Destruir o objeto do Selenium na memória
        driver.Dispose();
    }

    [Test]
    public void SearchProduct()
    {
        driver.Navigate().GoToUrl("https://www.giulianaflores.com.br/");
        IWebElement searchBox = driver.FindElement(By.XPath("//textarea[@id='txtDsKeyWord']"));
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        searchBox.SendKeys("Mini Rosa Encantada");
        IWebElement searchButton = driver.FindElement(By.Id("btnSearch"));
        searchButton.Click();
        IWebElement product = wait.Until(
            driver => driver.FindElement(By.XPath("//*[contains(text(),'Mini Rosa Encantada')]"))
        );
        Assert.AreEqual(product.Text, "Mini Rosa Encantada");
        product.Click();
        // Thread.Sleep(2000); // Espera 2 segundos para visualizar o resultado
        IWebElement productPageLabel = wait.Until(driver => driver.FindElement(By.Id("ContentSite_lblProductDsName")));
        IList<IWebElement> productPagePrice = driver.FindElements(
            By.XPath("//span[@class='precoPor_prod']")
        );
        Assert.AreEqual(productPageLabel.Text, "MINI ROSA ENCANTADA");
        Assert.AreEqual(productPagePrice[0].Text, "R$ 299,90");
    }
}
