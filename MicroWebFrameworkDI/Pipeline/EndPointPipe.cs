using MicroWebFramework.Contracts;
using MicroWebFramework.DI;
using MicroWebFramework.Exceptions;
using MicroWebFramework.Services;
using System.Linq;

namespace MicroWebFramework.Pipeline;
public class EndPointPipe : BasePipe
{
    public EndPointPipe() : base(null!)
    {
    }
    public EndPointPipe(Action<HttpContext> next) : base(next)
    {
    }
    public override void Handle(HttpContext context)
    {
        "Routing...".Dump();
        try
        {
            var parts = context.Url.Split('/');
            var controllerName = parts[1];
            var actionName = parts[2];
            string userId;

            if (string.IsNullOrEmpty(controllerName))
                throw new NoControllerProvidedException();

            if (string.IsNullOrEmpty(actionName))
                throw new NoActionProvidedException();

            var controllerNameTemplate = $"MicroWebFramework.Controllers.{controllerName}Controller";
            var controllerType = Type.GetType(controllerNameTemplate); 
            
            if(controllerType is null)
                throw new RouteNotFoundException();

            var controllerConstructor = controllerType!.GetConstructors().First();
            var controllerConstructorParameters = controllerConstructor.GetParameters();
            var controllerParameters = new List<object>();
            //var service = new DependencyServiceProvider();
            foreach (var param in controllerConstructorParameters)
            {
                if (param.ParameterType.IsInterface)
                {
                    controllerParameters.Add(DependencyServiceProvider.Get(param.ParameterType));
                    //services.Add(container.Resolve<INotificationService, EmailService>());
                }
            }
            var parameterArray = new object[controllerConstructorParameters.Length];
            for (int i = 0; i < controllerConstructorParameters.Length; i++)
            {
                if (controllerConstructorParameters.ElementAt(i).ParameterType == typeof(HttpContext))
                    parameterArray[i] = context;
                else if (controllerConstructorParameters.ElementAt(i).ParameterType.IsInterface)
                    parameterArray[i] = controllerParameters.Where(p =>
                        controllerConstructorParameters.ElementAt(i).ParameterType
                        .IsAssignableFrom(p.GetType())).FirstOrDefault()!;
            }

            object controllerInstance = Activator.CreateInstance(controllerType, parameterArray)!;
            //if (controllerParameters.Count > 0)
            //    controllerInstance = Activator.CreateInstance(controllerType, new object[] { context, controllerParameters })!;
            //else
            //    controllerInstance = Activator.CreateInstance(controllerType, new object[] { context })!;

            var methodInfo = controllerType.GetMethod(actionName);

            if (methodInfo is null)
                throw new RouteNotFoundException();

            var parameterList = methodInfo.GetParameters();
            if (parameterList.Length > 0 && parts.Length > 3)
            {
                userId = parts[3];

                if (string.IsNullOrEmpty(actionName))
                    throw new NoParameterProvidedException();

                object[] parameters = new object[parameterList.Length];
                for (int i = 0; i < parameterList.Length; i++)
                {
                    var convertedParameter = Convert.ChangeType(userId, parameterList[i].ParameterType);
                    parameters[i] = convertedParameter;
                }
                methodInfo.Invoke(controllerInstance, parameters);
            }
            else
            {
                methodInfo.Invoke(controllerInstance, null);
            }
            if (_next is not null) _next(context);
        }
        catch (NoControllerProvidedException ex)
        {
            ex.Message.Dump();
        }
        catch (NoActionProvidedException ex)
        {
            ex.Message.Dump();
        }
        catch (RouteNotFoundException ex)
        {
            ex.Message.Dump();
        }
        catch (Exception ex)
        {
            ex.Message.Dump();
        }
    }
}
