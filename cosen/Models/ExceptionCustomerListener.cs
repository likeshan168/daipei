
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
////using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
//using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

//using Microsoft.Practices.Unity;
namespace cosen.Models
{
    //[ConfigurationElementType(typeof(CustomTraceListenerData))]
    //public class ExceptionCustomerListener : CustomTraceListener
    //{
    //    //string writeLogSQL = string.Empty;
    //    DataContextDataContext dataContext;
    //    Exception ex;
    //    public ExceptionCustomerListener()
    //        : base()
    //    {
    //        dataContext = new DataContextDataContext();
    //    }
    //    public override void TraceData(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, object data)
    //    {
    //        if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
    //        {
    //            if (data is LogEntry)
    //            {
    //                LogEntry logEntry = data as LogEntry;

    //                ExceptionLog log = new ExceptionLog()
    //                      {
    //                          Message = logEntry.Message,
    //                          LogDate = logEntry.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
    //                          ExceptionLevel = logEntry.LoggedSeverity,
    //                          Exception = logEntry.Message
    //                      };
    //                ExecuteWriteLogSQL(log, dataContext);
    //            }
    //            else if (data is string)
    //            {

    //                Write(data as string);
    //            }
    //            else
    //            {
    //                base.TraceData(eventCache, source, eventType, id, data);
    //            }
    //        }

    //    }

    //    public override void Write(string message)
    //    {
    //        ExceptionLog log = new ExceptionLog()
    //        {
    //            Message = message,
    //            LogDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    //            ExceptionLevel = TraceEventType.Information.ToString(),
    //            Exception = message
    //        };
    //        ExecuteWriteLogSQL(log, dataContext);
    //    }

    //    public override void WriteLine(string message)
    //    {
    //        Write(message);
    //    }
    //    private void ExecuteWriteLogSQL(ExceptionLog log, DataContextDataContext db)
    //    {
    //        db.ExceptionLog.InsertOnSubmit(log);
    //        db.SubmitChanges();
    //    }


    //}
}