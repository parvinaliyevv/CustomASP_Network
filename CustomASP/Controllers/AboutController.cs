namespace CustomASP.Controllers;

public class AboutController
{
    public string Index() => "About controller index method without parameter";

    public string Index(string data) => "About controller index method with parameter";
}
