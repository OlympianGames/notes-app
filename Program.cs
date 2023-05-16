using System.Diagnostics;

namespace CopperStudios;

public static class Program
{
    private static bool runUpdate = true;

    private static List<Note> notes = new List<Note>();

    public static void Main()
    {
        while (runUpdate)
        {
            UpdateLoop();
        }
    }

    private static void UpdateLoop()
    {
        GetNoteFiles();

        Console.Clear();
        DebugEngine.Log("");
        DebugEngine.Log("");
        DebugEngine.Log("What would you like to do?", false);
        DebugEngine.Log("");
        DebugEngine.Log("[A] Create Note", false);
        DebugEngine.Log("[B] Open Note", false);
        DebugEngine.Log("[C] List Notes", false);
        DebugEngine.Log("[D] Delete Note", false);
        DebugEngine.Log("[E] Stop", false);
        DebugEngine.Log("");
        DebugEngine.Log("");

        string? consoleInput = Console.ReadLine();

        if(consoleInput == null)
            return;

        consoleInput = consoleInput.ToLower();

        switch (consoleInput)
        {
            case "a":
                CreateNote();
                break;
            case "b":
                OpenNote();
                break;
            case "c":
                ListNotes();
                break;
            case "d":
                DeleteNote();
                break;
            case "e":
                runUpdate = false;
                break;
            default:
                break;
        }
    }

    private static void GetNoteFiles()
    {
        notes.Clear();
        
        string[] filePaths = Directory.GetFiles("notes", "*.txt", SearchOption.AllDirectories);

        foreach (var path in filePaths)
        {
            Note note = new Note();
            note.path = path;
            note.name = Path.GetFileNameWithoutExtension(path);
            notes.Add(note);
        }

    }

    private static string GetNotePath(string noteName)
    {
        foreach (var note in notes)
        {
            if(note.name == noteName)
                return note.path;
        }

        return "";
    }

    private static void CreateNote()
    {
        Console.Clear();
        DebugEngine.Log("");
        DebugEngine.Log("");
        DebugEngine.Log("What would you like the note to be called?", false);
        DebugEngine.Log("Note - Do not include an extension", false);
        DebugEngine.Log("");
        DebugEngine.Log("");

        string? consoleInput = Console.ReadLine();

        if(consoleInput == null)
            return;

        consoleInput = consoleInput.ToLower().Replace(" ", "-");

        File.Create($"notes/{consoleInput}.txt").Dispose();
    }

    private static void OpenNote()
    {
        Console.Clear();
        DebugEngine.Log("");
        DebugEngine.Log("");
        DebugEngine.Log("What note do you want to open?", false);
        DebugEngine.Log("");
        foreach (var note in notes)
        {
            DebugEngine.Log(Path.GetFileNameWithoutExtension(note.name), false);
        }
        DebugEngine.Log("");

        string? consoleInput = Console.ReadLine();

        if(consoleInput == null)
            return;

        consoleInput = consoleInput.ToLower().Replace(" ", "-");


        string targetPath = GetNotePath(consoleInput);
        if(File.Exists(targetPath))
        {
            var fileToOpen = $@"{Directory.GetCurrentDirectory()}/{targetPath}";
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = fileToOpen
            };

            process.Start();
            process.WaitForExit();
        }
        else
        {
            DebugEngine.Log("");
            DebugEngine.LogError("Could not find target note");
            DebugEngine.Log("");
            Console.ReadLine();
        }
    }

    private static void ListNotes()
    {
        Console.Clear();

        DebugEngine.Log("");
        DebugEngine.Log("List of current notes", false);
        DebugEngine.Log("");

        foreach (var note in notes)
        {
            DebugEngine.Log("");
            DebugEngine.Log(Path.GetFileNameWithoutExtension(note.name), false);
            DebugEngine.Log(note.path, false);
            DebugEngine.Log("");
        }

        DebugEngine.Log("");

        Console.ReadLine();
    }

    private static void DeleteNote()
    {
        Console.Clear();
        DebugEngine.Log("");
        DebugEngine.Log("");
        DebugEngine.Log("What note do you want to delete?", false);
        DebugEngine.Log("");
        foreach (var note in notes)
        {
            DebugEngine.Log(Path.GetFileNameWithoutExtension(note.name), false);
        }
        DebugEngine.Log("");

        string? consoleInput = Console.ReadLine();

        if(consoleInput == null)
            return;

        consoleInput = consoleInput.ToLower().Replace(" ", "-");

        File.Delete($"notes/{consoleInput}.txt");
    }

    public struct Note
    {
        public string name;
        public string path;
    }
}