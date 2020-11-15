using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers {
    public interface IMetasiteFileWriteHelper {
        Task WriteTextAsync(string filePath, string text);
    }

    public class MetasiteFileWriteHelper : IMetasiteFileWriteHelper {
        private const string FileName = "CityWeather.txt";

        public async Task WriteTextAsync(string filePath, string text) {
            using (EventLog eventLog = new EventLog("Application")) {
                try {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Saving file...", EventLogEntryType.Information, 101, 1);

                    byte[] encodedText = Encoding.Unicode.GetBytes(text);

                    using (FileStream sourceStream = new FileStream($"{filePath}/{FileName}",
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true)) {
                        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);

                        eventLog.WriteEntry("File saved", EventLogEntryType.Information, 101, 1);
                    };
                } catch (Exception ex) {
                    eventLog.WriteEntry(
                        $"Exception occurred during file saving, message: {ex.Message}",
                        EventLogEntryType.Error, 101, 1);
                    throw ex;
                }
            }
        }
    }
}
