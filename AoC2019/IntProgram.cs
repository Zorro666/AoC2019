using System;
using System.IO;

struct IntProgram
{
    long[] mInitialState;
    long[] mData;
    long[] mInputData;
    long mPC;
    long mRelativeBase;
    long mInputIndex;
    long mNextInput;
    bool mUseNextInput;

    public void LoadProgram(string inputFile)
    {
        var source = File.ReadAllText(inputFile);
        CreateProgram(source);
    }

    public void CreateProgram(string source)
    {
        var sourceElements = source.Split(',');
        mInitialState = new long[sourceElements.Length + 10000];
        mData = new long[sourceElements.Length + 10000];
        var index = 0;
        foreach (var element in sourceElements)
        {
            mInitialState[index] = long.Parse(element);
            index++;
        }
        for (; index < mData.Length; ++index)
        {
            mInitialState[index] = 0;
        }
        Reset();
    }

    public void Reset()
    {
        mPC = 0;
        mRelativeBase = 0;
        mInputIndex = 0;
        mNextInput = 0;
        mUseNextInput = false;
        for (int index = 0; index < mData.Length; ++index)
        {
            mData[index] = mInitialState[index];
        }
    }

    public void SetNextInput(long inputData)
    {
        mUseNextInput = true;
        mNextInput = inputData;
    }

    public void SetInputData(long[] inputData)
    {
        mInputData = inputData;
        mInputIndex = 0;
        mUseNextInput = false;
    }

    public void SetData(long index, long value)
    {
        //Console.WriteLine($"mData[{index}] {mData[index]}");
        mData[index] = value;
        //Console.WriteLine($"mData[{index}] {mData[index]}");
    }

    public long RunProgram(ref bool halt, ref bool hasOutput)
    {
        bool readInput = false;
        long result = -666;
        while (!halt)
        {
            result = SingleStep(ref halt, ref hasOutput, ref readInput);
        };
        return result;
    }

    public long GetNextOutput(ref bool halt, ref bool hasOutput)
    {
        bool readInput = false;
        long result = -666;
        while (!halt)
        {
            result = SingleStep(ref halt, ref hasOutput, ref readInput);
            if (hasOutput)
            {
                return result;
            }
        }
        return result;
    }

    public long SingleStep(ref bool halt, ref bool hasOutput, ref bool readInput)
    {
        long result = -666;
        if (mPC >= mData.Length)
        {
            throw new InvalidDataException($"Invalid pc:{mPC}");
        }
        hasOutput = false;
        var instruction = mData[mPC];
        long opcode = instruction % 100;
        long param1Mode = (instruction / 100) % 10;
        long param2Mode = (instruction / 1000) % 10;
        long param3Mode = (instruction / 10000) % 10;

        if ((param1Mode != 0) && (param1Mode != 1) && (param1Mode != 2))
        {
            throw new ArgumentOutOfRangeException(nameof(param1Mode), $"Invalid param1Mode:{param1Mode}");
        }
        if ((param2Mode != 0) && (param2Mode != 1) && (param2Mode != 2))
        {
            throw new ArgumentOutOfRangeException(nameof(param2Mode), $"Invalid param1Mode:{param2Mode}");
        }
        if ((param3Mode != 0) && (param3Mode != 2))
        {
            throw new ArgumentOutOfRangeException(nameof(param3Mode), $"Invalid param3Mode:{param3Mode}");
        }

        long param1Index;
        long param2Index;
        long param3Index;
        long param1;
        long param2;
        long param3;
        long output;
        switch (opcode)
        {
            case 1:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param3Index = mData[mPC + 3];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                param3 = GetOutputIndex(param3Mode, param3Index);
                output = param1 + param2;
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
                break;
            case 2:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param3Index = mData[mPC + 3];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                param3 = GetOutputIndex(param3Mode, param3Index);
                output = param1 * param2;
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
                break;
            case 3:
                param1Index = mData[mPC + 1];
                long index = GetOutputIndex(param1Mode, param1Index);
                MakeDataBigEnough(index);
                long input = mUseNextInput ? mNextInput : mInputData[mInputIndex++];
                mData[index] = input;
                readInput = true;
                mPC += 2;
                break;
            case 4:
                param1Index = mData[mPC + 1];
                param1 = GetParam(param1Mode, param1Index);
                result = param1;
                hasOutput = true;
                mPC += 2;
                return result;
            case 5:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                if (param1 != 0)
                {
                    mPC = param2;
                }
                else
                {
                    mPC += 3;
                }

                break;
            case 6:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                if (param1 == 0)
                {
                    mPC = param2;
                }
                else
                {
                    mPC += 3;
                }

                break;
            case 7:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param3Index = mData[mPC + 3];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                param3 = GetOutputIndex(param3Mode, param3Index);
                output = 0;
                if (param1 < param2)
                {
                    output = 1;
                }
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
                break;
            case 8:
                param1Index = mData[mPC + 1];
                param2Index = mData[mPC + 2];
                param3Index = mData[mPC + 3];
                param1 = GetParam(param1Mode, param1Index);
                param2 = GetParam(param2Mode, param2Index);
                param3 = GetOutputIndex(param3Mode, param3Index);
                output = 0;
                if (param1 == param2)
                {
                    output = 1;
                }
                MakeDataBigEnough(param3);
                mData[param3] = output;
                mPC += 4;
                break;
            case 9:
                param1Index = mData[mPC + 1];
                param1 = GetParam(param1Mode, param1Index);
                mRelativeBase += param1;
                mPC += 2;
                break;
            case 99:
                hasOutput = false;
                halt = true;
                return result;
            default:
                throw new InvalidDataException($"Unknown opcode:{opcode}");
        }
        return result;
    }

    long GetParam(long paramMode, long paramIndex)
    {
        if (paramMode == 1)
        {
            return paramIndex;
        }
        long index;
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
            throw new ArgumentOutOfRangeException(nameof(paramMode), $"Invalid paramMode {paramMode}");
        }
        MakeDataBigEnough(index);
        return mData[index];
    }

    void MakeDataBigEnough(long index)
    {
        if (index < 0)
        {
            throw new InvalidDataException($"Invalid parameter index {index}");
        }
        if (index >= mData.Length)
        {
            long[] newData = new long[index + 1];
            int i = 0;
            foreach (var d in mData)
            {
                newData[i] = d;
                ++i;
            }
            for (; i <= index; ++i)
            {
                newData[i] = 0;
            }
            mData = newData;
        }
    }

    private long GetOutputIndex(long paramMode, long paramIndex)
    {
        return paramMode switch
        {
            0 => paramIndex,
            2 => paramIndex + mRelativeBase,
            _ => throw new InvalidDataException($"Invalid input paramMode {paramMode}"),
        };
    }

}
