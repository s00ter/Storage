using System;

namespace Storage.Database.Entities.Interfaces
{
    public interface ITrackable
    {
        public DateTime CreatedAt { get; set; }
    }
}