using System;
using Phos.Enumerations;

namespace Phos.Models
{
    public class LogEntry
    {
        public LogEntry()
        {
            
        }

        public LogEntry(LogLevel level, object body, DateTimeOffset createdOn)
        {
            Level = level;
            Body = body;
            CreatedOn = createdOn;
        }

        public LogLevel Level { get; set; }
        public object Body { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public void Save()
        {
            
        }

        public override string ToString()
        {
            if(Body is PlexRequest)
            {
                var plexRequest = Body as PlexRequest;
                return $"LogLevel: {Level} | Body: {plexRequest.Metadata.Title} | Episode: {plexRequest.Metadata.Index} | CreatedOn: {CreatedOn} | ";
            }
            else if (Body is Exception)
            {
                var error = Body as Exception;
                return $"LogLevel: {Level} | Body: {error.Message} | CreatedOn: {CreatedOn}";
            }

            return this.ToString();
        }
    }
}
