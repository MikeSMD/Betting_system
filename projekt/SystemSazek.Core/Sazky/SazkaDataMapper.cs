using System;
using Microsoft.Data.Sqlite;
namespace SystemSazek.Core.Sazky{

    public class SazkaDataMapper : IMapperSupertype<int, Sazka>{
        private string connection_string;
        
        public SazkaDataMapper( string connection )
        {
            this.connection_string = connection;
        }

        public List<Sazka> GetSazkyByUzivatel(Uzivatel uzivatel)
        {
            if (uzivatel == null || uzivatel.id_uzivatele == null)
            {
               return null;
            }

            List<Sazka> sazky = new List<Sazka>();

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"SELECT * FROM Sazka WHERE id_uzivatele = @id_uzivatele";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_uzivatele", uzivatel.id_uzivatele);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sazka = new SazkaVP
                            {
                                id_sazka = reader["id_sazka"] != DBNull.Value ? Convert.ToInt32(reader["id_sazka"]) : (int?)null,
                                datum_cas_vytvoreni = reader.GetDateTime(reader.GetOrdinal("datum_cas_vytvoreni")), 
                                datum_cas_uzavreni = reader["datum_cas_uzavreni"] != DBNull.Value ? Convert.ToDateTime(reader["datum_cas_uzavreni"]) : (DateTime?)null,
                                castka = reader.GetDouble(reader.GetOrdinal("castka")),
                                status = reader["status"] as string,
                                uzivatel = uzivatel,
                                polozky = null
                            };

                            sazky.Add(sazka);
                        }
                    }
                }
            

            return sazky;
        }
    }

        public int Save(Sazka sazka)
        {
            if (sazka == null)
            {
                throw new ArgumentNullException(nameof(sazka), "Sázka nemůže být null.");
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"INSERT INTO Sazka (datum_cas_vytvoreni, datum_cas_uzavreni, castka, status, id_uzivatele) VALUES (@datum_cas_vytvoreni, @datum_cas_uzavreni, @castka, @status, @id_uzivatele);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@datum_cas_vytvoreni", sazka.datum_cas_vytvoreni);
                    command.Parameters.AddWithValue("@datum_cas_uzavreni", (object)sazka.datum_cas_uzavreni ?? DBNull.Value);
                    command.Parameters.AddWithValue("@castka", sazka.castka);
                    command.Parameters.AddWithValue("@status", sazka.status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id_uzivatele", sazka.uzivatel?.id_uzivatele ?? (object)DBNull.Value);

                    try
                    {
                        command.ExecuteNonQuery();

                        int insertedId = -1;
                        using (var lastIdCommand = new SqliteCommand("SELECT last_insert_rowid();", connection))
                        {
                            insertedId = Convert.ToInt32(lastIdCommand.ExecuteScalar());
                        }
                        return insertedId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Sazka.Save chyba = " + ex.Message);
                        return -1;
                    }
                }
            }
        }
        public bool Update(Sazka sazka)
        {
            if (sazka == null || sazka.id_sazka == null)
            {
                throw new ArgumentNullException(nameof(sazka), "Sázka nebo její ID nemůže být null.");
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"UPDATE Sazka 
                    SET datum_cas_vytvoreni = @datum_cas_vytvoreni,
                        datum_cas_uzavreni = @datum_cas_uzavreni,
                        castka = @castka,
                        status = @status,
                        id_uzivatele = @id_uzivatele
                            WHERE id_sazka = @id_sazka;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@datum_cas_vytvoreni", sazka.datum_cas_vytvoreni);
                    command.Parameters.AddWithValue("@datum_cas_uzavreni", (object)sazka.datum_cas_uzavreni ?? DBNull.Value);
                    command.Parameters.AddWithValue("@castka", sazka.castka);
                    command.Parameters.AddWithValue("@status", sazka.status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id_uzivatele", sazka.uzivatel?.id_uzivatele ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id_sazka", sazka.id_sazka);

                    try
                    {
                        int r = command.ExecuteNonQuery();
                        if ( r > 0 ) Console.WriteLine("edited");
                        else Console.WriteLine("not edited" + sazka.id_sazka);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Sazka.Update chyba = " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
