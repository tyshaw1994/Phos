using System;
using Phos.Enumerations;

namespace Phos.Models
{
    public class LogEntry
    {
        public LogEntry()
        {
            
        }

        public LogEntry(LogLevel level, string body, DateTimeOffset createdOn)
        {
            Level = level;
            Body = body;
            CreatedOn = createdOn;
        }

        public LogLevel Level { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public void Save()
        {
            throw new NotImplementedException("Saving logs is not yet implemented.");
        }
    }
}
