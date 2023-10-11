namespace UserTestCRUD.BusinessLogic.ViewModel
{
    public class UserViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<RoleViewModel> Roles { get; set; } = new();
    }
}
