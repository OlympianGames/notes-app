namespace CopperStudios;

/// <summary> Internal engine for debugging </summary>
public static class DebugEngine
{   
    // Logs Settings
    /// <summary> Show logs in the console</summary>
    public static bool ShowLogs = true;
    /// <summary> Show error logs in the console</summary>
    public static bool ShowErrorsLogs = true;
    /// <summary> Show warning logs in the console</summary>
    public static bool ShowWarningLogs = true;
    /// <summary> Show important logs in the console</summary>
    public static bool ShowImportantLogs = true;


    // Data
    /// <summary> Every line ever written to the console</summary>
    public static List<string> logLines { get; private set; } = new List<string>();

    #region Log Writer

    /// <summary> Used for the writing of logs to a file at the end of the game </summary>
    private static void WriteLogs()
    {
        DebugEngine.Log("Settings Log Writer Variables");
        string currentDay = DateTime.Now.ToString("yyyy-MM-dd");
        string currentTime = DateTime.Now.ToString("h-mm-ss tt");

        string directoryPath = $"Logs/{currentDay}";
        string filePath = $"{directoryPath}/{currentTime}.txt";

        DebugEngine.Log("Create File Paths for Log Writer");
        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if(!File.Exists(filePath))
            File.Create(filePath).Dispose();

        DebugEngine.Log("Starting Log Writer");

        using(StreamWriter writer = new StreamWriter(filePath))
        {
            if(logLines == null)
                return;

            foreach (var line in logLines)
            {
                writer.WriteLine(line);
            }
        }

        DebugEngine.Log("Stopping Log Writer");

    }


    #endregion

    #region Logs

    /// <summary> Write a log the the console </summary>
    public static void Log(string message = "", bool showTitle = true)
    {
        if(ShowLogs)
            WriteConsoleMessage(new LogData("[Log]", message, ConsoleColor.DarkGray, showTitle));
    }

    /// <summary> Write a error to the console</summary>
    public static void LogError(string message = "", bool showTitle = true)
    {
        if(ShowErrorsLogs)
            WriteConsoleMessage(new LogData("[Error]", message, ConsoleColor.Red, showTitle));
    }

    /// <summary> Write a warning to the console </summary>
    public static void LogWarning(string message = "", bool showTitle = true)
    {
        if(ShowWarningLogs)
            WriteConsoleMessage(new LogData("[Warning]", message, ConsoleColor.Yellow, showTitle));
    }

    /// <summary> Writes an important log to the console </summary>
    public static void LogImportant(string message = "", bool showTitle = true)
    {
        if(ShowImportantLogs)
            WriteConsoleMessage(new LogData("[Important]", message, ConsoleColor.Gray, showTitle));
    }

    /// <summary> Writes a message to the console with a set color </summary>
    private static void WriteConsoleMessage(LogData data)
    {
        Console.ForegroundColor = data.color;

        string message = data.GetMessage();
        Console.WriteLine(message);
        logLines.Add(message);

        Console.ForegroundColor = ConsoleColor.White;
    }

    private class LogData
    {
        public ConsoleColor color = ConsoleColor.White;
        public string title = "";
        public string message = "";
        public bool showTitle = true;

        public LogData(){}

        public LogData(string title, string message, ConsoleColor color, bool showTitle = true)
        {
            this.color = color;
            this.title = title;
            this.message = message;
            this.showTitle = showTitle;
        }

        public string GetMessage()
        {
            if(string.IsNullOrEmpty(message))
                return "";
            else if(!showTitle)
                return message;
            else
                return $"{title} {message}";
        }
    }

    #endregion
}