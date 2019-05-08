// Question 1:
// workflow:
// 1. define inputSchema according to different formats of input file
// 2. read file into different input object
// 3. change type and currency according to lookup tables
// 4. convert dateformat into standard format
// 5. parse identifier into account code

// following codes are implementation of the workflow: 
// these are pseudo codes, only for demostration
// also, need some testcases to cover the code (I didn't write it.)

// by the way, using SSIS or other ETL tools will be helpful and will make coders' life easier :)
// =================================================
class InputSchema:IInputSchema
{
    public string LayoutClassName {get;set;}
    public bool HasHeader {get;set;}
    public string DateFormat {get;set;}
    public char Delimiter{get;set;} = ',';
    public int NumbersOfColumn {get;set;}
}


class InputColumn:IInputColumn
{
    public string ColumnName {get;set;}
    public string Value {get;set;}
    public string DataFormatRegex {get;set;} 
}

class StandardInputLayout:IStandardInputLayout
{
    public InputColumn Identifier {get;set;}
	public InputColumn Name {get; set;}
	public InputColumn Type {get; set;}
	public InputColumn Opened {get; set;}
	public InputColumn Currency {get; set;}

    public string this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return Identifier;
                case 1:
                    return Name;
                case 2:
                    return Type;
                case 3:
                    return Opened;
                case 4:
                    return Currency;
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    Identifier.Value = value;
                    break;
                case 1:
                    Name.Value = value;
                    break;
                case 2:
                    Type.Value = value;
                    break;
                case 3:
                    Opened.Value = value;
                    break;
                case 4:
                    Currency.Value = value;
                    break;
            }
        }
    }
}


class CustomInputOneLayout:ICustomInputOneLayout
{
	public InputColumn Name {get; set;}
	public InputColumn Type {get; set;}	
	public InputColumn Currency {get; set;}
    public InputColumn CustodianCode {get; set;}

    public string this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return Name;
                case 1:
                    return Type;
                case 2:
                    return Currency;
                case 3:
                    return CustodianCode;
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    Name.Value = value;
                    break;
                case 1:
                    Type.Value = value;
                    break;
                case 2:
                    Currency.Value = value;
                    break;
                case 3:
                    CustodianCode.Value = value;
                    break;
            }
        }
    }
}

class StandardOutputLayout : IStandardOutputLayout
{
    public GUID ID {get;set;}
	public string AccountCode {get; set;}
	public string Name {get; set;}
	public string Type {get; set;}
	public string OpenDate {get; set;}
	public string Currency {get; set;}
}


class Worker
{
    
    #key is the source value, value is the target value
    IDictionary<string, string> TypeLookupTable = new Dictionary<string, string>();
    TypeLookupTable.Add("1", "Trading")
    TypeLookupTable.Add("2", "RRSP")
    TypeLookupTable.Add("3", "RESP")
    TypeLookupTable.Add("4", "Fund")
    TypeLookupTable.Add("Trading", "Trading")
    TypeLookupTable.Add("RRSP", "RRSP")
    TypeLookupTable.Add("RESP", "RESP")
    TypeLookupTable.Add("Fund", "Fund")


    IDictionary<string, string> CurrencyLookupTable = new Dictionary<string, string>();
    CurrencyDictionary.Add("C", "CAD");
    CurrencyDictionary.Add("CD", "CAD");
    CurrencyDictionary.Add("U", "USD");
    CurrencyDictionary.Add("CD", "CAD");


    const string STANDARD_DATE_FORMAT = @"yyyy-mmd-dd";

    private ReadOnly _inputSchema;
    public void Worker(IInputSchema inputSchema)
    {
        _inputSchema = inputSchema;
    }

    public IList<IStandardOutputLayout> Transform(string inputFilePath)
    {
        # 1. read file, convert lines into layout object
        var file = readfile(inputFilePath, 'utf-8');
        switch(_inputSchema.LayoutClassName)
        {
            case "StandardInputLayout":
                IList<IStandardInputLayout> input = new GenericLayout(IStandardInputLayout).Generate(file, _inputSchema.Delimiter, 
                                                                            _inputSchema.NumbersOfColumn, _inputSchema.HasHeader)
                
                break;
            case "CustomInputOneLayout":
                IList<ICustomInputOneLayout> input = new GenericLayout(ICustomInputOneLayout).Generate(file, _inputSchema.Delimiter, 
                                                                            _inputSchema.NumbersOfColumn, _inputSchema.HasHeader)
                
                break;
        }
       IList<IStandardOutputLayout> output = GenerateOutput(input, _inputSchema.DateFormat);
       return output;
    }

