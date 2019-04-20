using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCampaign
{
    class GetCustomerIdOpened
    {
        public GetCustomerIdOpened()
        {

        }

        /// <summary>
        /// takes into consideration the ones that have clicked
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <returns></returns>
        public static List<string> GetIdAffected(string csvFilePath)
        {
            var res = new List<string>();
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                var line = sr.ReadLine(); // ignoring first line 
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    var values = line.Split(',');
                    Int32.TryParse(values[3], out int clicked);
                    if(clicked > 0)
                    {
                        res.Add(values[0]);
                    }
                }                
            }

            return res;
        }
    }
}
