using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCampaign
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the full path of the data");
            var path = Console.ReadLine();

            var dirInfo = new DirectoryInfo(path);           

            var resultCsvFilePath = Path.Combine(path, $"{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}_Campaign_Affected.csv");
            if (!File.Exists(resultCsvFilePath))
            {
                File.Create(resultCsvFilePath).Close();
            }            

            var userIdsOpenedFileDates = Path.Combine(path, $"{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}_User_Ids_Clicked_Date.csv");
            if (!File.Exists(userIdsOpenedFileDates))
            {
                File.Create(userIdsOpenedFileDates).Close();
            }

            var depositsResults = Path.Combine(path, $"{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}_Affected_Deposiots.csv");
            if (!File.Exists(depositsResults))
            {
                File.Create(depositsResults).Close();
            }

            if (dirInfo.GetFiles() != null)
            {
                FileInfo[] files = dirInfo.GetFiles();

                Dictionary<string, Dictionary<int, float>> results = new Dictionary<string, Dictionary<int, float>>();
                Dictionary<int, int> res = new Dictionary<int, int>();
                List<string> userIdsAffected = new List<string>();
                Dictionary<string, string> campaignAndUserIDs = new Dictionary<string, string>();
                Dictionary<string, string> uIdDate = new Dictionary<string, string>();
                Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
                List<User> users = new List<User>();
                List<Deposit> deposits = new List<Deposit>();

                foreach (var file in files)
                {
                    if (file.Name.StartsWith("campaign"))
                    {
                        res = ReadCsvFile.ReadFileRowSum(file.FullName);
                        var result = res.Keys.First();
                        float percentage;
                        var clicked = res.Values.First();
                        var opened = res.Keys.First();
                        if (res.Values.First() == 0) //opened
                        {
                            percentage = 0;
                        }
                        else
                        {
                            percentage = (float)Math.Round((float)(100*clicked) / opened); // 
                        }

                        var dic = new Dictionary<int, float>();
                        dic.Add(result, percentage);
                        results.Add(file.Name, dic);

                        userIdsAffected = GetCustomerIdOpened.GetIdAffected(file.FullName);
                        foreach (var userId in userIdsAffected)
                        {
                            if (campaignAndUserIDs.Keys.Contains(userId))
                                continue;

                            campaignAndUserIDs.Add(userId, file.Name);
                        }

                        uIdDate = GetCustomerIdAndDate.GetIdAffected(file.FullName);
                        foreach (var uId in uIdDate)
                        {
                            var user = new User();
                            user.Id = uId.Key;
                            user.Date = uId.Value;
                            user.Campaign = file.Name;

                            users.Add(user);
                        }
                    }
                }


                using (var writer = new StreamWriter(resultCsvFilePath))
                {
                    writer.WriteLine("Campaign Name, Opened Number, Percentage Clicked");
                    writer.Flush();

                    foreach (var kvp in results)
                    {
                        var line = string.Format($"{kvp.Key.ToString()}, {kvp.Value.Keys.First().ToString()}," +
                            $"{kvp.Value.Values.First().ToString()}");
                        writer.WriteLine(line);
                        writer.Flush();
                    }
                }                

                using (var writer = new StreamWriter(userIdsOpenedFileDates))
                {
                    writer.WriteLine("User ID, Date, Campaign Name");
                    writer.Flush();

                    foreach (var user in users)
                    {
                        var line = string.Format($"{user.Id}, {user.Date}, {user.Campaign} ");
                        writer.WriteLine(line);
                        writer.Flush();
                    }
                }


                foreach (var file in files)
                {
                    var finalDeposits = new List<Deposit>();
                    if (file.Name.StartsWith("transactions_daily_"))
                    {
                        deposits = ReadSumDepoist.GetSumDeposit(file.FullName, users);
                        finalDeposits.AddRange(deposits);
                    }                    
                }

                using (var writer = new StreamWriter(depositsResults))
                {
                    writer.WriteLine("Id, Campaign Name, Deposited, Date");
                    writer.Flush();

                    foreach (var deposit in deposits)
                    {
                        var line = string.Format($"{deposit.Id}, {deposit.CampaignName}, {deposit.Deposited}, {deposit.Date} ");
                        writer.WriteLine(line);
                        writer.Flush();
                    }

                }
            }
        }
    }
}
