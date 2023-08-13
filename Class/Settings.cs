using System;

[Serializable]
public static class Settings
{
    // all times (cursor, keyboard, scanColor, ...)

    public static int nextTableId = 0;
    //public static int nextRecordId;
    public static int currentlyUsedTableId;
    public static int currentlyUsedRecordId;

    public static string currentlyUsedFileName;
    public static bool isSaved;
}
