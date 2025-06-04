namespace API.Dtos.Common
{
    public class CommentArchiveDto
    {
        public int CommentArchiveID { get; set; }
        public int? Value { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public int? UserID { get; set; }
        public string CreateName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}