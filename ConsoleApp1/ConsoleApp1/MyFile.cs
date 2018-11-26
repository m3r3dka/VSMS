using System;
using System.Collections.Generic;
using System.Text;

namespace SklShingle
{
    class MyFile
    {
        public string FilePath;
        public string Filename;
        public List<string> hashes;

        public MyFile(string FilePath, string Filename, List<string> hashes)
        {
            this.FilePath = FilePath;
            this.Filename = Filename;

            //Удаление одинаковых хэшей
            for (int i = 0; i < hashes.Count - 1; i++)
            {
                for (int j = i + 1; j < hashes.Count - 1; j++)
                {
                    if (hashes[i] == hashes[j])
                    {
                        hashes.Remove(hashes[i]);
                        i--;
                    }
                }
            }
            this.hashes = hashes;
        }
    }
}
