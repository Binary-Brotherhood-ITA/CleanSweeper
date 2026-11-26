namespace Core
{
    public class WorkTask
    {
        public int Id { get; set; }
        public int CreatedByUserID { get; set; }
        public string Address { get; set; } 
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }
        public string? WorkTaskNote { get; set; }
        public int? EstWorkTime { get; set; }
        public int? AssignedToUserID { get; set; }
    }
}