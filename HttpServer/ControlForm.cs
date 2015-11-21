using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace HttpServer
{
    public partial class ControlForm : Form
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private delegate void MyDelegate(string message);

        HttpServer server;

        private Colors LogColor;

        public ControlForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ColorSet(0);
            textBox3.Text = Environment.CurrentDirectory + @"\Html\index.html";
            Init();
        }

        private void Init()
        {
            richTextBox1.Clear();
            
            WriteLineLog("//     ：コメント", LogColor.TipsColor);
            WriteLineLog("#      ：コマンドラインパイプ", LogColor.TipsColor);
            WriteLineLog("color  ：Logの色変更", LogColor.TipsColor);
            WriteLineLog("cmd    ：コマンドライン起動", LogColor.TipsColor);
            WriteLineLog("exit   ：終了", LogColor.TipsColor);

            button1.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = false;
        }

        private void ColorSet(int colorNum)
        { 
            switch (colorNum)
            { 
                case 0:
                    LogColor.BackColor = Color.White;
                    LogColor.ForeColor = Color.Black;
                    LogColor.CommentColor = Color.Green;
                    LogColor.CommandColor = Color.Blue;
                    LogColor.SystemColor = Color.Orange;
                    LogColor.TipsColor = Color.Gray;
                    break;

                case 1:
                    LogColor.BackColor = Color.Black;
                    LogColor.ForeColor = Color.White;
                    LogColor.CommentColor = Color.Green;
                    LogColor.CommandColor = Color.Blue;
                    LogColor.SystemColor = Color.Red;
                    LogColor.TipsColor = Color.Gray;
                    break;
            }

            richTextBox1.BackColor = LogColor.BackColor;
            richTextBox1.ForeColor = LogColor.ForeColor;
        }

        // コマンドライン
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Command(textBox2.Text);
                textBox2.Clear();
            }
        }

        // スタート
        private void button1_Click(object sender, EventArgs e)
        {
            // Port設定
            int port;
            if (!int.TryParse(textBox1.Text, out port))
            {
                MessageBox.Show("Port番号が正しくありません\n" + 
                                "(一般の使用では8080を使用してください)", 
                                "エラー", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                textBox1.Clear();
                return;
            }

            // Host名取得
            string hostname = Dns.GetHostName();

            // IPアドレス取得
            IPAddress[] adrList = Dns.GetHostAddresses(hostname);
            IPAddress ip = adrList[0].MapToIPv4();
            foreach (IPAddress address in adrList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ip = address;
                }
            }

            WriteLineLog(string.Format("IPv4    : {0},     Port : {1}", ip.ToString(), port.ToString()), LogColor.SystemColor);
            WriteLineLog(string.Format("Address : http://{0}:{1}/\n", ip.ToString(), port.ToString()), LogColor.SystemColor);

            if (textBox3.Text != "")
            {
                server = new HttpServer(ip, port, textBox3.Text + @"\index.txt", this);
            }
            else
            {
                server = new HttpServer(ip, port, this);
            }

            // textbox無効化
            textBox1.Enabled = false;

            // startButtonの無効化
            button1.Enabled = false;

            // conteStstartButtonの有効化
            button2.Enabled = true;

            // Htmlフォルダの変更不可
            textBox3.Enabled = false;
            button5.Enabled = false;
        }

        // コンテストスタート
        private void button2_Click(object sender, EventArgs e)
        {
            // 問題の公開
            Properties.Settings.Default.ServerStatus = true;

            // contestStartButtonの無効化
            button2.Enabled = false;

            // contestStopButtonの有効化
            button4.Enabled = true;
        }

        // コンテストストップ
        private void button4_Click(object sender, EventArgs e)
        {
            // textbox有効化
            textBox1.Enabled = true;

            // contestStartButtonの有効化
            button2.Enabled = true;

            // contestStopButtonの無効化
            button4.Enabled = false;

            Properties.Settings.Default.ServerStatus = false;

            //server.Stop();
        }

        // コマンド実行 
        private void Command(string command)
        {
            // コメント
            if (command.StartsWith("//"))
            { 
                WriteLineLog(command.Substring(2), LogColor.CommentColor);
                return;
            }

            // コマンドラインパイプ
            if (command.StartsWith("#"))
            { 
                WriteLineLog(command.ToLower().Substring(1), LogColor.CommandColor);

                //Processオブジェクトを作成
                Process p = new Process();
                p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");

                //出力を読み取れるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = false;

                //ウィンドウを表示しないようにする
                p.StartInfo.CreateNoWindow = true;
                //コマンドラインを指定（"/c"は実行後閉じるために必要）
                p.StartInfo.Arguments = @"/c" + command.ToLower().Substring(1);

                //起動
                p.Start();

                //出力を読み取る
                string results = p.StandardOutput.ReadToEnd();

                //プロセス終了まで待機する
                p.WaitForExit();
                p.Close();

                //出力された結果を表示
                WriteLineLog(results);
                
                return;
            }

            // logの色
            if (command.ToLower().StartsWith("color"))
            {
                ColorSet(int.Parse(command.Substring(5)));
                Init();
                return;
            }

            switch (command.ToLower())
            {
                case "cmd":  // コマンドライン起動
                    Process p = Process.Start("cmd.exe");
                    p.Dispose();
                    break;

                case "exit": // 終了
                    Environment.Exit(0);
                    break;

                default:
                    WriteLineLog(command);
                    break;
            }
        }

        // テキストボックス書き込み
#region
        private void WriteLog(string message, Color color)
        {
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;

            Font baseFont = richTextBox1.SelectionFont;
            Font fnt = new Font(baseFont.FontFamily, baseFont.Size, baseFont.Style);

            // Fontを変更する
            richTextBox1.SelectionFont = fnt;

            // 文字列を挿入する
            richTextBox1.SelectedText = message;

            // 使ったものは片付ける
            baseFont.Dispose();
            fnt.Dispose();
        }

        private void WriteLog(string message) { WriteLog(message, LogColor.ForeColor); }
        private void WriteLineLog(string message) { WriteLog(message + "\n"); }
        private void WriteLineLog(string message, Color color) { WriteLog(message + "\n", color); }

        public void WriteLileLog_Thread(string message)
        {
            Invoke(new MyDelegate(WriteLineLog), message);
        }
#endregion

        // HideCaret
#region
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(richTextBox1.Handle);
        }

        private void richTextBox1_GotFocus(object sender, EventArgs e)
        { 
             HideCaret(richTextBox1.Handle);
        }
#endregion

        // Logをクリアする
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        // コピー
        private void copyCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // クリップボードに選択範囲のテキストをコピー
            Clipboard.Clear();
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        // 削除
        private void deleatDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        // すべて選択
        private void allselectAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        // 文字色変更
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // カラーダイアログを表示
            colorDialog1.ShowDialog();
            
            // 選択箇所の文字色を変更
            richTextBox1.SelectionColor = colorDialog1.Color;
        }

        private struct Colors
        {
            public Color BackColor;
            public Color ForeColor;
            public Color CommentColor;
            public Color CommandColor;
            public Color SystemColor;
            public Color TipsColor;
        }

        private void ControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //server.Stop();
            Environment.Exit(0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();
                textBox3.Text = fbd.SelectedPath;
            }
        }
    }
}
