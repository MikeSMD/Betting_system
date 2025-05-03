using System;
using Microsoft.Data.Sqlite;
namespace SystemSazek.Core.Sazky{



    public class UzivatelDataMapper : IMapperSupertype<bool, Uzivatel>{
        private string connection_string;
        
        public UzivatelDataMapper( string connection )
        {
            this.connection_string = connection;

        }
    
        public bool Save( Uzivatel uzivatel )
        {
            using ( var connection = new SqliteConnection( this.connection_string ) )
            {

                connection.Open();
                string insertUzivatel = "INSERT INTO uzivatel (jmeno, prijmeni, email,datum_narozeni, heslo,sul, aktivni,stat, mesto, ulice, psc" +
                        ( uzivatel.prostredni_jmeno != null ? ", prostredni_jmeno" : "" ) +
                        ( uzivatel.telefon != null ? ", telefon" : "" ) + 
                        ") VALUES (@jmeno,@prijmeni,@email,@datum_narozeni, @heslo,@sul, @aktivni,@stat, @mesto, @ulice, @psc" +
                        ( uzivatel.prostredni_jmeno != null ? ", @prostredni_jmeno" : "" ) +
                        ( uzivatel.telefon != null ? ", @telefon" : "" ) + 
                        ")";


                using ( var command = new SqliteCommand( insertUzivatel, connection ) )
                    {
                        command.Parameters.AddWithValue("@jmeno", uzivatel.jmeno);
                        command.Parameters.AddWithValue("@prijmeni", uzivatel.prijmeni);
                        command.Parameters.AddWithValue("@email", uzivatel.email);
                        command.Parameters.AddWithValue("@datum_narozeni", uzivatel.datum_narozeni.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@heslo", uzivatel.heslo);
                        command.Parameters.AddWithValue("@sul", uzivatel.sul);
                        command.Parameters.AddWithValue("@aktivni", uzivatel.aktivni);
                        command.Parameters.AddWithValue("@stat", uzivatel.stat);
                        command.Parameters.AddWithValue("@mesto", uzivatel.mesto);
                        command.Parameters.AddWithValue("@ulice", uzivatel.ulice);
                        command.Parameters.AddWithValue("@psc", uzivatel.psc);
                        if ( uzivatel.prostredni_jmeno != null ) 
                        {
                            command.Parameters.AddWithValue("@prostredni_jmeno", uzivatel.prostredni_jmeno);
                        }
                        
                        if ( uzivatel.telefon != null ) 
                        {
                            command.Parameters.AddWithValue("@telefon", uzivatel.telefon);
                        }

                        try
                        {
                            command.ExecuteNonQuery();
                            return true;
                        }
                        catch ( Exception ex )
                        {
                            Console.WriteLine("udm.Save chyba = " + ex.Message);
                            return false;
                        }
                    }
            }
        }



        public Uzivatel GetUzivatelByEmail(string email)
        {
            using ( var connection = new SqliteConnection( this.connection_string ) )
            {

                connection.Open();
                string selectUzivatel = "SELECT * FROM Uzivatel WHERE email LIKE @email";


                using ( var command = new SqliteCommand( selectUzivatel, connection ) )
                {
                    command.Parameters.AddWithValue("@email", email);

                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                Uzivatel uzivatel = new UzivatelVP
                                {
                                    id_uzivatele = reader["id_uzivatele"] != DBNull.Value ? Convert.ToInt32(reader["id_uzivatele"]) : (int?)null,
                                    jmeno = reader["jmeno"] as string,
                                    prostredni_jmeno = reader["prostredni_jmeno"] as string,
                                    prijmeni = reader["prijmeni"] as string,
                                    email = reader["email"] as string,
                                    telefon = reader["telefon"] as string,
                                    datum_narozeni = reader["datum_narozeni"] != DBNull.Value ? Convert.ToDateTime(reader["datum_narozeni"]) : DateTime.MinValue,
                                    heslo = reader["heslo"] as string,
                                    sul = reader["sul"] as string,
                                    stat = reader["stat"] as string,
                                    mesto = reader["mesto"] as string,
                                    ulice = reader["ulice"] as string,
                                    psc = reader["psc"] as string,
                                    aktivni = Convert.ToBoolean(reader["aktivni"])
                                };
                                if(uzivatel.id_uzivatele == null ) Console.WriteLine("JSEM NULL");

                                 return uzivatel;
                            }
                            else
                            {
                                Console.WriteLine("NENALEZEN");
                                return null;
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        Console.WriteLine("udm.getById chyba = " + ex.Message);
                        return null;
                    }
                }
            }

        }
        


        public bool existuje_email(string email)
        {
            using ( var connection = new SqliteConnection( this.connection_string ) )
            {

                connection.Open();
                string selectUzivatel = "SELECT * FROM Uzivatel WHERE email LIKE @email";


                using ( var command = new SqliteCommand( selectUzivatel, connection ) )
                {
                    command.Parameters.AddWithValue("@email", email);

                        try
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    catch ( Exception ex )
                    {
                        Console.WriteLine("udm.exist_email chyba = " + ex.Message);
                        return false;
                    }
                }
            }

        }

        public bool Update(Uzivatel uzivatel)
        {
            using (var connection = new SqliteConnection(this.connection_string))
            {
                connection.Open();

                string updateUzivatel = "UPDATE uzivatel SET jmeno = @jmeno, prijmeni = @prijmeni, email = @email, datum_narozeni = @datum_narozeni, heslo = @heslo, sul = @sul, aktivni = @aktivni, stat = @stat, mesto = @mesto, ulice = @ulice, psc = @psc " + (uzivatel.prostredni_jmeno != null ? ",prostredni_jmeno = @prostredni_jmeno " : "") + (uzivatel.telefon != null ? ",telefon = @telefon " : "") + "WHERE id_uzivatele = @id_uzivatele";

                using (var command = new SqliteCommand(updateUzivatel, connection))
                {
                    command.Parameters.AddWithValue("@id_uzivatele", uzivatel.id_uzivatele);
                    command.Parameters.AddWithValue("@jmeno", uzivatel.jmeno);
                    command.Parameters.AddWithValue("@prijmeni", uzivatel.prijmeni);
                    command.Parameters.AddWithValue("@email", uzivatel.email);
                    command.Parameters.AddWithValue("@datum_narozeni", uzivatel.datum_narozeni.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@heslo", uzivatel.heslo);
                    command.Parameters.AddWithValue("@sul", uzivatel.sul);
                    command.Parameters.AddWithValue("@aktivni", uzivatel.aktivni);
                    command.Parameters.AddWithValue("@stat", uzivatel.stat);
                    command.Parameters.AddWithValue("@mesto", uzivatel.mesto);
                    command.Parameters.AddWithValue("@ulice", uzivatel.ulice);
                    command.Parameters.AddWithValue("@psc", uzivatel.psc);

                    if (uzivatel.prostredni_jmeno != null)
                    {
                        command.Parameters.AddWithValue("@prostredni_jmeno", uzivatel.prostredni_jmeno);
                    }

                    if (uzivatel.telefon != null)
                    {
                        command.Parameters.AddWithValue("@telefon", uzivatel.telefon);
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Uzivatel.Update chyba = " + ex.Message);
                        return false;
                    }
                }
            }
        }

    }
}
