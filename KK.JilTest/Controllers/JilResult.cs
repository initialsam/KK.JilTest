using System;
using System.Web;
using System.Web.Mvc;
using Jil;
using System.IO;

namespace KK.JilTest.Controllers
{
    public class JilResult : JsonResult
    {
        public Options JilOptions { get; set; }
        public JilResult()
        {
            // 指定日期格式
            JilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) 
                                        ? ContentType 
                                        : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                using (var writer = new StreamWriter(response.OutputStream))
                {
                    // 使用Jil進行序列化
                    JSON.Serialize(Data, writer, JilOptions);
                    writer.Flush();
                }
                
                // 也可這樣寫
                // response.Write(JSON.Serialize(Data, jilOptions));
            }
        }
    }
}