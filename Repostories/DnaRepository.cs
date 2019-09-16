using myMicroservice.Framework;
using myMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace myMicroservice.Repostories
{
    public class DnaRepository
    {

        public void InsertDna(List<string> dna, bool isMutant)
        {
            using (var connection = DBConnection.GetNewConnection())
            { 
               using (var sqlcommand = new SqlCommand())
               {
                    connection.Open();
                    sqlcommand.Connection = connection;
                    sqlcommand.CommandText = "ADD_DNA_SP";
                    sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlcommand.Parameters.Add("@dna", System.Data.SqlDbType.VarChar).Value = "[" + string.Join(',', dna) + "]";
                    sqlcommand.Parameters.Add("@hashcode", System.Data.SqlDbType.VarChar).Value = dna.GetDeterministicHashCode() ;
                    sqlcommand.Parameters.Add("@isMutant", System.Data.SqlDbType.Bit).Value = isMutant? 1: 0;

                    sqlcommand.ExecuteNonQuery();
                }               
            }
        }

        public MutantStatistic GetMutantStatistic()
        {
            var mutantStatistic = new MutantStatistic();
            
            using (var sqlcommand = new SqlCommand())
            {                    
                lock(DBConnection.ConnectedInstance)
                {
                    sqlcommand.Connection = DBConnection.ConnectedInstance;
                    sqlcommand.CommandText = "GET_STATS_SP";
                    sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = sqlcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mutantStatistic.CountHumanDna = Convert.ToInt32(reader["HUMAN_ADN_QTY"]);
                            mutantStatistic.CountMutantDna = Convert.ToInt32(reader["MUTANT_ADN_QTY"]);
                        }
                    }
                }
            }
            return mutantStatistic;
        }
    }
}
