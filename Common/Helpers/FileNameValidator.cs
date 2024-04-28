namespace Common.Helpers
{
    public static class FileNameValidator
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        public static bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            if (fileName.IndexOfAny(InvalidFileNameChars) != -1)
            {
                return false;
            }

            return true;
        }
    }
}
