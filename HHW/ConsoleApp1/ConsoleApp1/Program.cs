    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Text;
    using System.Threading.Tasks;
    using System.Security.Cryptography;
    using System.IO;
using System.Diagnostics;



namespace SklShingle
{
    class Program
    {

        static List<MyFile> Files = new List<MyFile>(); //Все файлы из директории читаемых и сравниваемых файлов
        static FileChains FChains; //Список всех списков похожих
        static List<FileChains> Dates = new List<FileChains>(); //Список всех цепочек похожих по датам
        static Dictionary<string, double> settings = new Dictionary<string, double>(); //настройки
        static string directory = @"..\..\..\..\Source\Files\"; //путь к директории с входящими файлами
        static string pathToJsons = @"..\..\..\..\Source\Jsons\"; //путь сохранения информации о каждом файле
        static string pathToCfg = @"..\..\..\..\Source\Cfg\"; //путь сохранения информации о каждом файле

        static void Main(string[] args)
        {
            SettingsLoad(false);
            bool exit = false;

            while (!exit)
            {
                Console.Write("----->");
                string[] str = CommandRead(Console.ReadLine());

                #region command enter

                switch (str[0]) // "sl", "d", "ent", "overlap", "c", "p", "nosort", "smtr", "un", "only", ".only" 
                {
                    case "0":
                        exit = true;
                        break;

                    case "clear":   //чистка консоли
                        Console.Clear();
                        break;

                    case "exit":
                        exit = true;
                        break;

                    case "start":   //запуск программы
                        Files.Clear();
                        ClearData();

                        SortAllFiles();

                        Console.WriteLine("----->done");
                        break;

                    case "h":    //вывод всех команд

                        Console.WriteLine(
                            "------------------"
                            + "\n clear - чистка консоли"
                            + ";\n exit "
                            + ";\n start - запуск программы"
                            + ";\n add - возможность ввести текст для сортировки"
                            + ";\n show - вывод обрабатываемых файлов"
                            + ";\n show set - вывод настроек на данный момент"
                            + ";\n nosort - чистка хранившейся в памяти информации прошлых запусков"
                            + ";\n ---"
                            + ";\n overlap - ввод перекрытия чешуек"
                            + ";\n sl - ввод длины шингла"
                            + ";\n d - ввод минимального коэффициента сходства"
                            + ";\n c - ввод минимального коэффициента сходства для цепочек"
                            + ";\n p - вывод пар похожих (no)"
                            + ";\n un - вывод цепочек похожих файлов"
                            + ";\n un .only- вывод уникальных файлов"
                            + ";\n------------------\n");
                        break;

                    case "add":    //
                        string text = Console.ReadLine();
                        MemoryRise();
                        AddDoc(text);
                        
                        break;

                    case "show":    //вывод настроек на данный момент
                        if (str[1] == "set")
                        {
                            Console.WriteLine(
                                "____________________\n"
                                + " shingleLen " + settings["shingleLen"]
                                + ";\n simFacForFiles " + settings["simFacForFiles"]
                                + ";\n entropy " + settings["entropy"]
                                + ";\n overlap " + settings["overlap"]
                                + ";\n simFacForChains " + settings["simFacForChains"]
                                + ";\n____________________");
                        }
                        break;

                    case "ovlap":   //ввод перекрытия чешуек
                        if (Convert.ToInt32(str[1]) >= settings["shingleLen"])
                        {
                            Console.WriteLine("Can not do that, cause shingle length is " + settings["shingleLen"]);
                        }
                        else
                        {
                            settings["overlap"] = Convert.ToInt32(str[1]);
                            SettingsSave();
                            Console.WriteLine("overlap --> " + str[1]);
                        }
                        break;

                    case "sl":      //ввод длины шингла
                        if (Convert.ToInt32(str[1]) <= settings["overlap"])
                        {
                            Console.WriteLine("Can not do that, cause overlap is " + settings["overlap"]);
                        }
                        else
                        {
                            settings["shingleLen"] = Convert.ToInt32(str[1]);
                            SettingsSave();
                            Console.WriteLine("shingleLen --> " + str[1]);
                        }
                        break;

                    case "d":       //ввод минимального коэффициента сходства
                        if (Convert.ToDouble(str[1]) <= 1 && Convert.ToDouble(str[1]) >= 0)
                        {
                            settings["simFacForFiles"] = Convert.ToDouble(str[1]);
                            SettingsSave();
                            Console.WriteLine("simFacForFiles --> " + str[1]);
                        }
                        else { Console.WriteLine("Can be [0,1]"); }
                        break;

                    case "c":       //ввод минимального коэффициента сходства для цепочек
                        if (Convert.ToDouble(str[1]) <= 1 && Convert.ToDouble(str[1]) >= 0)
                        {
                            settings["simFacForChains"] = Convert.ToDouble(str[1]);
                            SettingsSave();
                            Console.WriteLine("simFacForChains --> " + str[1]);
                        }
                        else { Console.WriteLine("Can be [0,1]"); }
                        break;

                    case "p":       //вывод пар похожих
                        MemoryRise();
                        PairListShow();
                        break;

                    case "un":      //вывод цепочек похожих файлов
                        MemoryRise();
                        FullListShow(str[1]);
                        break;

                    case "nosort":   //чистка хранившейся в памяти информации прошлых запусков
                        MemoryRise();
                        ClearData();
                        Console.WriteLine("----->done");
                        break;

                    default: Console.WriteLine("----->This command was not found"); break;
                }
                Console.WriteLine("");
                #endregion

            }
        }
        static void AddDoc(string text)
        {
            Random rand = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<string> allFilesNames = Directory.GetFiles(directory, "*.txt").ToList<string>();
            List<string> hashes = new List<string>();
            bool stop = false;
            bool doNotSim = true;
            hashes = HashesGeneration(Canonize(text), (int)settings["shingleLen"], (int)settings["overlap"]);
            string fileName =Convert.ToString(rand.Next()) ;
            File.WriteAllText(directory + fileName + ".txt",text);
            MyFile myFile = new MyFile(directory + fileName + ".txt",fileName, hashes);
            
            for (int d = 0; d < Dates.Count; d++)
            {
                if (stop) break;
                for (int i = 0; i < Dates[d].data.Count; i++)
                {
                    if (stop) break;
                    for (int j = 0; j < Dates[d].data[i].Count; j++)
                    {
                        if (stop) break;
                        if (CompareFilesByHash(myFile, Dates[d].data[i][j]) >= settings["simFacForFiles"])
                        {
                            Dates[d].data[i].Add(myFile);
                            SaveData(Dates[d]);
                            Console.WriteLine(Dates[d].name);
                            stop = true;
                            doNotSim = false;
                        }
                    }
                }
            }
            if(doNotSim)
            {
                Dates[Dates.Count - 1].data.Add(new List<MyFile>() { myFile });
                SaveData(Dates[Dates.Count - 1]);
                Console.WriteLine(Dates[Dates.Count - 1].name);
            }
            watch.Stop();
            Console.WriteLine("TIME " + watch.ElapsedMilliseconds);
        }
        static void SettingsSave()
        {
            string jsonData = JsonConvert.SerializeObject(settings);
            File.WriteAllText(pathToCfg + "Settings" + ".json", jsonData);
        }
        static void SettingsLoad(bool reset)
        {
            if (File.Exists(pathToCfg + "Settings" + ".json") && !reset)
            {
                settings = JsonConvert.DeserializeObject<Dictionary<string, double>>(File.ReadAllText(pathToCfg + "Settings" + ".json"));
            }
            else
            {
                settings.Add("shingleLen", 3);
                settings.Add("simFacForFiles", 0.62);
                settings.Add("entropy", 0);
                settings.Add("overlap", 2);
                settings.Add("simFacForChains", 1);
            }
        }

