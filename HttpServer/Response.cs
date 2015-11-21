using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpServer
{
    class Response
    {
        private Socket mClient;

        private bool isLog = false;
        private string logfile = "access.log";
        private string indexfile = @"\Html\index.html";
        private ControlForm _ctrlForm;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="client">サーバーソケット</param>
        /// <param name="index">indexファイルパス</param>
        public Response(Socket client, string index, ControlForm ctrlForm)
        {
            mClient = client;
            indexfile = index;
            isLog = false;
            _ctrlForm = ctrlForm;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="client">サーバーソケット</param>
        /// <param name="index">indexファイルパス</param>
        /// <param name="log">logファイルを出力する場合はtrue, 出力しない場合はfalse</param>
        public Response(Socket client, string index, bool log, ControlForm ctrlForm)
        {
            mClient = client;
            indexfile = index;
            isLog = log;
            _ctrlForm = ctrlForm;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="client">サーバーソケット</param>
        /// <param name="index">indexファイルパス</param>
        /// <param name="log">logファイルのファイル名</param>
        public Response(Socket client, string index, string log, ControlForm ctrlForm)
        {
            mClient = client;
            indexfile = index;
            logfile = log;
            isLog = true;
            _ctrlForm = ctrlForm;
        }

        /// <summary>
        /// 応答開始
        /// </summary>
        public void Start()
        {
            Thread thread1 = new Thread(RunReceive);
            thread1.Start();
        }

        /// <summary>
        /// 応答実行
        /// </summary>
        public void RunReceive()
        {
            try
            {
                // 要求受信
                byte[] buffer = new byte[16384];
                int recvLen = mClient.Receive(buffer);

                if (recvLen <= 0)
                    return;

                String message = Encoding.ASCII.GetString(buffer, 0, recvLen);

                if (isLog)
                {
                    using (StreamWriter sw = new StreamWriter(OpenWorkfileWithRetry(logfile)))
                    {
                        sw.WriteLine(message);
                    }
                }

                _ctrlForm.WriteLileLog_Thread(message);

                Request req = new Request(message);
                Dictionary<string, string> bodys = req.body;

                // http ヘッダー情報
                string getFileType = "text/html";
                string statusCode = "200 OK";

                if (req.type == ReqType.GET)
                {
                    // TODO:
                    // アクセスできるファイルの一覧は Directory<string, bool> に ファイル名, アクセス許可(true)
                    // 適切なファイルへのアクセスならファイルを返す
                    // ファイルリストにない場合は、404を返す
                    // アクセス権限がない場合は、403を返す

                    string path;
                    if (bodys != null) path = bodys["path"];
                    else path = indexfile;

                    if (File.Exists(path))
                    {
                        if (!Properties.Settings.Default.ServerStatus && Path.GetFileName(path).ToLower().StartsWith("problem"))
                        {
                            // 時間外に問題ファイルにアクセスしようとするとAccsess.htmlを表示
                            message = @"<meta http-equiv=""refresh"" content=""0;URL=Access.html"">";
                            statusCode = "403 Forbidden";

                            // バイナリエンコーディング
                            buffer = Encoding.UTF8.GetBytes(message);
                        }
                        else
                        {
                            Action readText = () =>
                            {
                                using (StreamReader sr = new StreamReader(path))
                                {
                                    message = sr.ReadToEnd();
                                }

                                // バイナリエンコーディング
                                buffer = Encoding.UTF8.GetBytes(message);
                            };

                            Action readBinary = () =>
                            {
                                buffer = File.ReadAllBytes(path);
                            };

                            // ファイルの種類に応じて読み込み方を変更 & MIMEタイプの設定 
                            switch (Path.GetExtension(path))
                            {
                                // TextData
                                case ".HTML":
                                case ".html":
                                    readText();
                                    getFileType = "text/html";
                                    break;

                                case ".CSS":
                                case ".css":
                                    readText();
                                    getFileType = "text/css";
                                    break;

                                case ".JS":
                                case ".js":
                                    readText();
                                    getFileType = "text/javascript";
                                    break;

                                case ".txt":
                                case ".TXT":
                                    readText();
                                    getFileType = "text/plain";
                                    break;

                                case ".XML":
                                case ".xml":
                                    readText();
                                    getFileType = "text/xml";
                                    break;

                                // BinaryData
                                case ".GIF":
                                case ".gif":
                                    readBinary();
                                    getFileType = "image/gif";
                                    break;

                                case ".PNG":
                                case ".png":
                                    readBinary();
                                    getFileType = "image/png";
                                    break;

                                case ".JPEG":
                                case ".jpeg":
                                case ".JPG":
                                case ".jpg":
                                    readBinary();
                                    getFileType = "image/jpeg";
                                    break;

                                case ".DOC":
                                case ".doc":
                                    readBinary();
                                    getFileType = "application/msword";
                                    break;

                                case ".PDF":
                                case ".pdf":
                                    readBinary();
                                    getFileType = "application/pdf";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        message = "<title>404 Not Found</title>" + "<h1>404 Not Found</h1>";
                        statusCode = "404 Not Found";

                        // バイナリエンコーディング
                        buffer = Encoding.UTF8.GetBytes(message);
                    }
                }
                else if (req.type == ReqType.POST)
                {
                    // 問題提出ならファイルを保存する
                    // クエリは http ://サーバー:8080/SubmitAnswer?playerid= &problemid= &language= &answer=
                    // 

                    if (!Properties.Settings.Default.ServerStatus)
                    {
                        // contest終了後/contest開始前に送信されたファイル
                    }

                    int lang = int.Parse(bodys["lang"]);
                    string playerid = bodys["playerid"];
                    string probid = bodys["probid"];
                    string answer = bodys["answer"];

                    Encoding enc = Encoding.GetEncoding("UTF-8");
                    string[] strLines = File.ReadAllLines("Submissions.txt", enc);
                    int index = strLines.Length + 1;

                    string filename = string.Format(@"Sources\{0:D4}", index);
                    DateTime t = DateTime.Now;

                    Stream fs = OpenWorkfileWithRetry("Submissions.txt");

                    string writeStr = "";

                    switch (lang)
                    {
                        case 1:
                            filename += ".c";

                            // ログに書き込み
                            _ctrlForm.WriteLileLog_Thread(string.Format("{0:D4},{1},Problem{2},C,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute));

                            writeStr = string.Format("{0:D4},{1},Problem{2},Waiting,C,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute);

                            break;

                        case 2:
                            filename += ".cpp";

                            // ログに書き込み
                            _ctrlForm.WriteLileLog_Thread(string.Format("{0:D4},{1},Problem{2},C++,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute));

                            writeStr = string.Format("{0:D4},{1},Problem{2},Waiting,C++,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute);

                            break;

                        case 3:
                            filename += ".cs";

                            // ログ書き込み
                            _ctrlForm.WriteLileLog_Thread(string.Format("{0:D4},{1},Problem{2},C#,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute));

                            writeStr = string.Format("{0:D4},{1},Problem{2},Waiting,C#,{3}-{4:D2}-{5:D2} {6:D2}:{7:D2}", index, playerid, probid, t.Year, t.Month, t.Day, t.Hour, t.Minute);

                            break;

                        default:
                            break;
                    }

                    // submissions.txtに書き込み
                    using (fs)
                    {
                        fs.Seek(0, SeekOrigin.End);
                        using (TextWriter tw = new StreamWriter(fs))
                        {
                            tw.WriteLine(writeStr);
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(filename, false))
                    {
                        sw.Write(System.Web.HttpUtility.UrlDecode(answer));
                    }

                    message = @"<meta http-equiv=""refresh"" content=""0;URL=Submissions.html"">";


                    // バイナリエンコーディング
                    buffer = Encoding.UTF8.GetBytes(message);
                }
                else
                {
                    if (message.IndexOf("GET /") >= 0)
                    {
                        using (StreamReader sr = new StreamReader(indexfile))
                        {
                            message = sr.ReadToEnd();
                        }
                    }
                    else
                    {
                        message = "<title>404 Not Found</title>" + "<h1>Not Found</h1>";
                        statusCode = "404 Not Found";
                    }

                    // バイナリエンコーディング
                    buffer = Encoding.UTF8.GetBytes(message);
                }


                int contentLen = buffer.GetLength(0);

                // HTTPヘッダー生成
                String httpHeader = String.Format("HTTP/1.1 {0}\n" + "Content-type: {1}; charset=UTF-8\n" + "Content-length: {2}\n" + "\n", statusCode, getFileType, contentLen);

                byte[] httpHeaderBuffer = new byte[4096];
                httpHeaderBuffer = Encoding.UTF8.GetBytes(httpHeader);

                // 応答内容送信
                mClient.Send(httpHeaderBuffer);
                mClient.Send(buffer);

            }
            catch (System.Net.Sockets.SocketException e)
            {
                if (isLog)
                {
                    ExceptionLogger.ExceptionLogger.errorLog(e);
                }
            }
            catch (Exception Ex)
            {
                ExceptionLogger.ExceptionLogger.errorLog(Ex);
            }
            finally
            {
                mClient.Close();
            }
        }

        // ファイルが使えるまで待ち、書き込む
        private Stream OpenWorkfileWithRetry(string fileName)
        {
            const int RetryCountMax = 20;
            const int RetryInterval = 30;

            FileStream fs = null;
            for (int i = 0; i < RetryCountMax; i++)
            {
                try
                {
                    fs = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                        fs = null;
                    }
                }
                Thread.Sleep(RetryInterval);
            }
            return null;
        }

    }
}
