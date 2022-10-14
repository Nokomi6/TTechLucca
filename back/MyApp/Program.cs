namespace firstProject
{
    class UniquePath
    {
        public List<string> Path;
        public List<int> IndexUse;
        public string Status;
        public List<double> Rate;

        public UniquePath(List<string> path, List<int> indexUse, string status, List<double> rate)
        {
            Path = path;
            IndexUse = indexUse;
            Status = status;
            Rate = rate;
        }
    }
    class CutFirstLine
    {
        string EntryCurrency;
        string AimCurrency;
        int StartMoney;

        public CutFirstLine(string entry)
        {
            string[] returnValue = entry.Split(";");
            if (returnValue.Length == 3)
            {
                EntryCurrency = returnValue[0];
                StartMoney = Int32.Parse(returnValue[1]);
                AimCurrency = returnValue[2];
            }
        }

        public string EntryCurr
        {
            get { return EntryCurrency; }
        }
        public string AimCurr
        {
            get { return AimCurrency; }
        }
        public int StartMon
        {
            get { return StartMoney; }
        }
    }
    class EntryArray
    {
        public string Entry;
        public string Output;
        public double Change;

        public EntryArray(string str)
        {
            string[] returnValue = str.Split(";");
            if (returnValue.Length == 3)
            {
                Entry = returnValue[0];
                Output = returnValue[1];
                Change = double.Parse(returnValue[2]);
            }
        }

        public string EntryCurr
        {
            get { return Entry; }
        }
        public string OutputCurr
        {
            get { return Output; }
        }
        public double ChangeRate
        {
            get { return Change; }
        }
    }

    class Program
    {
        List<EntryArray> ArrayEntry;
        List<UniquePath> UniquePaths;

        public Program(List<EntryArray> currencyArrayEntry, List<UniquePath> uniquePaths)
        {
            ArrayEntry = currencyArrayEntry;
            UniquePaths = uniquePaths;
        }

        public bool findIfBingo(List<EntryArray> entryArrays, CutFirstLine Cut, string entry)
        {
            bool check = entryArrays.Exists(x => ((x.Entry == entry && x.Output == Cut.AimCurr) || (x.Output == entry && x.Entry == Cut.AimCurr)));
            return check;
        }

        public void findFirstEndOrPotentialPath(CutFirstLine Cut, Program P)
        {
            bool check = P.findIfBingo(ArrayEntry, Cut, Cut.EntryCurr);
            if (check)
            {
                EntryArray bingo = ArrayEntry.Find(x => ((x.Entry == Cut.EntryCurr && x.Output == Cut.AimCurr) || (x.Output == Cut.EntryCurr && x.Entry == Cut.AimCurr)));
                double mult = bingo.Output == Cut.AimCurr ? bingo.Change : Math.Round(1 / bingo.Change, 4);
                Console.WriteLine(Math.Round(Cut.StartMon * mult));
                System.Environment.Exit(0);
            }
            else
            {
                int i = 0;
                foreach (var item in ArrayEntry)
                {
                    if (item.Entry == Cut.EntryCurr || item.Output == Cut.EntryCurr)
                    {
                        List<string> path = new List<string>();
                        List<int> indexUse = new List<int>();
                        List<double> rate = new List<double>();
                        path.Add(Cut.EntryCurr);
                        path.Add(item.Entry == Cut.EntryCurr ? item.Output : item.Entry);
                        indexUse.Add(i);
                        rate.Add(item.Entry == Cut.EntryCurr ? item.Change : Math.Round(1 / item.Change, 4));
                        UniquePath obj = new UniquePath(path, indexUse, "run", rate);
                        UniquePaths.Add(obj);
                    }
                    i++;
                }
            }
        }

        public void findEndOrPotentialPath(UniquePath uniquePath, int arrayIndex, CutFirstLine Cut, Program P)
        {
            string lastEntry = uniquePath.Path.Last();
            List<EntryArray> newEntryArrays = P.getArrayForCheckBingo(uniquePath);
            bool check = P.findIfBingo(newEntryArrays, Cut, lastEntry);
            if (check)
            {
                EntryArray bingo = ArrayEntry.Find(x => ((x.Entry == lastEntry && x.Output == Cut.AimCurr) || (x.Output == lastEntry && x.Entry == Cut.AimCurr)));
                UniquePaths[arrayIndex].Path.Add(Cut.AimCurr);
                UniquePaths[arrayIndex].Rate.Add(bingo.Output == Cut.AimCurr ? bingo.Change : Math.Round(1 / bingo.Change, 4));
                UniquePaths[arrayIndex].Status = "BINGO";
            }
            else
            {
                List<string> fixedPath = new List<string>(uniquePath.Path);
                List<int> fixedIndexUse = new List<int>(uniquePath.IndexUse);
                List<double> fixedRate = new List<double>(uniquePath.Rate);
                int i = 0;
                int j = 0;
                foreach (var item in ArrayEntry)
                {
                    if (!fixedIndexUse.Contains(j) &&
                    (!fixedPath.Contains(item.Entry) || !fixedPath.Contains(item.Output)) &&
                    (item.Entry == lastEntry || item.Output == lastEntry))
                    {
                        if (i == 0)
                        {
                            UniquePaths[arrayIndex].Path.Add(item.Entry == lastEntry ? item.Output : item.Entry);
                            UniquePaths[arrayIndex].IndexUse.Add(j);
                            UniquePaths[arrayIndex].Rate.Add(item.Entry == lastEntry ? item.Change : Math.Round(1 / item.Change, 4));
                            i++;
                        }
                        else
                        {
                            List<string> newListPath = new List<string>(fixedPath);
                            List<int> newListIndex = new List<int>(fixedIndexUse);
                            List<double> newListRate = new List<double>(fixedRate);
                            newListPath.Add(item.Entry == lastEntry ? item.Output : item.Entry);
                            newListIndex.Add(j);
                            newListRate.Add(item.Entry == lastEntry ? item.Change : Math.Round(1 / item.Change, 4));
                            UniquePath obj = new UniquePath(newListPath, newListIndex, "run", newListRate);
                            UniquePaths.Add(obj);
                        }
                    }
                    else
                    {
                        if (j + 1 == ArrayEntry.Count && i == 0)
                        {
                            UniquePaths[arrayIndex].Status = "stop";
                        }
                    }
                    j++;
                }
            }
        }

        public List<EntryArray> getArrayForCheckBingo(UniquePath path)
        {
            List<EntryArray> newEntryArrays = new List<EntryArray>();
            int i = 0;
            foreach (var item in ArrayEntry)
            {
                if (!path.IndexUse.Contains(i))
                {
                    newEntryArrays.Add(item);
                }
                i++;
            }
            return newEntryArrays;
        }

        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                // Get first Entry, valueMoney and Final Aim!
                CutFirstLine Cut = new CutFirstLine(args[0]);
                int currencyArg = Int32.Parse(args[1]);
                // Check how many line I have
                if (currencyArg > 0)
                {
                    // Create a list for all my entry and another for potentialPath
                    List<UniquePath> uniquePaths = new List<UniquePath>();
                    List<EntryArray> entryArrays = new List<EntryArray>();
                    for (int i = 2; i <= currencyArg + 1; i++)
                    {
                        EntryArray entryArray = new EntryArray(args[i]);
                        if (entryArray.Entry != entryArray.Output)
                        {
                            entryArrays.Add(entryArray);
                        }
                    }
                    if (entryArrays.Count > 0)
                    {
                        Program P = new Program(entryArrays, uniquePaths);
                        P.findFirstEndOrPotentialPath(Cut, P);
                        // If I have at least one Unique path I go check if bingo is possible
                        if (P.UniquePaths.Count >= 1)
                        {
                            while (P.UniquePaths.Exists(x => x.Status == "run"))
                            {
                                int index = 0;
                                foreach (var item in P.UniquePaths.ToList())
                                {
                                    if (item.Status == "run")
                                    {
                                        P.findEndOrPotentialPath(item, index, Cut, P);
                                    }
                                    index++;
                                }
                            }
                            if (P.UniquePaths.Exists(x => x.Status == "BINGO"))
                            {
                                UniquePath unique = P.UniquePaths.Find(x => x.Status == "BINGO");
                                foreach (var item in P.UniquePaths)
                                {
                                    if (item.IndexUse.Count < unique.IndexUse.Count && item.Status == "BINGO")
                                    {
                                        unique = item;
                                    }
                                }
                                double startMoney = Cut.StartMon;
                                foreach (var item in unique.Rate)
                                {
                                    startMoney = Math.Round(startMoney * item, 4);
                                }
                                Console.WriteLine(Math.Round(startMoney));
                            }
                            else
                            {
                                Console.WriteLine("No path possible in all the possibility");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No path possible");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No args passed");
            }
        }
    }
}

// dotnet run "EUR;550;JPY" 6 "AUD;CHF;0.9661" "JPY;KRW;13.1151" "EUR;CHF;1.2053" "AUD;JPY;86.0305" "EUR;USD;1.2989" "JPY;INR;0.6571"