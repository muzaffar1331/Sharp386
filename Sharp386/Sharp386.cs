using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharp386
{
    class Sharp386
    {
        static UInt32 RAM_SIZE = (UInt32)Math.Pow(2, 30);
        byte[] RAM = new byte[RAM_SIZE];

        bool Halted = true;

        System.Timers.Timer Timer;

        ulong InstructionsExecutedCount_Old = 0;
        ulong InstructionsExecutedCount_New = 0;
        public ulong InstructionsExecutedCount_Delta = 0;

        ulong ClockCount_Old = 0;
        ulong ClockCount_New = 0;
        public ulong ClockCount_Delta = 0;

        byte TempByte;
        UInt16 TempWord;
        UInt32 TempDword;
        UInt32 MAR;
        byte PIR;

        byte CIR;
        public UInt32 EIP;

        Mode ExecutionMode = Mode.Bits16;

        public UInt32 EAX;
        public UInt32 EBX;
        public UInt32 ECX;
        public UInt32 EDX;

        UInt32 EBP;
        UInt32 ESP;

        UInt32 ESI;
        UInt32 EDI;

        enum Mode
        {
            Bits16,
            Bits32,
        }

        //FLAGS [To be implemented]       

        byte ReadByte()
        {
            ClockCount_New++;

            byte value = RAM[EIP];
            EIP = (EIP + 1) % RAM_SIZE;

            return value;
        }

        UInt16 ReadWord()
        {
            ClockCount_New++;

            UInt16 value = (UInt16)((RAM[EIP]) | (RAM[EIP + 1] << 8));
            EIP = (EIP + 2) % RAM_SIZE;

            return value;
        }

        UInt32 ReadDword()
        {
            ClockCount_New++;

            UInt32 value = (UInt32)((RAM[EIP]) | (RAM[EIP + 1] << 8) | (RAM[EIP + 2] << 16) | (RAM[EIP + 3] << 24));
            EIP = (EIP + 4) % RAM_SIZE;

            return value;
        }

        public byte ReadByte(UInt32 address)
        {
            byte value = RAM[address];
            return value;
        }

        public UInt16 ReadWord(UInt32 address)
        {
            UInt16 value = (UInt16)((RAM[address]) | (RAM[address + 1] << 8));
            return value;
        }

        public UInt32 ReadDword(UInt32 address)
        {
            UInt32 value = (UInt32)((RAM[address]) | (RAM[address + 1] << 8) | (RAM[address + 2] << 16) | (RAM[address + 3] << 24));
            return value;
        }

        public void WriteByte(UInt32 address, byte data)
        {
            RAM[address] = data;
        }

        public void WriteWord(UInt32 address, UInt16 data)
        {
            TempByte = (byte)(data & 0x00FF);
            WriteByte(address, TempByte);

            TempByte = (byte)((data & 0xFF00) >> 8);
            WriteByte(address + 1, TempByte);
        }

        public void WriteDword(UInt32 address, UInt32 data)
        {
            TempByte = (byte)(data & 0x000000FF);
            WriteByte(address, TempByte);

            TempByte = (byte)((data & 0x0000FF00) >> 8);
            WriteByte(address + 1, TempByte);

            TempByte = (byte)((data & 0x00FF0000) >> 16);
            WriteByte(address + 2, TempByte);

            TempByte = (byte)((data & 0xFF000000) >> 24);
            WriteByte(address + 3, TempByte);
        }

        public Sharp386()
        {
            //for (int i = 0; i < RAM.Length; i++)
            //{
            //    RAM[i] = 0x90;
            //}

            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += (t, e) =>
              {
                  InstructionsExecutedCount_Delta = InstructionsExecutedCount_New - InstructionsExecutedCount_Old;
                  InstructionsExecutedCount_Old = InstructionsExecutedCount_New;

                  ClockCount_Delta = ClockCount_New - ClockCount_Old;
                  ClockCount_Old = ClockCount_New;
              };

            PIR = 0;
            CIR = 0;
            EIP = 0;

            EAX = 0;
            EBX = 0;
            ECX = 0;
            EDX = 0;

            EBP = 0;
            ESP = 0;

            ESI = 0;
            EDI = 0;
        }

        public void Start()
        {
            Halted = false;

            Timer.Start();

            while (!Halted)
            {
                Fetch();
                DecodeAndExecute();
            }
        }

        public void Stop()
        {
            Halted = true;
            Timer.Stop();
        }

        void Fetch()
        {
            CIR = ReadByte();
        }

        void DecodeAndExecute()
        {
            ClockCount_New++;

            switch (CIR)
            {
                //INC EAX
                case 0x40:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //INC AX
                        if (PIR != 0x66)
                        {
                            TempWord = (UInt16)(EAX & 0x0000FFFF);
                            TempWord++;
                            EAX = EAX & (0xFFFF0000) | TempWord;
                        }
                        //INC EAX
                        else
                        {
                            EAX++;
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //INC EAX
                        if (PIR != 0x66)
                        {
                            EAX++;
                        }
                        //INC AX
                        else
                        {
                            TempWord = (UInt16)(EAX & 0x0000FFFF);
                            TempWord++;
                            EAX = EAX & (0xFFFF0000) | TempWord;
                        }
                    }
                    break;

                //INC ECX
                case 0x41:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //INC CX
                        if (PIR != 0x66)
                        {
                            TempWord = (UInt16)(ECX & 0x0000FFFF);
                            TempWord++;
                            ECX = ECX & (0xFFFF0000) | TempWord;
                        }
                        //INC ECX
                        else
                        {
                            ECX++;
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //INC ECX
                        if (PIR != 0x66)
                        {
                            ECX++;
                        }
                        //INC CX
                        else
                        {
                            TempWord = (UInt16)(ECX & 0x0000FFFF);
                            TempWord++;
                            ECX = ECX & (0xFFFF0000) | TempWord;
                        }
                    }
                    break;

                //INC EDX
                case 0x42:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //INC DX
                        if (PIR != 0x66)
                        {
                            TempWord = (UInt16)(EDX & 0x0000FFFF);
                            TempWord++;
                            EDX = EDX & (0xFFFF0000) | TempWord;
                        }
                        //INC EDX
                        else
                        {
                            EDX++;
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //INC EDX
                        if (PIR != 0x66)
                        {
                            EDX++;
                        }
                        //INC DX
                        else
                        {
                            TempWord = (UInt16)(EDX & 0x0000FFFF);
                            TempWord++;
                            EDX = EDX & (0xFFFF0000) | TempWord;
                        }
                    }
                    break;

                //INC EBX
                case 0x43:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //INC BX
                        if (PIR != 0x66)
                        {
                            TempWord = (UInt16)(EBX & 0x0000FFFF);
                            TempWord++;
                            EBX = EBX & (0xFFFF0000) | TempWord;
                        }
                        //INC EBX
                        else
                        {
                            EBX++;
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //INC EBX
                        if (PIR != 0x66)
                        {
                            EBX++;
                        }
                        //INC BX
                        else
                        {
                            TempWord = (UInt16)(EBX & 0x0000FFFF);
                            TempWord++;
                            EBX = EBX & (0xFFFF0000) | TempWord;
                        }
                    }
                    break;

                //OPSIZE:
                case 0x66:
                    break;

                //MOV r8, [imm16]/[imm32]
                case 0x8A:
                    switch (ReadByte())
                    {
                        //MOV CL, [imm32]
                        case 0x0D:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            ECX = ECX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV CL, [imm16]
                        case 0x0E:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            ECX = ECX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV DL, [imm32]
                        case 0x15:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            EDX = EDX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV DL, [imm16]
                        case 0x16:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            EDX = EDX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV BL, [imm32]
                        case 0x1D:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            EBX = EBX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV BL, [imm16]
                        case 0x1E:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            EBX = EBX & (0xFFFFFF00) | TempByte;
                            break;

                        //MOV AH, [imm32]
                        case 0x25:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            EAX = EAX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV AH, [imm16]
                        case 0x26:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            EAX = EAX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV CH, [imm32]
                        case 0x2D:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            ECX = ECX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV CH, [imm16]
                        case 0x2E:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            ECX = ECX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV DH, [imm32]
                        case 0x35:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            EDX = EDX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV DH, [imm16]
                        case 0x36:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            EDX = EDX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV BH, [imm32]
                        case 0x3D:
                            MAR = ReadDword();
                            TempByte = RAM[MAR];
                            EBX = EBX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;

                        //MOV BH, [imm16]
                        case 0x3E:
                            MAR = ReadWord();
                            TempByte = RAM[MAR];
                            EBX = EBX & (0xFFFF00FF) | (UInt16)(TempByte << 8);
                            break;
                    }
                    break;

                //MOV r16, [imm16]
                case 0x8B:
                    switch (ReadByte())
                    {
                        //MOV CX, [imm32]
                        case 0x0D:
                            MAR = ReadDword();
                            TempWord = ReadWord(MAR);
                            ECX = ECX & (0xFFFF0000) | TempWord;
                            break;

                        //MOV CX, [imm16]
                        case 0x0E:
                            MAR = ReadWord();
                            TempWord = ReadWord(MAR);
                            ECX = ECX & (0xFFFF0000) | TempWord;
                            break;

                        //MOV DX, [imm32]
                        case 0x15:
                            MAR = ReadDword();
                            TempWord = ReadWord(MAR);
                            EDX = EDX & (0xFFFF0000) | TempWord;
                            break;

                        //MOV DX, [imm16]
                        case 0x16:
                            MAR = ReadWord();
                            TempWord = ReadWord(MAR);
                            EDX = EDX & (0xFFFF0000) | TempWord;
                            break;

                        //MOV BX, [imm32]
                        case 0x1D:
                            MAR = ReadDword();
                            TempWord = ReadWord(MAR);
                            EBX = EBX & (0xFFFF0000) | TempWord;
                            break;

                        //MOV BX, [imm16]
                        case 0x1E:
                            MAR = ReadWord();
                            TempWord = ReadWord(MAR);
                            EBX = EBX & (0xFFFF0000) | TempWord;
                            break;
                    }
                    break;

                //NOP
                case 0x90:
                    Thread.Sleep(500);
                    break;

                //MOV AL, [imm16]/[imm32]
                case 0xA0:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        MAR = ReadWord();
                    }
                    else
                    {
                        MAR = ReadDword();
                    }
                    TempByte = RAM[MAR];
                    EAX = EAX & (0xFFFFFF00) | TempByte;
                    break;

                //MOV AX, [imm16]
                case 0xA1:
                    if (PIR != 0x66)
                    {
                        MAR = ReadWord();
                        TempWord = ReadWord(MAR);
                        EAX = EAX & (0xFFFF0000) | TempWord;
                    }
                    //MOV AX, [imm32]
                    else
                    {
                        MAR = ReadDword();
                        TempWord = ReadWord(MAR);
                        EAX = EAX & (0xFFFF0000) | TempWord;
                    }

                    break;

                //MOV AL, imm8
                case 0xB0:
                    EAX = EAX & (0xFFFFFF00) | ReadByte();
                    break;

                //MOV CL, imm8
                case 0xB1:
                    ECX = ECX & (0xFFFFFF00) | ReadByte();
                    break;

                //MOV DL, imm8
                case 0xB2:
                    EDX = EDX & (0xFFFFFF00) | ReadByte();
                    break;

                //MOV BL, imm8
                case 0xB3:
                    EBX = EBX & (0xFFFFFF00) | ReadByte();
                    break;

                //MOV AH, imm8
                case 0xB4:
                    EAX = EAX & (0xFFFF00FF) | (UInt16)(ReadByte() << 8);
                    break;

                //MOV CH, imm8
                case 0xB5:
                    ECX = ECX & (0xFFFF00FF) | (UInt16)(ReadByte() << 8);
                    break;

                //MOV DH, imm8
                case 0xB6:
                    EDX = EDX & (0xFFFF00FF) | (UInt16)(ReadByte() << 8);
                    break;

                //MOV BH, imm8
                case 0xB7:
                    EBX = EBX & (0xFFFF00FF) | (UInt16)(ReadByte() << 8);
                    break;

                //MOV AX/EAX, imm16/imm32
                case 0xB8:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //MOV AX, imm16
                        if (PIR != 0x66)
                        {
                            EAX = EAX & (0xFFFF0000) | ReadWord();
                        }
                        //MOV EAX, imm32
                        else
                        {
                            EAX = ReadDword();
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //MOV EAX, imm32
                        if (PIR != 0x66)
                        {
                            EAX = ReadDword();
                        }
                        //MOV AX, imm16
                        else
                        {
                            EAX = EAX & (0xFFFF0000) | ReadWord();
                        }
                    }
                    break;

                //MOV CX/ECX, imm16/imm32
                case 0xB9:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //MOV CX, imm16
                        if (PIR != 0x66)
                        {
                            ECX = ECX & (0xFFFF0000) | ReadWord();
                        }
                        //MOV ECX, imm32
                        else
                        {
                            ECX = ReadDword();
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //MOV ECX, imm32
                        if (PIR != 0x66)
                        {
                            ECX = ReadDword();
                        }
                        //MOV CX, imm16
                        else
                        {
                            ECX = ECX & (0xFFFF0000) | ReadWord();
                        }
                    }
                    break;

                //MOV DX/EDX, imm16/imm32
                case 0xBA:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //MOV DX, imm16
                        if (PIR != 0x66)
                        {
                            EDX = EDX & (0xFFFF0000) | ReadWord();
                        }
                        //MOV EDX, imm32
                        else
                        {
                            EDX = ReadDword();
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //MOV EDX, imm32
                        if (PIR != 0x66)
                        {
                            EDX = ReadDword();
                        }
                        //MOV DX, imm16
                        else
                        {
                            EDX = EDX & (0xFFFF0000) | ReadWord();
                        }
                    }
                    break;

                //MOV BX/EBX, imm16/imm32
                case 0xBB:
                    if (ExecutionMode == Mode.Bits16)
                    {
                        //MOV BX, imm16
                        if (PIR != 0x66)
                        {
                            EBX = EBX & (0xFFFF0000) | ReadWord();
                        }
                        //MOV EBX, imm32
                        else
                        {
                            EBX = ReadDword();
                        }
                    }
                    else if (ExecutionMode == Mode.Bits32)
                    {
                        //MOV EBX, imm32
                        if (PIR != 0x66)
                        {
                            EBX = ReadDword();
                        }
                        //MOV BX, imm16
                        else
                        {
                            EBX = EBX & (0xFFFF0000) | ReadWord();
                        }
                    }
                    break;

                //JMP rel8
                case 0xEB:
                    EIP = (EIP + ReadByte() + 1) % 256;
                    break;

                //HLT
                case 0xF4:
                    Halted = true;
                    break;

                //INC / DEC
                case 0xFE:
                    TempByte = ReadByte();

                    switch (TempByte)
                    {
                        //INC byte [imm32]
                        case 0x05:
                            MAR = ReadDword();
                            RAM[MAR]++;
                            break;

                        //INC byte [imm16]
                        case 0x06:
                            MAR = ReadWord();
                            RAM[MAR]++;
                            break;

                        //INC AL
                        case 0xC0:
                            TempByte = (byte)(EAX & 0x000000FF);
                            TempByte++;
                            EAX = EAX & (0xFFFFFF00) | TempByte;
                            break;

                        //INC CL
                        case 0xC1:
                            TempByte = (byte)(ECX & 0x000000FF);
                            TempByte++;
                            ECX = ECX & (0xFFFFFF00) | TempByte;
                            break;

                        //INC DL
                        case 0xC2:
                            TempByte = (byte)(EDX & 0x000000FF);
                            TempByte++;
                            EDX = EDX & (0xFFFFFF00) | TempByte;
                            break;

                        //INC BL
                        case 0xC3:
                            TempByte = (byte)(EBX & 0x000000FF);
                            TempByte++;
                            EBX = EBX & (0xFFFFFF00) | TempByte;
                            break;

                        //INC AH
                        case 0xC4:
                            TempByte = (byte)((EAX & 0x0000FF00) >> 8);
                            TempByte++;
                            TempWord = (UInt16)(TempByte << 8);
                            EAX = EAX & (0xFFFF00FF) | TempWord;
                            break;

                        //INC CH
                        case 0xC5:
                            TempByte = (byte)((ECX & 0x0000FF00) >> 8);
                            TempByte++;
                            TempWord = (UInt16)(TempByte << 8);
                            ECX = ECX & (0xFFFF00FF) | TempWord;
                            break;

                        //INC DH
                        case 0xC6:
                            TempByte = (byte)((EDX & 0x0000FF00) >> 8);
                            TempByte++;
                            TempWord = (UInt16)(TempByte << 8);
                            EDX = EDX & (0xFFFF00FF) | TempWord;
                            break;

                        //INC BH
                        case 0xC7:
                            TempByte = (byte)((EBX & 0x0000FF00) >> 8);
                            TempByte++;
                            TempWord = (UInt16)(TempByte << 8);
                            EBX = EBX & (0xFFFF00FF) | TempWord;
                            break;
                    }
                    break;

                //INC / DEC
                case 0xFF:
                    TempByte = ReadByte();

                    switch (TempByte)
                    {
                        //INC dword [imm32]
                        case 0x05:
                            if (PIR != 0x66)
                            {
                                MAR = ReadDword();
                                TempDword = ReadDword(MAR);
                                TempDword++;
                                WriteDword(MAR, TempDword);
                            }
                            //INC word [imm32]
                            else
                            {
                                MAR = ReadDword();
                                TempWord = ReadWord(MAR);
                                TempWord++;
                                WriteWord(MAR, TempWord);
                            }
                            break;

                        //INC word [imm16]
                        case 0x06:
                            if (PIR != 0x66)
                            {
                                MAR = ReadWord();
                                TempWord = ReadWord(MAR);
                                TempWord++;
                                WriteWord(MAR, TempWord);
                            }
                            //INC dword [imm16]
                            else
                            {
                                MAR = ReadWord();
                                TempDword = ReadDword(MAR);
                                TempDword++;
                                WriteDword(MAR, TempDword);
                            }
                            break;
                    }
                    break;

                //Invalid Opcode
                default:
                    Halted = true;
                    throw new Exception("Invalid Opcode: 0x" + CIR.ToString("X"));
            }

            PIR = CIR;
            InstructionsExecutedCount_New++;
        }

    }
}