    private IList<IStandardOutputLayout> GenerateOutput(IList<T> input, string dateFormat)
    {
         # 2. assign to output object
        IList<IStandardOutputLayout> result = new List<IStandardOutputLayout>{};
        foreach(var item in input)
        {
            try{       
                # 2.0 validate data according to its DataFormatRegex
                # more code .....

                IStandardOutputLayout outputItem = new StandardOutputLayout();

                outputItem.GUID = new GUID();
                if(typeof(item).HasProperty("Name"))
                {
                    outputItem.Name = item.Name.Value;
                }

                # 2.1convert type and currency according to lookup tables  
                if(typeof(item).HasProperty("Type"))
                {
                   outputItem.Type = ConverFromLookupTable(item.Type.Value, TypeLookupTable);
                }
                if(typeof(item).HasProperty("Currency"))
                {
                   outputItem.Currency = ConverFromLookupTable(item.Type.Value, CurrencyLookupTable);   
                }

                #2.2 convert dateformat for StandardInputLayout
                if(typeof(item).HasProperty("Currency"))
                {
                   outputItem.OpenDate = ConvertDateFormat(item.OpenDate.Value, dateFormat);
                }
                
                 #2.3 parse indentifier into AccountCode
                 if(typeof(item).HasProperty("Identifier"))
                {
                   outputItem.OpenDate = ParseIdentifier(item.Identifier.Value);
                }
                
            }
            catch{
                //log error, continue
            }
            result.Add(outputItem);
        }
        
        return result;
    }
    private string ParseIdentifier(string id)
    {
        return id.Split('|')[1];
    }
    private static HasProperty(this Type type, string propertyName)
    {
        # code .....
        return True;
    }
    private string ConverFromLookupTable(string source, IDictionary dict)
    {
        try{
            if (string.IsNullOrEmpty(item.Opened.Value))
            {
                return "";
            }
            return dict[source];
        }
        catch{
            throw;
        }
    }

    private string ConvertDateFormat(string source, string inputFormat)
    {
        if(inputFormat == STANDARD_DATE_FORMAT)
        {
            return source;
        }
        try{
            # more code......
            # validate yyyy, mm, dd ....
            return source.tostring(STANDARD_DATE_FORMAT)
        }
        catch{
            throw;
        }
        
    }

    class GenericLayout<T>
    {
        private T _genericLayout;
        public GenericLayout(T value)
        {
            _genericLayout = value;
        }
        public IList<T> Generate(stream file, char delimiter, int maxColumnNum, bool hasHeader)
        {
            IList<T> result = new List<T>{};
            int numOfLine = 0;
            while(!file.eof)
            {
                string line = file.ReadLine();
                numberOfLine ++;
                if(numOfLine == 1 && hasHeader){
                    continue;
                }            
                if(!string.IsNullOrEmpty(line))
                {
                    try
                    {
                        string[] fields = line.Split(delimiter);
                        T layout = ReadFieldsInSequence(fields, maxColumnNum);                    
                        result.Add(standard);
                    }
                    catch (System.Exception)
                    {                    
                        throw; //log error, ignore this line and continue
                    } 
                }
            }
        }
    }

    private T ReadFieldsInSequence(string[] fields, int maxColumnNum)
    {
        T result = new T();
        for(int index = 0; index < fields.Length; index++)
        {
            if(index == maxColumnNum - 1)
            {
                break;
            }
            result[index] = fields[index];
        }
        return T;
    }
}



// ================================
// Question 2:
// Secure code principles:
// 1. DRY and KISS
// 2. define access levels on applicatins, classes and so on.
// 3. expose minimum interface to public
// 4. users access permission
// 5. input data validation
// 6. deep knowledge of third party components
// 7. using encrytion
// 8. error handling, catch all exceptions, donnot release unnessary handling information (for example, avoid error message such as "username is not found in our database")
