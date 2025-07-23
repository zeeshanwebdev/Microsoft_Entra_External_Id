namespace TaskManagementApi.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public Guid Owner { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
