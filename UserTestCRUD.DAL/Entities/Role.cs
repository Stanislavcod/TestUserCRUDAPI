using System.Text.Json.Serialization;

namespace UserTestCRUD.DAL.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<User> Users { get; set; } = new();
    }
}
