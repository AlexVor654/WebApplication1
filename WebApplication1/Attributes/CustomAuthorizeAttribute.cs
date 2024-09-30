using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : ActionFilterAttribute
{
    private readonly int _requiredRole;

    public CustomAuthorizeAttribute(int requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.Session.GetInt32("UserRole");

        if (!userRole.HasValue || userRole.Value != _requiredRole)
        {
            // Пользователь не имеет нужной роли, перенаправляем его на страницу логина или запрещаем доступ
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }

        base.OnActionExecuting(context);
    }
}
