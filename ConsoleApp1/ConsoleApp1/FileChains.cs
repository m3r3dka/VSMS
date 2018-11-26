using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace SklShingle
{
    class FileChains
    {
        public string name;
        public double simChains;
        public List<List<MyFile>> data;

        public FileChains (string name, List<List<MyFile>> data, double simChains)
        {
            this.name = name;
            this.data = new List<List<MyFile>>(data);
            this.simChains = simChains;
        }
        public void NormalizeChains()
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                double simFilesInChains = 0;
                int shorterList = 0;

                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[i].Count <= data[j].Count) shorterList = data[i].Count;
                    else shorterList = data[j].Count;

                    for (int k = 0; k < data[j].Count; k++) //бег по списку [i+1] подсчет элементов содержащихся в [i]
                    {
                        if (data[i].Contains(data[j][k]))
                        {
                            simFilesInChains++;
                        }
                    }
                    if (simFilesInChains / shorterList >= simChains)
                    {
                        data[i].AddRange(data[j]);
                        data.Remove(data[j]);
                        j--;
                    }
                    simFilesInChains = 0;
                }
                data[i] = data[i].Distinct().ToList();
            }
        }
    }
}
