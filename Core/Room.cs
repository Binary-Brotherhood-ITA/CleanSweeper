using System.Collections.Generic;

namespace Core
{
    public class Room
    {
        public int Id { get; set; }
        public int WorkTaskID { get; set; }
        public string Name { get; set; } 
        public int Area { get; set; }
        public string Type { get; set; } 
        public string? RoomNote { get; set; }
        public List<int> CleaningTaskIDs { get; set; } = new List<int>();
    }
}