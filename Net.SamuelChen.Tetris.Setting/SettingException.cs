using System;

namespace Net.SamuelChen.Tetris.Setting {
    public class SettingException : Exception {
        public SettingException(string message, Exception innerException) 
            : base(message, innerException) {}
    }
}
