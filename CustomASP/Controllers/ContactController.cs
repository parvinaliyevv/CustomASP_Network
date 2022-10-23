namespace CustomASP.Controllers;

public class ContactController
{
    public string Index() => "Contact controller index method without parameter";

    public string Index(string data) => "Contact controller index method with parameter";
}
