using JsonToDatabase.Common;
using JsonToDatabase.Common.Resources;
using JsonToDatabase.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JsonToDatabase.BAL
{
    public class ReportFactory
    {
        private readonly DbHelper dbHelper;
        private readonly Mail mail;
        private string _identity;
        public ReportFactory()
        {
            dbHelper = new DbHelper();
            mail = new Mail();
            _identity = WindowsIdentity.GetCurrent().Name.ToString(); 
        }
        string queueID = string.Empty;
        public  void BindQueueID()
        {
            try
            {
                //EncryptDecrypt.Encrypt("rpa_tst_prtl_1");
                //EncryptDecrypt.Encrypt("yT4tTZTa");
                var queueData = dbHelper.BindQueueID();
                    if (queueData.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in queueData.Rows)
                        {
                            queueID = oRow["Process_Queue_ID"].ToString();

                        string url = Assets.EndPoint + queueID; //"4217";

                            HttpClient client = new HttpClient(new HttpClientHandler()
                            {
                                Credentials = new NetworkCredential()
                                {
                                    UserName = EncryptDecrypt.Decrypt(Assets.NUID),
                                    Password = EncryptDecrypt.Decrypt(Assets.NUID_Password),
                                    Domain = Assets.Domain
                                }
                            })
                            {
                                BaseAddress = new Uri(url)
                            };

                            HttpResponseMessage response = client.GetAsync(url).Result;

                            var dt = CreateTable();

                            if (response.IsSuccessStatusCode)
                            {
                                var httpResponseResult = response.Content.ReadAsStringAsync().Result;
                                var data = JObject.Parse(httpResponseResult);

                                var list = data["value"].ToList();

                                foreach (var item in list)
                                {
                                    string qID = item["QueueDefinitionId"].ToString();
                                    string status = item["Status"].ToString();
                                    string Key = item["Key"].ToString();
                                    string ProcessingExceptionType = item["ProcessingExceptionType"].ToString();
                                    string StartProcessing = item["StartProcessing"].ToString();
                                    string EndProcessing = item["EndProcessing"].ToString();
                                    string Id = item["Id"].ToString();

                                    dt.Rows.Add(Id, Key, qID, StartProcessing, EndProcessing, status, ProcessingExceptionType);

                                }
                                dbHelper.UpdateTable(dt);
                            }
                        }
                        var a = mail.Send(Assets.Mail_From, Assets.Mail_To, Assets.Mail_Subject, Assets.Mail_Body);
                    }
            }
            catch (Exception ex)
            {
                dbHelper.ErrorLogging(queueID, ex.Message, "Application", ex.StackTrace, Assets.ProcessName, _identity);
            }
        }
        public DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Clear();
                dt.Columns.Add("Row_Id", typeof(string));
                dt.Columns.Add("Key", typeof(string));
                dt.Columns.Add("Queue_Id", typeof(string));
                dt.Columns.Add("StartTime", typeof(string));
                dt.Columns.Add("EndTime", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("ExceptionType", typeof(string));
            }
            catch (Exception)
            {

                throw;
            }
    
            return dt;
        }
    }
}
