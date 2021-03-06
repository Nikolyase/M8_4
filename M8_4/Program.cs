using System;

using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace M8_4
{

    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Student(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Укажите полный путь к бинарному файлу базы студентов");
            string FileNameS = Console.ReadLine();
            string DirName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";
            CreateDir(DirName);
            ReadBinStud(FileNameS, DirName);

            Console.ReadKey();
        }

        static void ReadBinStud(string pathFile, string path)
        {
            if (File.Exists(pathFile))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    using (var fs = new FileStream(pathFile, FileMode.OpenOrCreate))
                    {
                        Student[] newStudent = (Student[])formatter.Deserialize(fs);
                        Console.WriteLine($"База студентов {pathFile} десериализована");
                        Console.WriteLine();

                        foreach (Student st in newStudent)
                        {
                            Console.WriteLine($"Имя: {st.Name},  Группа: {st.Group},  День рождения {st.DateOfBirth.ToString("dd.MM.yyyy")}");
                            WriteGroup(path + @"\Группа_" + st.Group + ".txt", st);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Все студенты распределены на группы.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка чтения бинарного файла: {0}", ex.Message);
                }
            }
            else { Console.WriteLine($"Файла {pathFile} не существует по уазанному пути"); }
        }

        static void WriteGroup(string filePath, Student stud)
        {
            var fileInfo = new FileInfo(filePath);
            try
            {
                if (!fileInfo.Exists)
                {
                    using (StreamWriter sw = fileInfo.CreateText())
                    {
                        sw.WriteLine(stud.Name + ", " + stud.DateOfBirth.ToString("dd.MM.yyyy"));
                    }
                }
                else
                {
                    using (StreamWriter sw = fileInfo.AppendText())
                    {
                        sw.WriteLine(stud.Name + ", " + stud.DateOfBirth);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи студента в файл группы: {0}", ex.Message);
            }
        }

        static void CreateDir(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                try

                {
                    dirInfo.Create();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка создания папки: {0}", ex.Message);
                }
            }

        }

    }

}