using Microsoft.AspNetCore.Mvc;
public class BaseController : Controller
{
    protected void SetFlashMessage(string message, string type = "info")
    {
        TempData["Message"] = message;
        TempData["MessageType"] = type.ToLower();
    }
}

