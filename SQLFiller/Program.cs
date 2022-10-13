using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace SQLFiller
{
    internal class Program
    {
        static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static string getValues(string[] lines)
        {
            //
            Random random = new Random();
            List<string> randomInput = new List<string>();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (lines[i].Contains("#")) continue;
                if (lines[i].Contains("VARCHAR"))
                {
                    int start = lines[i].IndexOf("VARCHAR(") + "VARCHAR(".Length;
                    int count = int.Parse(lines[i].Substring(start, lines[i].IndexOf(")") - start));
                    randomInput.Add(RandomString(count));
                }
                else
                    randomInput.Add(random.Next(1, 16).ToString());
            }
            return String.Join(",", randomInput.ToArray());
        }
        static string ParseSql(string str)
        {
            //str = "CREATE TABLE vehicle(    # vehicleId: Unique ID for Primary Key.    # This is how we will reference a record  vehicleId INT NOT NULL,      make VARCHAR(64), # String 64 chars max    model VARCHAR(128),    derivative VARCHAR(255),    PRIMARY KEY(vehicleId) ); CREATE TABLE vehicle(    # vehicleId: Unique ID for Primary Key.    # This is how we will reference a record  vehicleId INT NOT NULL,      make VARCHAR(64), # String 64 chars max    model VARCHAR(128),    derivative VARCHAR(255),    PRIMARY KEY(vehicleId) );";
            string[] arr = str.Split("CREATE TABLE");
            string newscript = "";
            string result = "";

            int rowCount = 0;
            while (true)
            {
                Console.WriteLine("Количество строк:");
                rowCount = int.Parse(Console.ReadLine());

                if (rowCount < 100 && rowCount >1) break;
            }
            foreach (var table in arr)
            {
                if (table == String.Empty) continue;
                string tableName = table.Substring(1, table.IndexOf("(") - 1);
                string inner = table.Substring(table.IndexOf("("), table.IndexOf(");") - table.IndexOf("("));

                string[] lines = inner.Split(",");
                for (int i = 0; i < rowCount; i++)
                {
                    newscript += $"INSERT INTO {tableName} VALUES({getValues(lines)})\n";
                }
                Console.WriteLine(newscript);
                result += newscript;
                newscript = "";
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу:");
            string path = Console.ReadLine();
            string result = readSql(path);
            string sql = ParseSql(result);

            Console.WriteLine("Хотите записать файл?(Y/N)");
            if (Console.ReadLine().ToLower() == "y")
                writeSql(sql);
        }


        static string readSql(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else return null;
        }
        static void writeSql(string text)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\result.sql";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, text);
            }
        }
    }
}


//var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);







