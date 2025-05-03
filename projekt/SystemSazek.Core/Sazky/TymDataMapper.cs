using System;
using Microsoft.Data.Sqlite;
namespace SystemSazek.Core.Sazky{

    public class TymDataMapper{
        private string connection_string;
        
        public TymDataMapper( string connection )
        {
            this.connection_string = connection;
        }

        public Tym GetTymById(int id_tym)
        {
            if (id_tym == null)
            {
               return null;
            }

            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string query = @"SELECT * FROM Tym WHERE id_tym = @id_tym";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_tym", id_tym);

                    using (var reader = command.ExecuteReader())
                    {
                        if( reader.HasRows )
                        {
                            reader.Read();
                            var zapas = new Tym 
                            {
                                id_tym = reader["id_tym"] != DBNull.Value ? Convert.ToInt32(reader["id_tym"]) : (int?)null,
                                datum_zalozeni= reader.GetDateTime(reader.GetOrdinal("datum_zalozeni")), 
                                status = reader["status"] as string,
                                nazev = reader["nazev"] as string,
                                misto_zalozeni = reader["misto_zalozeni"] as string,
     
                            };

                            return zapas;
                        }
                        else return null;
                    }
                }
            }

            return null;
        } 



    }
}
