using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Specialty
  {

    public int _specialtyId { get; set; }
    public string _specialtyName { get; set; }
    public string _specialtyPrice { get; set; }

    public Specialty (string specialtyName, string specialtyPrice)
    {
      // _specialtyId = specialtyId;
      _specialtyName = specialtyName;
      _specialtyPrice = specialtyPrice;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialties;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Specialty> GetAll()
    {
      List<Specialty> allSpecialties = new List<Specialty> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT id, name, price FROM specialties;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        // int clientId = rdr.GetInt32(0);
        // string clientName = rdr.GetString(1);
        // string clientPhone = rdr.GetString(2);

        Specialty newSpecialty = new Specialty(
          rdr.GetString(1),
          rdr.GetString(2)
        );
        newSpecialty._specialtyId = rdr.GetInt32(0);
        allSpecialties.Add(newSpecialty);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allSpecialties;
    }

    public override bool Equals(System.Object otherSpecialty)
    {
      if (!(otherSpecialty is Specialty))
      {
        return false;
      }
      else
      {
        Specialty newSpecialty = (Specialty) otherSpecialty;
        bool idEquality = this._specialtyId.Equals(newSpecialty._specialtyId);
        bool nameEquality = this._specialtyName.Equals(newSpecialty._specialtyName);
        return (idEquality && nameEquality);
      }
    }


    public int Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO specialties (name, price) VALUES (@name, @price);";
      cmd.Parameters.AddWithValue("@name", _specialtyName);
      cmd.Parameters.AddWithValue("@price", _specialtyPrice);
      cmd.ExecuteNonQuery();
      _specialtyId = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return _specialtyId;
    }

    public static Specialty Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM specialties WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int specialtyId = 0;
      string specialtyName = "";
      string specialtyPrice = "";

      while(rdr.Read())
      {
        specialtyId = rdr.GetInt32(0);
        specialtyName = rdr.GetString(1);
        specialtyPrice = rdr.GetString(2);
      }

      Specialty newSpecialty = new Specialty(specialtyName, specialtyPrice);
      newSpecialty._specialtyId = specialtyId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newSpecialty;
    }

    public void AddStylist(Stylist newStylist)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO services (stylists_id, specialties_id) VALUES (@StylistId, @SpecialtyId);";
      cmd.Parameters.AddWithValue("@StylistId", newStylist.GetId());
      cmd.Parameters.AddWithValue("@SpecialtyId", _specialtyId);

      // MySqlParameter stylists_id = new MySqlParameter();
      // stylists_id.ParameterName = "@StylistId";
      // stylists_id.Value = newStylist._id;
      // cmd.Parameters.Add(stylists_id);
      // cmd.Parameters.AddWithValue("@StylistId", _specialtyName);
      //
      // MySqlParameter specialties_id = new MySqlParameter();
      // specialties_id.ParameterName = "@SpecialtyId";
      // specialties_id.Value = _specialtyId;
      // cmd.Parameters.Add(specialties_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Stylist> GetStylists()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT stylists.* FROM specialties
          JOIN services ON (specialties.id = services.specialties_id)
          JOIN stylists ON (services.stylists_id = stylists.id)
          WHERE specialties.id = @SpecialtyId;";

      MySqlParameter specialtyIdParameter = new MySqlParameter();
      specialtyIdParameter.ParameterName = "@SpecialtyId";
      specialtyIdParameter.Value = _specialtyId;
      cmd.Parameters.Add(specialtyIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Stylist> stylists = new List<Stylist>{};
      while(rdr.Read())
      {
        // int stylistId = rdr.GetInt32(0);
        string stylistName = rdr.GetString(1);
        string bio = rdr.GetString(2);
        // string stylistPrice = rdr.GetString(2);
        Stylist newStylist = new Stylist(stylistName, bio);
        stylists.Add(newStylist);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return stylists;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM specialties WHERE id = @SpecialtyId; DELETE FROM services WHERE specialties_id = @SpecialtyId;";
      MySqlParameter specialtyIdParameter = new MySqlParameter();
      specialtyIdParameter.ParameterName = "@SpecialtyId";
      specialtyIdParameter.Value = this._specialtyId;
      cmd.Parameters.Add(specialtyIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
