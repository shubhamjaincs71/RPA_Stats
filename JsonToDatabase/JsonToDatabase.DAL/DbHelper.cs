using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using JsonToDatabase.Common.Resources;

namespace JsonToDatabase.DAL
{
    public class DbHelper
    {
        public DataTable BindQueueID()
        {
            try
            {
                var reportTable = getDataTable(Assets.queueProcName);
                return reportTable;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable getDataTable(string procName)
        {
            DataTable dt = new DataTable();
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(procName))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;

        }
        public void UpdateTable(DataTable dataTable)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Assets.InserDataProcName;
                cmd.Parameters.AddWithValue("@HRMS_Report_Datatable", dataTable);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void ErrorLogging(string row_Id = null, string ErrorMessege = null, string ExceptionType = null,
            string StackTrace = null, string ProcessName = null, string UserId = null)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Assets.ErrorLoggingProcName;
                cmd.Parameters.Add("@Row_Id", SqlDbType.NVarChar).Value = row_Id;
                cmd.Parameters.Add("@ErrorMessege", SqlDbType.NVarChar).Value = ErrorMessege;
                cmd.Parameters.Add("@ExceptionType", SqlDbType.NVarChar).Value = ExceptionType;
                cmd.Parameters.Add("@StackTrace", SqlDbType.NVarChar).Value = StackTrace;
                cmd.Parameters.Add("@ProcessName", SqlDbType.NVarChar).Value = ProcessName;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = UserId;

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
