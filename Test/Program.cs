using System.Text;
using System.IO;
using System.Runtime.InteropServices;
{
    int indx = 0, Max = 0;
    double med = 0, xmean = 0, disp = 0, skv=0;
    var Frequency = new List<int>();
    var f_N = new List<float>();
    var CumulativeF = new List<int>();
    var PopFilm = new List<string>();

    Console.WriteLine("Введите название файла");
    try
    {
        using (StreamReader sr = new StreamReader(Console.ReadLine() + ".txt"))
        {
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                Frequency.Add(int.Parse(line));

                if (CumulativeF.Count == 0)
                    CumulativeF.Add(Frequency[indx]);
                else
                    CumulativeF.Add(Frequency[indx] + CumulativeF[indx - 1]);

                if (Max < Frequency[indx])
                    Max = Frequency[indx];
                indx++;
            }
        }
        for (int i = 0; i < Frequency.Count; i++)
        {
            if (Frequency[i] == Max)
                PopFilm.Add((i + 1).ToString());
            f_N.Add((float)Frequency[i] / CumulativeF.Last() * 100);
        }

        //Мода
        var mode = Frequency.GroupBy(g => g).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Select(g => g.Key);

        //Среднне + Дисперсия
        for (int i = 0; i < Frequency.Count; i++)
            xmean = xmean + (Frequency[i] * (i + 1));
        xmean = xmean / CumulativeF.Last();
        for (int i = 0; i < Frequency.Count; i++)
            disp = disp + (Frequency[i] * ((i + 1) * (i + 1)));
        disp = (disp / CumulativeF.Last()) - (xmean * xmean);
        skv = Math.Sqrt(disp);

        //Медиана
        if (Frequency.Count % 2 != 0)
            med = (double)(Frequency.Count + 1) / 2;
        else med = (double)((Frequency.Count / 2 + Frequency.Count / 2 - 1) / 2);

        using (StreamWriter writer = new StreamWriter("Result.txt", false))
        {
            writer.WriteLine("Х середнє = " + "{0: 0.000}", xmean);
            writer.WriteLine("Дисперсія = " + "{0: 0.000}", disp);
            writer.WriteLine("Середнє квадратичне відхилення розподілу = " + "{0: 0.000}", skv);
            writer.WriteLine("Мода = " + mode.ElementAt(0));
            writer.WriteLine("Медіана = " + med);
            if (PopFilm.Count == 1)
                writer.WriteLine("Фільм, який був переглянутий частіше за інші = " + PopFilm[0]);
            writer.WriteLine("        Частота   -   Сукупність частот");
            for (int i = 0; i < Frequency.Count; i++)
                writer.WriteLine("Фільм  :" + (i + 1) + "  " + Frequency[i] + "   --   " + CumulativeF[i] + "   --   " + "{0: 0.000}", f_N[i] + " %");
            writer.Close();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Ошибка! " + e.Message);
    }
    Console.ReadKey();
}