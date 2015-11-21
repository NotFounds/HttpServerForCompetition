using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    class HttpServer
    {
        private Socket server;
        private ControlForm _ctrlForm;
        private bool state;
        Thread thread;
        private string DefaultHtml = Environment.CurrentDirectory + @"\Html\index.html";
        public Submissions submissionsManager = new Submissions();

        public HttpServer(IPAddress address, int port, ControlForm ctrlForm)
        {
            try
            {
                // サーバーソケット初期化
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
                IPEndPoint ipEndPoint = new IPEndPoint(address, port);
                if (ipEndPoint == null)
                { 
                    return;
                }
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                server.Bind(ipEndPoint);
                server.Listen(5);

                _ctrlForm = ctrlForm;

                state = true;

                thread = new Thread(Start);
                thread.Start();
                submissionsManager.JudgeStart();
            }
            catch (Exception Ex)
            {
                ExceptionLogger.ExceptionLogger.errorLog(Ex);
                return;
            }
        }

        public HttpServer(IPAddress address, int port, string HtmlPath, ControlForm ctrlForm)
        {
            try
            {
                DefaultHtml = HtmlPath;

                // サーバーソケット初期化
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint ipEndPoint = new IPEndPoint(address, port);
                if (ipEndPoint == null)
                {
                    return;
                }
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                server.Bind(ipEndPoint);
                server.Listen(5);

                _ctrlForm = ctrlForm;

                state = true;

                thread = new Thread(Start);
                thread.Start();
                submissionsManager.JudgeStart();
            }
            catch (Exception Ex)
            {
                ExceptionLogger.ExceptionLogger.errorLog(Ex);
                return;
            }
        }

        public void  Start()
        {
            // 要求待ち
                while(state)
                {
                    Console.WriteLine("HTTPSERVER");
                    Socket client = server.Accept();

                    Response response = new Response(client, DefaultHtml, true, _ctrlForm, submissionsManager);
                    response.Start();
                }
        }

        public void Stop()
        {
            state = false;
            thread.Abort();
        }
    }
}
