namespace CustomASP.Controllers;

public class HomeController
{
    public string Index() => "Home controller index method without parameter";

    public string Index(string data) => "Home controller index method with parameter";
}