        //Вывод цепочки пар похожих
        static void PairListShow()
        {
            for (int d = 0; d < Dates.Count; d++)
            {
                Console.WriteLine("\n---" + Dates[d].name + "---");
                Console.WriteLine("____________________");
                for (int i = 0; i < Dates[d].data.Count; i++)
                {
                    for (int k = 0; k < Dates[d].data[i].Count - 1; k++)
                    {
                        Console.Write("#(");
                        for (int j = 0; j < 2; j++)
                        {
                            Console.Write(Dates[d].data[i][k + j].Filename);
                            if (k + j != Dates[d].data[i].Count - 1) Console.Write("|");
                        }
                        Console.Write(");\n");
                    }
                }
                Console.WriteLine("____________________");
            }
        }

        //Вывод всех цепочек похожих
        static void FullListShow(string mode)
        {
            if (mode != ",only")
            {
                for (int d = 0; d < Dates.Count; d++)
                {
                    Console.WriteLine("---" + Dates[d].name + "---");
                    Console.WriteLine("____________________");
                    for (int i = 0; i < Dates[d].data.Count; i++)
                    {
                        Console.Write("#(");
                        for (int j = 0; j < Dates[d].data[i].Count; j++)
                        {
                            Console.Write(Dates[d].data[i][j].Filename);
                            if (j != Dates[d].data[i].Count - 1) Console.Write("|");
                        }
                        Console.Write(");\n");
                    }
                    Console.WriteLine("____________________");
                }

            }
            else if (mode == ",only")
            {
                for (int d = 0; d < Dates.Count; d++)
                {
                    Console.WriteLine("---" + Dates[d].name + "---");
                    Console.WriteLine("____________________");
                    for (int i = 0; i < Dates[d].data.Count; i++)
                    {
                        if (Dates[d].data[i].Count == 1)
                        {
                            Console.Write("#(" + Dates[d].data[i][0].Filename + ");\n");
                        }
                    }
                    Console.WriteLine("____________________");
                }
            }

        }

