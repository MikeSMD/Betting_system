using System;
using Microsoft.Data.Sqlite;
namespace SystemSazek.Core.Sazky{

    public class PolozkaDataMapper : IMapperSupertype<bool, Polozka> {
        private string connection_string;
        
        public PolozkaDataMapper( string connection )
        {
            this.connection_string = connection;
        }

        public List<Polozka> GetPolozkyBySazka(Sazka sazka)
        {
            if ( sazka == null || sazka.id_sazka == null )
            {
               return null;
            }

            List<Polozka> polozky = new List<Polozka>();

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"SELECT * FROM sazka_zapas WHERE id_sazka = @id_sazka";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_sazka", sazka.id_sazka);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PolozkaVP p = new PolozkaVP
                            {
                                id_sazka_zapas = reader["id_sazka_zapas"] != DBNull.Value ? Convert.ToInt32(reader["id_sazka_zapas"]) : (int?)null,
                                vsazeno_na = reader.GetInt32(reader.GetOrdinal("vsazeno_na")),
                                sazka = sazka,
                                zapas = null
                            };

                            p.set_id_zapas(reader.GetInt32(reader.GetOrdinal("id_zapas")));

                            polozky.Add(p);
                        }
                    }
                }
            }

            return polozky;
        } 

        public bool Save(Polozka polozka)
        {
            if (polozka == null || polozka.sazka.id_sazka == null || polozka.zapas.id_zapas == null || polozka.vsazeno_na == null)
            {
                if ( polozka.sazka.id_sazka == null ) Console.WriteLine("je to sazka");
                        Console.WriteLine("Polozka.Save chyba = hodnoty nejsou spravne nastaveny..");
               return false;
            }

            if (!polozka.validace_polozky())
            {
                throw new InvalidOperationException("Položka není validní a nelze ji uložit.");
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"
                    INSERT INTO sazka_zapas (id_sazka, id_zapas, vsazeno_na)
                    VALUES (@id_sazka, @id_zapas, @vsazeno_na);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_sazka", polozka.sazka.id_sazka);
                    command.Parameters.AddWithValue("@id_zapas", polozka.zapas.id_zapas);
                    command.Parameters.AddWithValue("@vsazeno_na", polozka.vsazeno_na);

                      try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Polozka.Save chyba = " + ex.Message);
                            return false;
                    }
                }
            }
        }
        public bool Update(Polozka polozka)
        {
            if (polozka == null || polozka.id_sazka_zapas == null ||  polozka.sazka.id_sazka == null || polozka.zapas.id_zapas == null || polozka.vsazeno_na == null)
            {
                return false;
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"UPDATE sazka_zapas SET id_zapas = @id_zapas, vsazeno_na = @vsazeno_na, id_sazka = @id_sazka WHERE id_sazka_zapas = @id_sazka_zapas;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_sazka_zapas", polozka.id_sazka_zapas);
                    command.Parameters.AddWithValue("@id_zapas", polozka.zapas.id_zapas);
                    command.Parameters.AddWithValue("@vsazeno_na", polozka.vsazeno_na);
                    command.Parameters.AddWithValue("@id_sazka", polozka.sazka.id_sazka);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Polozka.Update chyba = " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool Delete(Polozka polozka)
        {
            if (polozka == null || polozka.id_sazka_zapas == null)
            {
                return false;
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"DELETE FROM sazka_zapas WHERE id_sazka_zapas = @id_sazka_zapas;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_sazka", polozka.sazka.id_sazka);
                    command.Parameters.AddWithValue("@id_zapas", polozka.zapas.id_zapas);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Polozka.Delete chyba = " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
