# Logger package

## Usage
* Install the Logger NuGet package on your project.
* Create a Logging.config file in the root of your project
```XML
<log4net>
  <appender name="AccessLogAppender" type="log4net.Appender.RollingFileAppender">
    <file value="C:\sandbox\logger\logs\testApi_access.log" />
    <appendToFile value="true" />
    <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
    <rollingStyle value="Size" />
    <maxSizeRollBackups value="10" />
    <maximumFileSize value="10MB" />
    <staticLogFileName value="true" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%message%newline" />
    </layout>
  </appender>

  <appender name="ApplicationAppender" type="log4net.Appender.RollingFileAppender">
    <file value="C:\sandbox\logger\logs\testApi_application.log" />
    <appendToFile value="true" />
    <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
    <rollingStyle value="Size" />
    <maxSizeRollBackups value="10" />
    <maximumFileSize value="10MB" />
    <staticLogFileName value="true" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%message%newline" />
    </layout>
  </appender>


  <root>
    <level value="ALL" />
    <appender-ref ref="AccessLogAppender" />
  </root>

  <logger name="AccessLog" additivity="false">
    <level value="INFO" />
    <appender-ref ref="AccessLogAppender" />
  </logger>

  <logger name="Application" additivity="false">
    <level value="INFO" />
    <appender-ref ref="ApplicationAppender" />
  </logger>

</log4net>
```
* Add a reference to Unity Web API NuGet package and setup DI for the following types in UnityConfig.RegisterComponents:
  ```C#
    // register all your components with the container here
    // it is NOT necessary to register your controllers
            
    // e.g. container.RegisterType<ITestService, TestService>();

    // Can obtain these from web.config 
    var application = "TestAPI";
    var applicationVersion = "1.0";
    var environment = "Dev";

    var loggerFactory = new LoggerFactory(System.Enum.GetNames(typeof(LoggerTypes)), application,applicationVersion, environment);
    container.RegisterInstance<ILoggerFactory>(loggerFactory, InstanceLifetime.Singleton);
    container.RegisterInstance<IApplicationLogRecordBuilder>(new ApplicationLogRecordBuilder(application, applicationVersion, environment));
    container.RegisterType<IStructuredLogHelper, StructuredLogHelper>();
  ```
* Derive from LogRecord to create an ApplicationLogRecord if you want to log application specific fields.
```C#
_logger = loggerFactory.GetLogger(LoggerTypes.Application.ToString());
_logger.LogInfo(new ApplicationLogRecord(...));
```
* Log an exception.
```C#
try
{
...
}
catch (Exception e)
{
	var logRecord = _logRecordBuilder.Build(ex, correlationId);
    _logger.LogError(logRecord);;
}
```

## Access Logging Usage

Access logging is handled using an http module. Add this to your web.config:
```
	<system.webServer>
      <modules>
        <add name="AccessLoggingModule" type="Logger.Middleware.AccessLoggingModule, Logger" />
    </modules>
```
Add the following to global.asax.cs to setup unhandled exception logging and to filter access logging requests (to filter out health checks for example):
```C#
	public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            // Setup unhandled error logging & filter
            var accessLoggingModule = HttpContext.Current.ApplicationInstance.Modules.Get("AccessLoggingModule") as AccessLoggingModule;
            if (accessLoggingModule != null)
            {
                accessLoggingModule.LogUnhandledError = LogUnhandledError;
                accessLoggingModule.Filter = Filter;
            }
        }

        private void LogUnhandledError(Exception ex, string correlationId)
        {
            var structuredLogHelper =
                GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IStructuredLogHelper)) as IStructuredLogHelper;
            structuredLogHelper?.LogError(ex, correlationId);
        }

        private bool Filter(HttpRequest request)
        {
            return request.Url.AbsolutePath.Contains("/health");
        }
    }    
```