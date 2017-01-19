using System.Web.Http.ExceptionHandling;
using log4net;

namespace GameWebApi.Web.Common
{
    /// <summary>
    /// Logs any undhandled exceptions thrown in the code
    /// </summary>
    public class GlobalExceptionLogger : ExceptionLogger
    {
        //Get the log4net logger which has a name GlobalExceptionLogger
        private readonly ILog _log = LogManager.GetLogger("GlobalExceptionLogger");

        public override void Log(ExceptionLoggerContext context)
        {
            _log.Error("Unhandled exception", context.Exception);
        }
    }
}