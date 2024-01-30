namespace DiscussionForum.Models.APIModels
{
    public class SingleUserDTO
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? Score { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string RoleName { get; set; }

    }
}
