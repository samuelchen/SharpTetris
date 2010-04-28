using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Controller {
    public enum EnumControllerType {
        Virtual = 0,
        Keyboard,
        Joystick,
    }


    public enum EnumControllerFactoryType {
        Virtual = 0,
        DirectX = 1,
    }

}
