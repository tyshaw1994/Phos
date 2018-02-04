using System;
using Phos.Enumerations;

namespace Phos.Models
{
    public class LogEntry
    {
        public LogEntry()
        {
            
        }

        public LogEntry(LogLevel level, PlexRequest body, DateTimeOffset createdOn)
        {
            Level = level;
            Body = body;
            CreatedOn = createdOn;
        }

        public LogLevel Level { get; set; }
        public PlexRequest Body { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public void Save()
        {
            
        }

        public override string ToString()
        {
            return $"LogLevel: {Level} | Body: {Body.Metadata.Title} | CreatedOn: {CreatedOn} | ";
        }
    }
}
