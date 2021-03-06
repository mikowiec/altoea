﻿

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Altoea.Framework.Mvc.Extend;

namespace Altoea.Framework.Services.Logging
{
    public class FileLogger : ILogger
    {
        public static string Path = "Logs";
        public const string TitleTemplate = "----------------------------------------------------------------\r\nEvent Time: {0}\r\nError Message:\r\n";
        public const string Split = "----------------------------------------------------------------";
        public const string FileTemplate = "{0}.log";
        public const string DateNameTemplate = "yyyy-MM-dd";
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileLogger(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        #region private method
        void WriteInfo(string msg)
        {
            lock (Path)
            {
                string logPath = GetLogFile();
                FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine(string.Format(TitleTemplate, DateTime.Now.ToString("G")) + msg);
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request != null)
                {
                    if (_httpContextAccessor.HttpContext.Request.Headers != null)
                    {
                        writer.WriteLine("Headers:");
                        foreach (var item in _httpContextAccessor.HttpContext.Request.Headers)
                        {
                            writer.WriteLine($"{item.Key}:{item.Value}");
                        }
                    }
                    
                    writer.WriteLine(_httpContextAccessor.HttpContext.Request.GetAbsoluteUrl());
                }
                writer.WriteLine(Split);
                writer.Flush();
                fs.Flush();
                writer.Dispose();
                fs.Dispose();
            }
        }
        string GetLogFile()
        {
            string logPath = System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, Path);
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string fileName = string.Format(FileTemplate, DateTime.Now.ToString(DateNameTemplate));
            return System.IO.Path.Combine(logPath, fileName);
        }
        #endregion
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel) && exception != null)
            {
                WriteInfo(exception.ToString());
            }
        }
    }
}
