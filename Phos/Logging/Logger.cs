using System;
using Phos.Enumerations;
using Phos.Models;

namespace Phos.Logging
{
    public class Logger
    {
        public void CreateLogEntry(LogLevel level, string body, DateTimeOffset createdOn)
        {
            var entry = new LogEntry(level, body, createdOn);
            entry.Save();

            // TODO(Tyler): Store log entries in MongoDB by date
        }

        public void GetLogEntry()
        {
            // TODO(Tyler): Implement some way of getting log entries back by ID
        }
    }
}
