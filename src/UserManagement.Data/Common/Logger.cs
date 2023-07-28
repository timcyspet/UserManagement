using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data.Common
{
    public interface ILogger
    {
        void Fatal(System.Exception exception);
        void Error(string message, System.Exception exception = null);
        void Warn(string message);
        void Info(string message);
        void Debug(string message);

    }
    public class Logger : ILogger
    {
       // private static readonly ILog Log;//= LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly long InstanceTicks;
        public Logger()
        {
            InstanceTicks = DateTime.Now.Ticks;
        }

        public void Fatal(System.Exception exception)
        {

           // Log.Debug($"[{InstanceTicks}] [Exception] {exception.Message} {exception.ToString()} ");

        }
        public void Error(string message, System.Exception exception = null)
        {
            //if (exception == null) Log.Error($"[{InstanceTicks}] {message}");
            //else Log.Debug($"[{InstanceTicks}] {message}", exception);
        }
        public void Warn(string message)
        {
            //Log.Warn($"[{InstanceTicks}] {message}");
        }
        public void Info(string message)
        {
           // Log.Info($"[{InstanceTicks}] {message}");
        }

        public void Debug(string message)
        {
            //Log.Debug($"[{InstanceTicks}] {message}");
        }
    }
}
