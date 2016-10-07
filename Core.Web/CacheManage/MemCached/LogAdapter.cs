using System;

namespace Core.Web.CacheManage.Memcached
{
	internal class LogAdapter {
		public static LogAdapter GetLogger(Type type) {
			return new LogAdapter(type);
		}

		public static LogAdapter GetLogger(string name) {
			return new LogAdapter(name);
		}

		private string loggerName;
		private LogAdapter(string name) { loggerName = name; }
		private LogAdapter(Type type) { loggerName = type.FullName; }
		public void Debug(string message) { Console.Out.WriteLine(DateTime.Now + " DEBUG " + loggerName + " - " + message); }
		public void Info(string message) { Console.Out.WriteLine(DateTime.Now + " INFO " + loggerName + " - " + message); }
		public void Warn(string message) { Console.Out.WriteLine(DateTime.Now + " WARN " + loggerName + " - " + message); }
		public void Error(string message) { Console.Out.WriteLine(DateTime.Now + " ERROR " + loggerName + " - " + message); }
		public void Fatal(string message) { Console.Out.WriteLine(DateTime.Now + " FATAL " + loggerName + " - " + message); }
		public void Debug(string message, Exception e) { Console.Out.WriteLine(DateTime.Now + " DEBUG " + loggerName + " - " + message + "\n" + e.Message + "\n" + e.StackTrace); }
		public void Info(string message, Exception e) { Console.Out.WriteLine(DateTime.Now + " INFO " + loggerName + " - " + message + "\n" + e.Message + "\n" + e.StackTrace); }
		public void Warn(string message, Exception e) { Console.Out.WriteLine(DateTime.Now + " WARN " + loggerName + " - " + message + "\n" + e.Message + "\n" + e.StackTrace); }
		public void Error(string message, Exception e) { Console.Out.WriteLine(DateTime.Now + " ERROR " + loggerName + " - " + message + "\n" + e.Message + "\n" + e.StackTrace); }
		public void Fatal(string message, Exception e) { Console.Out.WriteLine(DateTime.Now + " FATAL " + loggerName + " - " + message + "\n" + e.Message + "\n" + e.StackTrace); }

	
	}
}
