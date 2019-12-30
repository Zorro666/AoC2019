using System;
using System.IO;

struct IntProgram
{
    Int64[] mData;
    Int64 mRelativeBase;
    Int64 mPC;

    public void LoadProgram(string inputFile)
    {
        var source = File.ReadAllText(inputFile);
        CreateProgram(source);
    }

    public void CreateProgram(string source)
    {
        var sourceElements = source.Split(',');
        mData = new Int64[sourceElements.Length];
        var index = 0;
        foreach (var element in sourceElements)
        {
            mData[index] = Int64.Parse(element);
            index++;
        }
        mPC = 0;
        mRelativeBase = 0;
    }

    public Int64 RunProgram(Int64 input, ref bool halt, ref bool hasOutput)
    {
        Int64 instruction = mData[mPC];
        Int64 result = -666;
        while (instruction != 99)
        {
            if (mPC >= mData.Length)
            {
                throw new InvalidDataException($"Invalid pc:{mPC}");
            }
            Int64 opcode = instruction % 100;
            Int64 param1Mode = (instruction / 100) % 10;
            Int64 param2Mode = (instruction / 1000) % 10;
            Int64 param3Mode = (instruction / 10000) % 10;

            if ((param1Mode != 0) && (param1Mode != 1) && (param1Mode != 2))
            {
                throw new ArgumentOutOfRangeException("param1Mode", $"Invalid param1Mode:{param1Mode}");
            }
            if ((param2Mode != 0) && (param2Mode != 1) && (param2Mode != 2))
            {
                throw new ArgumentOutOfRangeException("param2Mode", $"Invalid param1Mode:{param2Mode}");
            }
            if ((param3Mode != 0) && (param3Mode != 2))
            {
                throw new ArgumentOutOfRangeException("param3Mode", $"Invalid param3Mode:{param3Mode}");
            }

            if (opcode == 1)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param3Index = mData[mPC + 3];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                Int64 param3 = GetOutputIndex(param3Mode, param3Index);
                Int64 output = param1 + param2;
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
            }
            else if (opcode == 2)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param3Index = mData[mPC + 3];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                Int64 param3 = GetOutputIndex(param3Mode, param3Index);
                Int64 output = param1 * param2;
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
            }
            else if (opcode == 3)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 index = GetOutputIndex(param1Mode, param1Index);
                MakeDataBigEnough(index);
                mData[index] = input;
                mPC += 2;
            }
            else if (opcode == 4)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param1 = GetParam(param1Mode, param1Index);
                result = param1;
                hasOutput = true;
                mPC += 2;
                return result;
            }
            else if (opcode == 5)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                if (param1 != 0)
                {
                    mPC = param2;
                }
                else
                {
                    mPC += 3;
                }
            }
            else if (opcode == 6)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                if (param1 == 0)
                {
                    mPC = param2;
                }
                else
                {
                    mPC += 3;
                }
            }
            else if (opcode == 7)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param3Index = mData[mPC + 3];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                Int64 param3 = GetOutputIndex(param3Mode, param3Index);
                Int64 output = 0;
                if (param1 < param2)
                {
                    output = 1;
                }
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
            }
            else if (opcode == 8)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param2Index = mData[mPC + 2];
                Int64 param3Index = mData[mPC + 3];
                Int64 param1 = GetParam(param1Mode, param1Index);
                Int64 param2 = GetParam(param2Mode, param2Index);
                Int64 param3 = GetOutputIndex(param3Mode, param3Index);
                Int64 output = 0;
                if (param1 == param2)
                {
                    output = 1;
                }
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
            }
            else if (opcode == 9)
            {
                Int64 param1Index = mData[mPC + 1];
                Int64 param1 = GetParam(param1Mode, param1Index);
                mRelativeBase += param1;
                mPC += 2;
            }
            else
            {
                throw new InvalidDataException($"Unknown opcode:{opcode}");
            }
            instruction = mData[mPC];
        }
        halt = true;
        return result;
    }

    Int64 GetParam(Int64 paramMode, Int64 paramIndex)
    {
        if (paramMode == 1)
        {
            return paramIndex;
        }
        Int64 index;
        if (paramMode == 0)
        {
            index = paramIndex;
        }
        else if (paramMode == 2)
        {
            index = paramIndex + mRelativeBase;
        }
        else
        {
            throw new ArgumentOutOfRangeException("paramMode", $"Invalid paramMode {paramMode}");
        }
        MakeDataBigEnough(index);
        return mData[index];
    }

    void MakeDataBigEnough(Int64 index)
    {
        if (index < 0)
        {
            throw new InvalidDataException($"Invalid parameter index {index}");
        }
        if (index >= mData.Length)
        {
            Int64[] newData = new Int64[index + 1];
            int i = 0;
            foreach (var d in mData)
            {
                newData[i] = d;
                ++i;
            }
            for (; i < index; ++i)
            {
                newData[i] = 0;
            }
            mData = newData;
        }
    }

    Int64 GetOutputIndex(Int64 paramMode, Int64 paramIndex)
    {
        if (paramMode == 0)
        {
            return paramIndex;
        }
        else if (paramMode == 2)
        {
            return paramIndex + mRelativeBase;
        }
        else
        {
            throw new InvalidDataException($"Invalid input paramMode {paramMode}");
        }
    }

}
