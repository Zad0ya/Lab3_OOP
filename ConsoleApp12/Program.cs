using System.IO;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "transactions.csv";
        string outputFilePath = "sum.csv";
        string dateFormat = "MM/DD/YYYY";

        Func<string, DateTime> getDate = line =>
        {
            string[] parts = line.Split(';');
            return DateTime.ParseExact(parts[0], dateFormat, CultureInfo.InvariantCulture);
        };

        Func<string, double> getSum = line =>
        {
            string[] parts = line.Split(',');
            return double.Parse(parts[1], CultureInfo.InvariantCulture);
        };

        Action<DateTime, double> setSum = (date, amount) =>
        {
            using (StreamWriter sw = new StreamWriter(outputFilePath, true))
            {
                sw.WriteLine($"{date.ToString(dateFormat)}, {amount.ToString("0.00", CultureInfo.InvariantCulture)}");
            }
        };

        int count = 0;
        DateTime Date = DateTime.MinValue;
        double dailyTotal = 0;

        using (StreamReader sr = new StreamReader(inputFilePath))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                DateTime date = getDate(line);
                double amount = getSum(line);

                if (date != Date)
                {
                    if (dailyTotal > 0)
                    {
                        setSum(Date, dailyTotal);
                    }
                    Date = date;
                    dailyTotal = 0;
                }

                dailyTotal += amount;
                count++;

                if (count % 10 == 0)
                {
                    setSum(Date, dailyTotal);
                    dailyTotal = 0;
                }
            }
        }

        if (dailyTotal > 0)
        {
            setSum(Date, dailyTotal);
        }
    }
}
