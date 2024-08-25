using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Models;
using MySql.Data.MySqlClient;

namespace APP.Data
{
    internal class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=GymManagement;Uid=APP1;Pwd=12345;";

        public IEnumerable<Member> GetMembers()
        {
            var members = new List<Member>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Members", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(new Member
                        {
                            MemberID = reader.GetInt32("MemberID"),
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            MembershipType = reader.GetString("MembershipType"),
                            MembershipStartDate = reader.GetDateTime("MembershipStartDate"),
                            MembershipEndDate = reader.GetDateTime("MembershipEndDate")
                        });
                    }
                }
            }
            return members;
        }

        public void AddMember(Member member)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Members (FullName, PhoneNumber, MembershipType, MembershipStartDate, MembershipEndDate) VALUES (@FullName, @PhoneNumber, @MembershipType, @MembershipStartDate, @MembershipEndDate)", connection);
                command.Parameters.AddWithValue("@FullName", member.FullName);
                command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                command.Parameters.AddWithValue("@MembershipType", member.MembershipType);
                command.Parameters.AddWithValue("@MembershipStartDate", member.MembershipStartDate);
                command.Parameters.AddWithValue("@MembershipEndDate", member.MembershipEndDate);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateMember(Member member)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE Members SET FullName = @FullName, PhoneNumber = @PhoneNumber, MembershipType = @MembershipType, MembershipStartDate = @MembershipStartDate, MembershipEndDate = @MembershipEndDate WHERE MemberID = @MemberID", connection);
                command.Parameters.AddWithValue("@FullName", member.FullName);
                command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                command.Parameters.AddWithValue("@MembershipType", member.MembershipType);
                command.Parameters.AddWithValue("@MembershipStartDate", member.MembershipStartDate);
                command.Parameters.AddWithValue("@MembershipEndDate", member.MembershipEndDate);
                command.Parameters.AddWithValue("@MemberID", member.MemberID);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteMember(int memberId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Members WHERE MemberID = @MemberID", connection);
                command.Parameters.AddWithValue("@MemberID", memberId);
                command.ExecuteNonQuery();
            }
        }

        public void ExtendMembership(int memberId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE Members SET MembershipEndDate = DATE_ADD(MembershipEndDate, INTERVAL 30 DAY) WHERE MemberID = @MemberID", connection);
                command.Parameters.AddWithValue("@MemberID", memberId);
                command.ExecuteNonQuery();
            }
        }

        public decimal GetTotalRevenue()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT SUM(Amount) FROM Revenue", connection);
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

    }
}