        //чистка памяти с прошлых запусков
        static void ClearData()
        {
            List<string> allNames = Directory.GetFiles(pathToJsons, "*.json").ToList<string>();
            for (int i = 0; i < allNames.Count; i++)
            {
                File.Delete(allNames[i]);
            }
            File.Delete(pathToCfg + "FileChains.json");
        }

        //чтение консольных команд
        static string[] CommandRead(string line)
        {
            string com = "";
            long tracker = 0;
            string[] fullcommand = new string[2];
            StringBuilder str = new StringBuilder(line);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '.')
                    str[i] = ',';

                if (str[i] != ' ')
                    com += str[i];

                else if (i > 0 && str[i] == ' ' && str[i - 1] != ' ')
                {
                    if (tracker > 1) break;
                    fullcommand[tracker] = com;
                    tracker++;
                    com = "";
                }
            }
            if (com == "") fullcommand[tracker] = "";
            else fullcommand[tracker] = com;

            return fullcommand;
        }

        //нормализация текста .txt файла
        static List<string> Canonize(string phrase)
        {
            string stop_symbols = ".,!?:;-/()";

            string[] stop_words = { "это", "как", "так", "и", "в", "над", "к", "до", "не", "из", "за",
                                    "на", "но", "за", "то", "с", "ли", "а", "во", "от", "со", "под",
                                    "для", "о", "же", "ну", "вы", "бы", "что", "кто", "он", "она" };

            List<string> listPhrase = new List<string>();
            string word = "";

            char[] charrPhrase = phrase.ToCharArray();


            for (int i = 0; i < charrPhrase.Length; i++)
            {
                for (int j = 0; j < stop_symbols.Length; j++)
                {
                    if (charrPhrase[i] == stop_symbols[j])
                    {
                        charrPhrase[i] = ' ';
                    }

                    if (charrPhrase[i] == ' ' && word != "")
                    {
                        listPhrase = WordCheck(word, stop_words, listPhrase);
                        word = "";
                    }
                }
                if (charrPhrase[i] != ' ')
                    word += charrPhrase[i];
            }

            return WordCheck(word, stop_words, listPhrase);

        }
        
            //удаление предлогов из .txt файла (для Cononize())
            static List<string> WordCheck(string word, string[] stop_words, List<string> listPhrase)
        {
            for (int k = 0; k < stop_words.Length; k++)
            {
                if (word == stop_words[k])
                {
                    word = ".";
                    break;
                }
            }
            if (word != ".")
                listPhrase.Add(word);
            word = "";

            return listPhrase;
        }

        //чтение нормализованного текста, разбиение на шинглы и вычисление хэшей каждого шингла
        static List<string> HashesGeneration(List<string> text, int shingleLen, int overlap)
        {
            int shingleCount = text.Count - shingleLen + 1;


            //List<Shingle> shingleList = new List<Shingle>();

            int currInteger = 0;
            MD5 mD = MD5.Create();
            List<string> hashes = new List<string>();
            while (true)
            {
                string currShingle = "";
                for (int j = 0; j < shingleLen && currInteger < text.Count; j++, currInteger++)
                {
                    currShingle += text[currInteger];
                }
                hashes.Add(GetMd5Hash(mD, currShingle));
                if (currInteger >= text.Count - 1) break;

                currInteger -= overlap;
            }
            return hashes;
        }

        //нормалицация имени файла (из пути к файлу к имени файла)
        static string NormalizeName(string name, string mode)
        {
            string n = "", n1 = "";
            StringBuilder str = new StringBuilder(name);

            if (mode == "FILE")
            {
                bool start = false;
                for (int i = name.Length - 1; i > 0; i--)
                {
                    if (str[i] == '.') start = true;
                    if (str[i] == '\\') break;
                    if (start)
                        n1 += str[i];
                }
                for (int i = n1.Length - 1; i > 0; i--)
                {
                    n += n1[i];
                }
            }

            else if (mode == "FOLDER")
            {
                for (int i = name.Length - 1; i >= 0; i--)
                {
                    if (str[i] == '\\') break;
                    else
                        n1 += str[i];
                }
                for (int i = n1.Length - 1; i >= 0; i--)
                {
                    n += n1[i];
                }
            }
            return n;
        }

        //поднятие памяти
        static void MemoryRise()
        {
            Files.Clear();
            Dates.Clear();

            List<string> allJsonsNames = Directory.GetFiles(pathToJsons, "*.json").ToList<string>();

            for (int i = 0; i < allJsonsNames.Count; i++)
            {
                Dates.Add(JsonConvert.DeserializeObject<FileChains>(File.ReadAllText(allJsonsNames[i])));
            }
        }

        //заполнение списка .txt файлов файлами из памяти и дополнение новыми
        static void SortAllFiles()
        {
            List<string> allDates = Directory.GetDirectories(directory).ToList<string>();

            for (int d = 0; d < allDates.Count; d++)
            {
                string date = NormalizeName(allDates[d], "FOLDER");

                FillFileList(allDates[d], date);
                CreateChainOfSimilar(settings["simFacForFiles"], settings["simFacForChains"], date);
                Console.WriteLine(date + " done");
                Files.Clear();
            }
        }

        static void FillFileList(string directory, string date)
        {
            List<string> allFilesNames = Directory.GetFiles(directory, "*.txt").ToList<string>();
            MyFile myFile;

            for (int i = 0; i < allFilesNames.Count; i++)
            {
                string fileName = NormalizeName(allFilesNames[i], "FILE");

                bool exists = false;
                List<string> Hashes = new List<string>();

                for (int j = 0; j < Files.Count; j++)
                {
                    if (fileName == Files[j].Filename)
                        exists = true;
                }

                if (!exists)
                {
                    string text = "";
                    using (StreamReader str = new StreamReader(allFilesNames[i]))
                    {
                        text = str.ReadToEnd();
                        str.Close();
                    }

                    Hashes = HashesGeneration(Canonize(text), (int)settings["shingleLen"], (int)settings["overlap"]);

                    myFile = new MyFile(allFilesNames[i], fileName, Hashes);

                    Files.Add(myFile);
                }

                exists = true;
            }
        }

        //создание списка списков похожих файлов
        static void CreateChainOfSimilar(double simFactor, double simChains, string Date) //заполнение цепочки списками похожих
        {
            List<MyFile> similar;
            FChains = new FileChains(Date, new List<List<MyFile>>(), simChains);

            for (int i = 0; i < Files.Count - 1; i++)
            {
                similar = new List<MyFile>();
                similar.Add(Files[i]);
                for (int j = i + 1; j < Files.Count; j++)
                {
                    if (CompareFilesByHash(Files[i], Files[j]) >= simFactor)
                    {
                        similar.Add(Files[j]);
                    }
                }
                FChains.data.Add(similar);

                if (i == Files.Count - 2 && !similar.Contains(Files[i + 1])) { FChains.data.Add(new List<MyFile>() { Files[i + 1] }); } //индивидуальное добавление последнего элемента в случае если он не схож с предыдущим
            }
            FChains.NormalizeChains();

            SaveData(FChains);
        }
        static public void SaveData(FileChains fch)
        {
            string jsonData = JsonConvert.SerializeObject(fch);

            File.WriteAllText(pathToJsons + fch.name + ".json", jsonData);
        }
        //сравнение хэшей шинглов файлов и возврат коэффициента сходства двух файлов (для CreateChainOfSimilar())
        static double CompareFilesByHash(MyFile File1, MyFile File2)
        {
            MyFile shorterFile, longerFile;

            if (File1.hashes.Count <= File2.hashes.Count) { shorterFile = File1; longerFile = File2; }
            else { shorterFile = File2; longerFile = File1; }
            double sameHashesCounter = 0;

            for (int i = 0; i < shorterFile.hashes.Count; i++)
            {
                for (int j = 0; j < longerFile.hashes.Count; j++)
                {
                    if (shorterFile.hashes[i] == longerFile.hashes[j])
                    {
                        sameHashesCounter++;
                    }
                }
            }
            double k1 = sameHashesCounter / ((shorterFile.hashes.Count + longerFile.hashes.Count) - sameHashesCounter);

            return k1;
        }

        //алгоритм получения хэша через функцию MD5 (для HashesGeneration())
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}




