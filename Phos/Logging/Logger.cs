﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Phos.Enumerations;
using Phos.Models;

namespace Phos.Logging
{
    public class Logger
    {
        public void CreateLogEntry(LogLevel level, string body, DateTimeOffset createdOn)
        {
            string outputFileName = ConfigurationManager.AppSettings["TraceOutputFileName"];
            Stream outputFile = (File.Exists(outputFileName)) ? File.Open(outputFileName, FileMode.Append) : File.Create(outputFileName);

            // Make sure the log file doesn't get crazy big
            FileInfo info = new FileInfo(outputFileName);
            if(info.Length >= 26214400)
            {
                File.Delete(outputFileName);
                File.Create(outputFileName);
            }

            TextWriterTraceListener textListener = new TextWriterTraceListener(outputFile);
            Trace.Listeners.Add(textListener);
            Trace.AutoFlush = true;

            //var entry = new LogEntry(level, body, createdOn);
            Trace.Write(body);

            outputFile.Close();
            // TODO(Tyler): Store log entries in MongoDB by date
        }

        public void GetLogEntry()
        {
            // TODO(Tyler): Implement some way of getting log entries back by ID
        }
    }
}
