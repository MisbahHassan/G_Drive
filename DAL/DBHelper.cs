﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    internal class DBHelper : IDisposable
    {
        String _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        SqlConnection _conn = null;
        public DBHelper()
        {
            try
            {
                _conn = new SqlConnection(_connStr);
                _conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int ExecuteQuery(String sqlQuery)
        {
            try
            {
                SqlCommand command = new SqlCommand(sqlQuery, _conn);
                var count = command.ExecuteNonQuery();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        public Object ExecuteScalar(String sqlQuery)
        {
            try
            {
                SqlCommand command = new SqlCommand(sqlQuery, _conn);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public SqlDataReader ExecuteReader(String sqlQuery)
        {
            try
            {


                SqlCommand command = new SqlCommand(sqlQuery, _conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public void Dispose()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }
    }
}
