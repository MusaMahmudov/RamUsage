
using System.Management;

 ulong GetTotalMemoryInMiB()
{
    var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
    ulong totalCapacity = 0;

    foreach (var item in searcher.Get())
    {
        totalCapacity += (ulong)item["Capacity"];
    }

    return totalCapacity / (1024 * 1024);
}
 ulong GetAvailableMemoryInMiB()
{
    var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
    ulong freeMemory = 0;
    var data = searcher.Get();

    foreach (var item in data)
    {
        freeMemory = (ulong)item["FreePhysicalMemory"];
    }

    return freeMemory / 1024;
}

Timer _timer;


void RecordMemoryUsage(object state)
{
    try
    {
        ulong totalMemory = GetTotalMemoryInMiB();
        ulong availableMemory = GetAvailableMemoryInMiB();

        string data = $"{DateTime.Now}: Total Memory: {totalMemory} MB, Available Memory: {availableMemory} MB";
        var file = "C:\\Users\\binch\\OneDrive\\Рабочий стол\\AISTGroup\\RamApplication\\RamApplication\\LogFile.txt";

        using (StreamWriter writer = new StreamWriter(file,true))
        {
            writer.WriteLine(data);
        }

        Console.WriteLine(data);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error recording memory usage: {ex.Message}");
    }
}
_timer = new Timer(RecordMemoryUsage, null, 0, 15000);
Console.ReadLine();
