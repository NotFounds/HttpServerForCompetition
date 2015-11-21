using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace HttpServer
{
    class Submissions
    {
        private string STARTUP_PATH = "";
        private string sourceFilePath = @"\Sources\";
        private string fileName = "Submissions.txt";
        private string[] strLines;
        private string[] testCaseNum;
        private Thread JudgeThread;

        private string HTMLString = "";

        private Queue<string> JudgeQueue;

        public Submissions()
        {
            // 初期化
            Assembly asm = Assembly.GetEntryAssembly();
            JudgeQueue = new Queue<string>();
            STARTUP_PATH = Path.GetDirectoryName(asm.Location);
            sourceFilePath = STARTUP_PATH + @"\Sources\";

            // テストケースの数を読み取る
            Encoding enc = Encoding.GetEncoding("UTF-8");
            testCaseNum = File.ReadAllLines(STARTUP_PATH + @"\TestCases\index.txt", enc);
            
            Update();
            //using (StreamWriter sw = new StreamWriter(fileName)) { }
        }

        public void Submit(string judgeData)
        {
            try
            {
                JudgeQueue.Enqueue(judgeData);
            }
            catch
            {
            }
        }

        // ジャッジ部分
        private bool JudgeState = false;
        public void JudgeStart()
        {
            JudgeThread = new Thread(new ThreadStart(judge));
            JudgeState = true;
            JudgeThread.Start();
        }

        public void JudgeStop()
        {
            JudgeState = false;
            JudgeThread.Abort();
        }

        private void judge()
        {
            while (true)
            {
                // ジャッジを行うフラグがfalseなら終了
                if (!JudgeState) break;

                if (JudgeQueue.Count > 0)
                {
                    string j = JudgeQueue.Dequeue();
                    string[] strs = j.Split(',');
                    if (strs.Length < 6) continue;

                    // ジャッジ待機中出ないならスキップ
                    if (strs[3] != "Waiting") continue;

                    pair p = ConvertStringToPair(j);
                    //Console.WriteLine(p.index + " : " + p.path);

                    string status = compile(p);

                    if (status == "Compile Error")
                    {
                        // コンパイルエラー
                    }
                    else
                    {
                        for (int i = 1; i <= int.Parse(testCaseNum[int.Parse(p.problem) - 1]); i++)
                        {
                            status = run(p, i.ToString());

                            if (status == "Runtime Error")
                            {
                                // ランタイムエラー
                                break;
                            }
                            else if (status == "Time Limit Exceeded")
                            {
                                // タイムリミット
                                break;
                            }
                            else
                            {
                                // 出力されたファイルの正誤チェック
                                status = check(p, p.problem, i.ToString());
                            }
                        }
                    }

                    // 出力
                    using (StreamWriter sw = new StreamWriter("Submissions.txt", true))
                    {
                        sw.WriteLine(j.Replace("Waiting", status));
                    }

                    // 画面出力
                    switch (status)
                    {
                        case "Accepted":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case "Wrong Answer":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                    }
                    Console.WriteLine(j.Replace("Waiting", status));
                    Console.ResetColor();
                }

                Update();
                Thread.Sleep(50);
            }
        }

        // 更新
        public void Update()
        {
            readAll();
            writeFile(readFile());
        }

        private void readAll()
        {
            const int RetryCountMax = 15;
            const int RetryInterval = 30;

            for (int i = 0; i < RetryCountMax; i++)
            {
                try
                {
                    Encoding enc = Encoding.GetEncoding("UTF-8");
                    strLines = File.ReadAllLines(fileName, enc);
                    strLines.Where<string>(x => x.Contains("Waiting")).ToList<string>().ForEach(x => Submit(x));
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {

                        strLines.Where<string>(x => !x.Contains("Waiting")).ToList<string>().ForEach(x => sw.WriteLine(x));
                    }
                    break;
                }
                catch (IOException)
                {
                    // エラーが起きてもなにもしない
                }

                Thread.Sleep(RetryInterval);
            }
        }

        // 実行結果の答え合わせ
        private string check(pair file, string problemNum, string caseNum)
        {
            string ret = "Accepted";
            Encoding enc = Encoding.GetEncoding("UTF-8");

            // 答え
            string[] ansLines;
            ansLines = File.ReadAllLines(getAnswerFileName(problemNum, caseNum), enc);

            // 実行結果
            string[] retLines;
            string retPath = getOutFileName(file.index, problemNum, caseNum);
            if (!File.Exists(retPath))
            {
                return "Waiting";
            }

            retLines = File.ReadAllLines(retPath, enc);

            if (ansLines.Length != retLines.Length)
            {
                // 行数が違うから間違い
                ret = "Wrong Answer";
            }
            else
            {
                for (int i = 0; i < ansLines.Length; i++)
                {
                    if (ansLines[i] != retLines[i])
                    {
                        ret = "Wrong Answer";
                        break;
                    }
                }
            }

            return ret;
        }

        // 実行
        private string run(pair file, string caseNum)
        {
            string ret = "";

            // Processオブジェクトを作成
            Process p = new Process();
            p.StartInfo.FileName = sourceFilePath + Path.GetFileNameWithoutExtension(file.path) + ".exe";

            // 出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;

            // エラーダイアログを表示しない
            p.StartInfo.ErrorDialog = false;

            // ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;

            // 起動
            p.Start();
            string input;
            using (StreamReader sr = new StreamReader(getTestFileName(file.problem, caseNum)))
            {
                input = sr.ReadToEnd();
            }

            p.StandardInput.Write(input);

            // プロセス終了まで5s待機する
            bool status = p.WaitForExit(5000);
            if (!p.HasExited)
            {
                p.Kill();
            }

            // User時間を測定
            TimeSpan ts = p.UserProcessorTime;

            // 出力を読み取る
            string output = p.StandardOutput.ReadToEnd();
            using (StreamWriter sw = new StreamWriter(getOutFileName(file.index, file.problem, caseNum)))
            {
                sw.Write(output);
            }

            // エラーの読み取り
            int exit = p.ExitCode;
            string error = p.StandardError.ReadToEnd();

            p.Close();

            if (error != "" || exit != 0)
            {
                // Runtime Error
                ret = "Runtime Error";
            }
            else if (!status)
            {
                // Time Limit Exceeded
                ret = "Time Limit Exceeded";
            }
            if (ts.TotalSeconds >= 2.0)
            {
                // User実行時間が2秒以上ならTLE
                ret = "Time Limit Exceeded";
            }

            return ret;
        }

        // AnswerFile名を取得
        private string getAnswerFileName(string problemNum, string caseNum)
        {
            string ret = "";
            ret += STARTUP_PATH + @"\TestCases\" + "Answer_" + problemNum + "_" + caseNum + ".txt";
            return ret;
        }

        // TestCaseFile名を取得
        private string getTestFileName(string problemNum, string caseNum)
        {
            string ret = "";
            ret += STARTUP_PATH + @"\TestCases\" + "TestCase_" + problemNum + "_" + caseNum + ".txt";
            return ret;
        }

        // OutPutFile名を取得
        private string getOutFileName(string runNum, string problemNum, string caseNum)
        {
            string ret = "";
            ret += sourceFilePath + runNum + "_" + problemNum + "_" + caseNum + ".txt";
            return ret;
        }

        // コンパイル
        private string compile(pair file)
        {
            string ret = "";

            // Processオブジェクトを作成
            Process p = new Process();
            p.StartInfo.FileName = STARTUP_PATH + @"\compile.bat";

            // 出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;

            // ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;
            // コマンドラインを指定
            p.StartInfo.Arguments = file.path;

            // 起動
            p.Start();

            // プロセス終了まで3s待機する
            bool status = p.WaitForExit(3000);
            if (!p.HasExited)
            {
                p.Kill();
            }

            // 出力を読み取る
            string output = p.StandardOutput.ReadToEnd();

            p.Close();

            if (output.Contains("エラー") || output.ToLower().Contains("error"))
            {
                ret = "Compile Error";
            }

            return ret;
        }

        private pair ConvertStringToPair(string state)
        {
            pair p;

            string[] strs = state.Split(',');

            p.index = strs[0];
            p.problem = strs[2][strs[2].Length - 1].ToString();
            p.path = sourceFilePath + p.index;

            switch (strs[4])
            {
                case "C":
                    p.path += ".c";
                    break;

                case "C++":
                    p.path += ".cpp";
                    break;

                case "C#":
                    p.path += ".cs";
                    break;
            }

            return p;
        } 

        public string getHTML()
        {
            return HTMLString;
        }

        // Html書き換え部分
        private List<string> readFile()
        {
            List<string> result = new List<string>();

            int a = strLines.Length - 50;
            if (strLines.Length < 50)
            {
                a = 0;
            }

            for (int i = a; i < strLines.Length; i++)
            {
                if (strLines[i] == "") break;

                string str = @"                <tr>" + Environment.NewLine;
                string[] strs = strLines[i].Split(',');

                string statusColor = "";

                switch (strs[3])
                {
                    case "Wrong Answer":
                        statusColor = @"<td style=""color:#FF0000"">";
                        break;

                    case "Accepted":
                        statusColor = @"<td style=""color:#008000"">";
                        break;

                    case "Waiting":
                        statusColor = @"<td style=""color:#404060"">";
                        break;

                    default:
                        statusColor = @"<td style=""color:#FF9900"">";
                        break;
                }

                str += string.Format(@"                        <td>{0}</td>", strs[0]) + Environment.NewLine;
                str += string.Format(@"                        <td>{0}</td>", strs[1]) + Environment.NewLine;
                str += string.Format(@"                        <td><a href=""{0}.html"">{1}</a></td>", strs[2], strs[2]) + Environment.NewLine;
                str += string.Format(@"                        {0}{1}</td>", statusColor, strs[3]) + Environment.NewLine;
                str += string.Format(@"                        <td>{0}</td>", strs[4]) + Environment.NewLine;
                str += string.Format(@"                        <td>{0}</td>", strs[5]) + Environment.NewLine;
                str += @"                </tr>" + Environment.NewLine;
                result.Add(str);
            }

            return result;

        }

        private void writeFile(List<string> results)
        {
            StringBuilder sb = new StringBuilder();

            string header =
    @"<!DOCTYPE HTML>
<html>
	<head>
		<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
		<meta http-equiv=""refresh"" content=""60;URL=Submissions.html"">
		<title>Submissions</title>		 
        <link href=""css/body.css"" type=""text/css"" rel=""stylesheet"" />
		<link href=""css/buttons.css"" type=""text/css"" rel=""stylesheet"" />
		<link href=""css/table.css"" type=""text/css"" rel=""stylesheet"" />
	</head>		 
	<body>
		<div style=""text-align: center;"">
			<font face=""Segoe UI,メイリオ,Meiryo,Verdana,Helvetica,ヒラギノ角ゴ Pro W,Hiragino Kaku Gothic Pro,MS Pゴシック"" size=""5"" style=""color: rgb(64, 64, 96);"">Submissions<br>
			</font>
		</div>
        <div id=""main"">
		<font face=""Segoe UI,メイリオ,Meiryo,Verdana,Helvetica,ヒラギノ角ゴ Pro W,Hiragino Kaku Gothic Pro,MS Pゴシック"" size=""2"" style=""color: rgb(33, 33, 77);"">
				<h3>
			<a name=""TOC--3""></a>
			<font face=""Segoe UI,メイリオ,Meiryo,Verdana,Helvetica,ヒラギノ角ゴ Pro W,Hiragino Kaku Gothic Pro,MS Pゴシック"" size=""4"" style=""color: rgb(64, 64, 96);"">ジャッジのステータスについて
			</font>		 
			<hr>
		</h3>
		<table class=""zebra"">
			<thead>
				<tr>
					<th>ステータス</th>        
					<th>説明</th>
				</tr>
			</thead>  
			<tr>
				<td>Waiting</td>        
				<td>ジャッジを行っています。</td>
			</tr>
			<tr>
				<td>Compile Error</td>
				<td>提出されたプログラムのコンパイルに失敗しました。</td>
			</tr>
			<tr>
				<td>Runtime Error</td>        
				<td>提出されたプログラムの実行中にエラーが発生しました。</td>
			</tr>   
			<tr>
				<td>Time Limit Exceeded</td>        
				<td>問題に指定された実行時間内にプログラムが終了しませんでした。</td>
			</tr>
			<tr>
				<td>Wrong Answer</td>        
				<td>誤答です。提出したプログラムの出力は正しくありません。</td>
			</tr>   
			<tr>
				<td>Accepted</td>        
				<td>正答です。提出したプログラムは正しいことが保障されました。</td>
			</tr>   
		</table>
		<br>
		<br>
		<h3>
			<a name=""TOC--""></a>
			<font face=""Segoe UI,メイリオ,Meiryo,Verdana,Helvetica,ヒラギノ角ゴ Pro W,Hiragino Kaku Gothic Pro,MS Pゴシック"" size=""4"" style=""color: rgb(64, 64, 96);"">すべての結果
			</font>		 
			<hr>
		</h3>
		<br>
		<table class=""zebra"">
			<thead>
				<tr>
					<th>Run#</th>        
					<th>Author</th>
					<th>Problem</th>
					<th>Status</th>
					<th>Lang</th>
					<th>Submission Date</th>
				</tr>
			</thead>  ";

            sb.AppendLine(header);

            // データの追加
            foreach (string s in results)
            {
                sb.AppendLine(s);
            }

            // footerの書き込み
            string footer = @"      </table>
        <br><br>
		<button class=""btn"" onclick=""location.href='Submit.html'"" type=""button"">Submit Page</button>
		<button class=""btn"" onclick=""location.href='index.html'"" type=""button"">Top Page</button>	
	</font>
    </div>
	</body>
</html>";
            sb.AppendLine(footer);

            using (StreamWriter sw = new StreamWriter(@"Html\Submissions.html", false))
            {
                sw.Write(sb);                
            }
            
            HTMLString = sb.ToString();
        }
    }

    struct pair
    {
        public string index;
        public string path;
        public string problem;
    }
}
