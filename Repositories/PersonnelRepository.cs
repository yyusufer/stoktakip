using MySql.Data.MySqlClient;
using Takip.Models;
using Takip.Services;

namespace Takip.Repositories
{
    public class PersonnelRepository
    {
        private readonly DatabaseService _dbService;

        public PersonnelRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<Personnel> GetAllPersonnel()
        {
            var list = new List<Personnel>();

            using var conn = _dbService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, name FROM personnel", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Personnel
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name")
                });
            }

            return list;
        }
    }
}
