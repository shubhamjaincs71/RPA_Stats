using JsonToDatabase.BAL;
using JsonToDatabase.Common;
using JsonToDatabase.Common.Resources;
using JsonToDatabase.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace JsonToDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter text that needs to be encrypted..");
            //string data = Console.ReadLine();
            //EncryptDecrypt.Encrypt(data);
            //Console.ReadLine();
            ReportFactory reportFactory = new ReportFactory();
            try
            {
                reportFactory.BindQueueID();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }  
    }
}
