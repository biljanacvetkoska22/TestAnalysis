using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCampaign
{
    class ReadCsvFile 
    {
        public ReadCsvFile()
        {

        }
        /// <summary>
        /// return kvp (opened, clicked)
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <returns></returns>
        public static Dictionary<int, int> ReadFileRowSum(string csvFilePath)
        {
            var clicked = 0;
            var opened = 0;
            var res = new Dictionary<int, int>();
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                var line = sr.ReadLine(); // ignoring first line 
                while(!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    var values = line.Split(',');
                    Int32.TryParse(values[3], out int value);
                    Int32.TryParse(values[2], out int open);
                    opened += open;
                    clicked += value;                    
                }

                res.Add(opened, clicked);
            }

            return res;
        }

        void Dispose()
        {
                      
        }
    }
}
