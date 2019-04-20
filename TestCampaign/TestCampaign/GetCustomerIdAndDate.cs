using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCampaign
{
    class GetCustomerIdAndDate
    {
        public GetCustomerIdAndDate()
        {

        }

        public static Dictionary<string, string> GetIdAffected(string csvFilePath)
        {
            var res = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                var line = sr.ReadLine(); // ignoring first line 
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    var values = line.Split(',');
                    Int32.TryParse(values[3], out int clicked);
                    if (clicked > 0 && !res.Keys.Contains(values[0]))
                    {
                        res.Add(values[0], values[1]);
                    }
                }
            }

            return res;
        }
    }
}
