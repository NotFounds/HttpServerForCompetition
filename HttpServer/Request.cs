using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HttpServer
{
    class Request
    {
        private ReqType _type;
        private string _req;

        private Dictionary<string, string> bodys;

        public Request(string req)
        {
            _req = req;

            // 要求URL確認 ＆ 応答内容生成
            switch (req.Split(' ')[0])
            {
                case "GET":
                    _type = ReqType.GET;

                    MatchCollection get_reg = Regex.Matches(req, @"GET (?<path>/[a-zA-Z0-9/]*.(html|txt|css|js|jpeg|jpg|png|pdf|xml|gif))", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match m in get_reg)
                    {
                        string path = Environment.CurrentDirectory + @"\Html\" + m.Groups["path"].ToString().Replace('/', '\\');
                        bodys = new Dictionary<string, string>();
                        bodys.Add("path", path);
                    }

                    break;

                case "POST":
                    bodys = new Dictionary<string, string>();
                    _type = ReqType.POST;

                    //MatchCollection post_reg = Regex.Matches(req, @"POST /(?<cmd>[a-zA-Z0-9]*) ", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    // bodysにidやvalueを格納する
                    bodys = new Dictionary<string, string>();

                    // playerid
                    var reg_player = new Regex(@"playerid=(?<id>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    var player = reg_player.Match(req);
                    bodys.Add("playerid", player.Groups["id"].ToString());

                    // probid
                    var reg_prob = new Regex(@"problemid=(?<id>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    var prob = reg_prob.Match(req);
                    bodys.Add("probid", prob.Groups["id"].ToString());
         
                    // language : c, c++, java, c#, vb (1, 2, 3, 4, 5)
                    var reg_lang = new Regex(@"language=(?<id>[0-9]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    var lang = reg_lang.Match(req);
                    bodys.Add("lang", lang.Groups["id"].ToString());

                    // answer
                    bodys.Add("answer", req.Substring(req.IndexOf("answer=") + "answer=".Length));

                    break;
            }
        }

        public ReqType type
        {
            get
            {
                return _type;
            }
        }

        public Dictionary<string, string> body
        {
            get 
            {
                return bodys;
            }
        }
    }

    enum ReqType
    { 
        GET,
        POST
    }
}
