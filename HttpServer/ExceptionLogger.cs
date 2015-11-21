using System;
using System.Windows.Forms;

namespace ExceptionLogger
{
    public class ExceptionLogger
    {
        //エラー出力
        /// <summary>エラー出力</summary>
        /// <param name="Ex">エラー</param>
        public static void errorLog(Exception Ex)
        {
            MessageBox.Show("エラーが発生しました\n" + 
                            "直ちにプログラムを終了し「Error.log」を報告してください。", 
                            "エラー", MessageBoxButtons.OK, 
                            MessageBoxIcon.Exclamation);   
         
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Application.StartupPath + @"\Error.log", true))
            {
                sw.WriteLine("Error : " + DateTime.Now);
                sw.WriteLine("----- Exceptions -----");
                sw.WriteLine(Ex.ToString());
                sw.WriteLine("----- StackTrace -----");
                sw.WriteLine(Ex.StackTrace);
                sw.WriteLine("----- Message    -----");
                sw.WriteLine(Ex.Message);
                sw.WriteLine("----- TargetSite -----");
                sw.WriteLine(Ex.TargetSite);
                sw.WriteLine("----- Inner      -----");
                sw.WriteLine(Ex.InnerException);
                sw.WriteLine("----- Source     -----");
                sw.WriteLine(Ex.Source);
                sw.WriteLine("----- Data       -----");
                sw.WriteLine(Ex.Data.ToString());
                sw.WriteLine("");
            }
            //Environment.Exit(0);
        }
    }
}

