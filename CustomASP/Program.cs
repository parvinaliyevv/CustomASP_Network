namespace CustomASP;

public static class Program
{
    private static HttpListener _listener;


    static Program()
    {
        _listener = new();

        try
        {
            _listener.Prefixes.Add("http://localhost:27001/");
            _listener.Start();
        }
        catch { }
    }


    private static void Main(string[] args)
    {
        HttpListenerContext context = default;

        while (true)
        {
            try
            {
                context = _listener.GetContext();
            }
            catch
            {
                Console.WriteLine("Failed to start server!"); return;
            }

            CommunicationWithHostAsync(context);
        }
    }


    private static void CommunicationWithHost(HttpListenerContext httpListenerContext)
    {
        var request = httpListenerContext.Request;
        var response = httpListenerContext.Response;

        try
        {
            var controllerName = request.QueryString["controller"].ToString().Trim('"');
            var methodName = request.QueryString["method"].ToString().Trim('"');

            controllerName = char.ToUpper(controllerName[0]) + controllerName.Substring(1);
            methodName = char.ToUpper(methodName[0]) + methodName.Substring(1);

            var query = string.Format("CustomASP.Controllers.{0}Controller", controllerName);
            var controllerType = Type.GetType(query);

            var method = controllerType.GetMethod(methodName, Type.EmptyTypes);
            var returnedData = method.Invoke(Activator.CreateInstance(controllerType), new object[] {});

            var writer = new StreamWriter(response.OutputStream);

            writer.Write(returnedData);
            response.StatusCode = 200;

            writer.Close(); 
        }
        catch
        {
            response.StatusCode = 400;

            response.OutputStream.Close();
            response.Close();
        }
    }

    private static async Task CommunicationWithHostAsync(HttpListenerContext httpListenerContext)
        => await Task.Factory.StartNew(() => CommunicationWithHost(httpListenerContext));
}
