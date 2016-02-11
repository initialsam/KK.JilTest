using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Jil;
using System.IO.Compression;
using System.Threading;

namespace JsonNetVSJil
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ref: http://blog.darkthread.net/post-2012-06-09-json-net-performance.aspx
            //CreateSerializedData();
            //TestJsonNet();

            //TestJil();
            //JilDateTime();
        }

        private static void JilDateTime()
        {
            var dt = DateTime.Now;

            Console.WriteLine(JSON.Serialize(dt));
            // 二種指定 Options 選擇方式
            Console.WriteLine(JSON.Serialize(dt, new Options(dateFormat: Jil.DateTimeFormat.ISO8601)));
            Console.WriteLine(JSON.Serialize(dt, Options.ISO8601));

            Console.WriteLine(JSON.Deserialize<DateTime>(JSON.Serialize(dt)).ToLocalTime());
            Console.Read();
        }

        private static void TestJil()
        {
            //隨機假造20萬筆User資料
            List<User> bigList = GenSimData();
            string fileName = "serialized.data";
            int indexToTest = 1024; //用來比對測試的筆數
            //序列化前取出第indexToTest筆資料的顯示內容
            string beforeSer = bigList[indexToTest].Display, afterDeser = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //將List<User> JSON化
            string json1 = JSON.Serialize(bigList, Options.ISO8601);
            //string json1 = JSON.Serialize<List<User>>(bigList);
            //string json1 = JSON.SerializeDynamic(bigList);
            sw.Stop();
            Console.WriteLine("Serialization: {0:N0}ms", sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            //由檔案字串反序列化還原回List<User>
            using (FileStream stm = new FileStream(fileName, FileMode.Open))
            {
                //還原後一樣取出第indexToTest筆的User顯示內容
                afterDeser = (JSON.Deserialize<List<User>>(json1,Options.ISO8601))
                             [indexToTest].Display;
            }
            sw.Stop();
            Console.WriteLine("Deserialization: {0:N0}ms", sw.ElapsedMilliseconds);

            //比對還原後的資料是否相同
            Console.WriteLine("Before: {0}", beforeSer);
            Console.WriteLine("After: {0}", afterDeser);
            Console.WriteLine("Pass Test: {0}", beforeSer.Equals(afterDeser));
            Console.Read();
        }

        private static void TestJsonNet()
        {
            //隨機假造20萬筆User資料
            List<User> bigList = GenSimData();
            string fileName = "serialized.data";
            int indexToTest = 1024; //用來比對測試的筆數
            //序列化前取出第indexToTest筆資料的顯示內容
            string beforeSer = bigList[indexToTest].Display, afterDeser = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //將List<User> JSON化
            string json1 = JsonConvert.SerializeObject(bigList);
            sw.Stop();
            Console.WriteLine("Serialization: {0:N0}ms", sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            //由檔案字串反序列化還原回List<User>
            using (FileStream stm = new FileStream(fileName, FileMode.Open))
            {
                //還原後一樣取出第indexToTest筆的User顯示內容
                afterDeser = (JsonConvert.DeserializeObject<List<User>>(json1))
                             [indexToTest].Display;
            }
            sw.Stop();
            Console.WriteLine("Deserialization: {0:N0}ms", sw.ElapsedMilliseconds);

            //比對還原後的資料是否相同
            Console.WriteLine("Before: {0}", beforeSer);
            Console.WriteLine("After: {0}", afterDeser);
            Console.WriteLine("Pass Test: {0}", beforeSer.Equals(afterDeser));
            Console.Read();
        }

        private static void CreateSerializedData()
        {
            //隨機假造20萬筆User資料
            List<User> bigList = GenSimData();
            string fileName = "serialized.data";
            int indexToTest = 1024; //用來比對測試的筆數
            //序列化前取出第indexToTest筆資料的顯示內容
            string beforeSer = bigList[indexToTest].Display, afterDeser = null;

            DataContractSerializer dcs = new DataContractSerializer(bigList.GetType());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //將List<User>序列化後寫入檔案
            using (FileStream stm = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(stm, bigList);
            }
            //using (FileStream stm = new FileStream(fileName, FileMode.Create))
            //{
            //    //用GZipStream把FileStream包起來
            //    using (GZipStream zip = new GZipStream(stm, CompressionMode.Compress))
            //    {
            //        //序列化結果改寫入GZipStream
            //        dcs.WriteObject(zip, bigList);
            //    }
            //}

            sw.Stop();
            Console.WriteLine("Serialization: {0:N0}ms", sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            //由檔案反序列化還原回List<User>
            using (FileStream stm = new FileStream(fileName, FileMode.Open))
            {
                //還原後一樣取出第indexToTest筆的User顯示內容
                afterDeser = (dcs.ReadObject(stm) as List<User>)[indexToTest].Display;
            }
            //using (FileStream stm = new FileStream(fileName, FileMode.Open))
            //{
            //    //一樣用GZipStream把FileStream包起來
            //    using (GZipStream zip = new GZipStream(stm, CompressionMode.Decompress))
            //    {
            //        //還原的二進位資料來源改為GZipStream
            //        afterDeser = (dcs.ReadObject(zip) as List<User>)[indexToTest].Display;
            //    }
            //}
            sw.Stop();
            Console.WriteLine("Deserialization: {0:N0}ms", sw.ElapsedMilliseconds);

            //比對還原後的資料是否相同
            Console.WriteLine("Before: {0}", beforeSer);
            Console.WriteLine("After: {0}", afterDeser);
            Console.WriteLine("Pass Test: {0}", beforeSer.Equals(afterDeser));
            Console.Read();
        }

        private static List<User> GenSimData()
        {
            List<User> lst = new List<User>();
            Random rnd = new Random();
            for (int i = 0; i < 200000; i++)
            {
                lst.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    RegDate = DateTime.Today.AddDays(-rnd.Next(5000)),
                    Name = "User" + i,
                    Score = rnd.Next(65535)
                });
            }
            return lst;
        }

        [Serializable]
        private class User
        {
            public Guid Id { get; set; }
            public DateTime RegDate { get; set; }
            public string Name { get; set; }
            public decimal Score { get; set; }
            public string Display
            {
                get
                {
                    return string.Format(
                        "{0} / {1:yyyy-MM-dd} / {2:N0}",
                        Name, RegDate.ToLocalTime() /*修正時區問題*/, Score);
                }
            }
        }
    }
}
