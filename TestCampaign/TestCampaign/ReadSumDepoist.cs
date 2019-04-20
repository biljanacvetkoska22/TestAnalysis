using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCampaign
{
    class ReadSumDepoist
    {
        public ReadSumDepoist()
        {

        }

        public static List<Deposit> GetSumDeposit(string csvFilePath, List<User> users)
        {
            var res = new List<Deposit>();
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                var line = sr.ReadLine(); // ignoring first line 
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    var values = line.Split(',');
                    var userID = values[0];
                    foreach(var user in users)
                    {
                        if (user.Id.Equals(userID))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(csvFilePath);                           

                            var depoist = new Deposit();
                            depoist.Id = userID;
                            depoist.CampaignName = user.Campaign;
                            depoist.Deposited = values[8];
                            depoist.Date = values[13];
                            res.Add(depoist);
                        }
                    }
                }
            }

            return res;
        }
    }
}
