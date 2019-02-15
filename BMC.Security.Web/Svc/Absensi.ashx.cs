using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using BMC.Security.Web.Helpers;
using Newtonsoft.Json;
namespace BMC.Security.Web.Svc
{
    /// <summary>
    /// Summary description for Absensi
    /// </summary>
    public class Absensi : IHttpAsyncHandler
    {
        public bool IsReusable => true;

        public class Absen{
            public string IDS { get; set; }
        }
        public Absensi()
        {
          
        }
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            //context.Response.Write("<p>Begin IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
            AsynchOperation asynch = new AsynchOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }
    }

    class AsynchOperation : IAsyncResult
    {
        static AzureTableHelper DB;
        private bool _completed;
        private Object _state;
        private AsyncCallback _callback;
        private HttpContext _context;

        bool IAsyncResult.IsCompleted { get { return _completed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        Object IAsyncResult.AsyncState { get { return _state; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsynchOperation(AsyncCallback callback, HttpContext context, Object state)
        {
            if (DB == null)
            {
                DB = new AzureTableHelper("Absen");
            }
            _callback = callback;
            _context = context;
            _state = state;
            _completed = false;
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
        }

        private async void StartAsyncTask(Object workItemState)
        {


            _context.Response.ContentType = "text/plain";
            try
            {
                string strJson = new StreamReader(_context.Request.InputStream).ReadToEnd();

                //deserialize the object
                Absen objAbsen = Newtonsoft.Json.JsonConvert.DeserializeObject<Absen>(strJson);
                if (objAbsen != null)
                {
                    var item = new Models.AbsenData() { IDS = objAbsen.IDS };
                    item.AssignKey();
                    await DB.InsertDataAbsen(item);
                    _context.Response.Write(string.Format("Data={0}", objAbsen.IDS));
                }
                else
                {
                    _context.Response.Write("No Data");
                }
            }
            catch (Exception ex)
            {
                _context.Response.Write("Error :" + ex.Message);
            }


            //_context.Response.Write("<p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");

           // _context.Response.Write("Hello World from Async Handler!");
            _completed = true;
            _callback(this);
        }
    }

    public class Absen
    {
        public string IDS { get; set; }
    }

}