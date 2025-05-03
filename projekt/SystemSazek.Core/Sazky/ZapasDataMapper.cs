using System;
using Microsoft.Data.Sqlite;
namespace SystemSazek.Core.Sazky{

    public class ZapasDataMapper{
        private string connection_string;
        
        public ZapasDataMapper( string connection )
        {
            this.connection_string = connection;
        }

        public Zapas GetZapasById(int id_zapas)
        {
            if (id_zapas == null)
            {
               return null;
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"SELECT * FROM Zapas WHERE id_zapas = @id_zapas";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_zapas", id_zapas);

                    using (var reader = command.ExecuteReader())
                    {
                        if( reader.HasRows )
                        {
                            reader.Read();
                            TymDataMapper tdm = new TymDataMapper(this.connection_string);
                            var zapas = new Zapas
                            {
                                id_zapas = reader["id_zapas"] != DBNull.Value ? Convert.ToInt32(reader["id_zapas"]) : (int?)null,
                                datum_cas_zacatku= reader.GetDateTime(reader.GetOrdinal("datum_cas_zacatku")), 
                                datum_cas_ukonceni = reader["datum_cas_ukonceni"] != DBNull.Value ? Convert.ToDateTime(reader["datum_cas_ukonceni"]) : (DateTime?)null,
                                skore_domaci = reader["skore_domaci"] != DBNull.Value ? Convert.ToInt32(reader["skore_domaci"]) : (int?)null,
                                skore_hoste = reader["skore_hoste"] != DBNull.Value ? Convert.ToInt32(reader["skore_hoste"]) : (int?)null,                                
                                status = reader["status"] as string,
                                kurz_domaci = reader.GetDouble(reader.GetOrdinal("kurz_domaci")),
                                kurz_shoda = reader.GetDouble(reader.GetOrdinal("kurz_shoda")),
                                kurz_hoste = reader.GetDouble(reader.GetOrdinal("kurz_hoste")),
                                tym_domaci = tdm.GetTymById( reader.GetInt32(reader.GetOrdinal("id_tym_domaci")) ),
                                tym_hoste =  tdm.GetTymById( reader.GetInt32(reader.GetOrdinal("id_tym_hoste")) )
                            };

                            return zapas;
                        }
                        else return null;
                    }
                }
            }

            return null;
        }

        public List<Zapas> GetAllZapasy()
        {
            using (var connection = new SqliteConnection(this.connection_string))
            {
		Console.WriteLine(this.connection_string);
                connection.Open();

                string query = "SELECT * FROM Zapas";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        List<Zapas> zapasy = new List<Zapas>();
                        TymDataMapper tdm = new TymDataMapper(this.connection_string);

                        while (reader.Read())
                        {
                            var zapas = new Zapas
                            {
                                id_zapas = reader["id_zapas"] != DBNull.Value ? Convert.ToInt32(reader["id_zapas"]) : (int?)null,
                                         datum_cas_zacatku = reader.GetDateTime(reader.GetOrdinal("datum_cas_zacatku")),
                                         datum_cas_ukonceni = reader["datum_cas_ukonceni"] != DBNull.Value ? Convert.ToDateTime(reader["datum_cas_ukonceni"]) : (DateTime?)null,
                                         skore_domaci = reader["skore_domaci"] != DBNull.Value ? Convert.ToInt32(reader["skore_domaci"]) : (int?)null,
                                         skore_hoste = reader["skore_hoste"] != DBNull.Value ? Convert.ToInt32(reader["skore_hoste"]) : (int?)null,
                                         status = reader["status"] as string,
                                         kurz_domaci = reader.GetDouble(reader.GetOrdinal("kurz_domaci")),
                                         kurz_shoda = reader.GetDouble(reader.GetOrdinal("kurz_shoda")),
                                         kurz_hoste = reader.GetDouble(reader.GetOrdinal("kurz_hoste")),
                                         tym_domaci = tdm.GetTymById(reader.GetInt32(reader.GetOrdinal("id_tym_domaci"))),
                                         tym_hoste = tdm.GetTymById(reader.GetInt32(reader.GetOrdinal("id_tym_hoste")))
                            };

                            zapasy.Add(zapas);
                        }

                        return zapasy;
                    }
                }
            }
        } 

    }
}
