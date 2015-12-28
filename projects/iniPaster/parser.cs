using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/*
 * Парсер конфигурационных файлов, автор Сергей Алексеев 
 * 2015, Волгоград
 * На кой ляд я это написал? Мне скучно...
 * 
 * 
 */

namespace iniPaster
{
    public class Constants
    {
        public static String EmptySearchResultFromBranch = null;
    }

    public class configFilesUniversalParser
    {

    }

    public class customConfigFormatParser:configParser
    {
        int parsePosition;

        int lexemPosition;

        public customConfigFormatParser(String configFilePath):base(configFilePath)
        {
            environmentInitialize();
        }

        public customConfigFormatParser loadConfigurationsFromConfigFile(String configFilePath)
        {
            customConfigFormatParser generatedParser = new customConfigFormatParser(configFilePath);
            generatedParser.parseFile();
            return generatedParser;
        }

        private void environmentInitialize()
        {
            requireFileExtensions.Add("ccf");
            requireFileExtensions.Add("ini");
            requireFileExtensions.Add("cnf");
            configFileContent = "";
        }

        public override bool checkFileExtensionForCompability()
        {
            Boolean fileIsCompatible=false;
            foreach(String correctExtension in requireFileExtensions)
            {
                fileIsCompatible = correctExtension == Path.GetExtension(configFilePath);
            }
            return fileIsCompatible;
        }

        public override void parseFile()
        {
            List<String> lexems;
            readyForParse();
            lexems = getLexemsList();
            generateParametersTreeFromLexems(lexems);
        }

        private void readyForParse()
        {
            parsePosition = 0;
            if (configFileContent.Length == 0)
                loadFileIntoMemory();
        }

        private List<String> getLexemsList()
        {
            List<String> lexems = new List<String>();
            while(moveIfCanToNexLexem())
            {
                lexems.Add(getNextLexem());
            }
            return lexems;
        }

        private bool moveIfCanToNexLexem()
        {
            if (parsePosition < configFileContent.Length)
            {
                while (isSymbolCorrectSpacer(configFileContent[parsePosition]) && configFileContent[parsePosition] != '=' )
                {
                    parsePosition++;
                    if (parsePosition >= configFileContent.Length)
                        return false;
                    
                }
                
            }
            else
            {
                return false;
            }
            return true;
        }

        private String getNextLexem()
        {
            String lexem = "";
            if (parsePosition < configFileContent.Length && configFileContent[parsePosition] == '=')
            {
                lexem = "=";
                parsePosition++;
            }
            else
            {
                while (parsePosition < configFileContent.Length && !isSymbolCorrectSpacer(configFileContent[parsePosition]) && configFileContent[parsePosition] != '=')
                {
                    lexem = lexem + configFileContent[parsePosition];
                    parsePosition++;
                }
            }
            return lexem;
        }

        private bool isSymbolCorrectSpacer(Char symbol)
        {
            return symbol == ' ' ||
                symbol == '\n' ||
                symbol == '\r' ||
                symbol == '\t' ||
                symbol == '\0';
        }

        private void generateParametersTreeFromLexems(List<String> lexems)
        {
            readyForGeneration();
            configProperty currentHead = headProperty;
            for (; lexemPosition < lexems.Count; lexemPosition++)
            {
                if (lexemIsSection(lexems[lexemPosition]))
                {
                    currentHead = generateSection(lexems);
                }
                else if (lexemIsParameter(lexems[lexemPosition]))
                {
                    generateParameter(lexems, currentHead);
                }
                else
                {
                    throw new WrongLexemException("Неожиданное выражение. Ожидалось либо новая секция, либо новый параметр. Получено - " + lexems[lexemPosition]);
                }
            }
        }

        private void readyForGeneration()
        {
            lexemPosition = 0;
        }

        private bool lexemIsSection(String lexem)
        {
            return lexem[0] == '[' && lexem[lexem.Length - 1] == ']';
        }

        private bool lexemIsParameter(String lexem)
        {
            return lexem[0] != '=';
        }

        private configProperty generateSection(List<String> lexems)
        {
            configProperty newProperty = configProperty.generatePropertyWithParent(lexems[lexemPosition], "", headProperty);
            headProperty.insertExistingChildProperty(newProperty);
            return newProperty;
        }

        private void generateParameter(List<String> lexems, configProperty head)
        {
            if (lexemPosition + 2 < lexems.Count)
            {
                if(lexems[lexemPosition + 1] != "=")
                {
                    throw new WrongLexemException("Неверное выражение. Требуется \"=\", получено - " + lexems[lexemPosition + 1]);
                }
                configProperty newProperty = configProperty.generatePropertyWithParent(lexems[lexemPosition], lexems[lexemPosition + 2], head);
                head.insertExistingChildProperty(newProperty);
            }
            else
            {
                throw new MissingLexemException("Неожиданный конец списка лексем. Не хватает значения параметра.");
            }
            lexemPosition += 2;
        }

        public override decimal getDecimalPropertyByName(string propertyName)
        {
            throw new NotImplementedException();
        }

        public override string getStringPropetryValueByName(string propertyName)
        {
            return headProperty.getValueFromBranchByName(propertyName);
        }

        public override void saveChangedFile()
        {
            throw new NotImplementedException();
        }

        public override void saveChangesToNewFile(string newFilePath)
        {
            throw new NotImplementedException();
        }

        public override void setDecimalPropertyValueByName(string propertyName, decimal propertyValue)
        {
            throw new NotImplementedException();
        }

        public override void setStringPropertyValueByName(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public bool unitTests()
        {
            if (testLexemParse() && treeGenerationTest() && fileTestParse() && treeGenerationTest())
                return true;
            else
                return false;
        }

        private bool testLexemParse()
        {
            Console.WriteLine("Config lexem parser starting...");
            configFileContent = "[section1]\nparameter1=value1\nparameter2 = value2\nparameter3= value3\n[section2]\nparameter4=value4";
            List<String> sample = new List<String> {"[section1]","parameter1","=","value1","parameter2","=","value2",
                                        "parameter3","=","value3","[section2]","parameter4","=","value4"};
            List<String> result = this.getLexemsList();
            if(sample.Count!=result.Count)
            {
                Console.WriteLine("CONFIG LEXEM PARSER: COUNT FAIL " + sample.Count + "!=" + result.Count);
                return false;
            }
            bool success = true;
            for(int i=0;i<sample.Count;i++)
            {
                if(sample[i]!=result[i])
                {
                    Console.WriteLine("CONFIG LEXEM PARSER: VALUE FAIL " + sample[i] + "!=" + result[i]);
                    success = false;
                }
            }
            return success;
        }

        private bool treeGenerationTest()
        {
            Console.WriteLine("Tree generation test started...");
            List<String> testSource = new List<String> {"[section1]","parameter1","=","value1","parameter2","=","value2",
                                        "parameter3","=","value3","[section2]","parameter4","=","value4"};
            generateParametersTreeFromLexems(testSource);
            bool success=true;
            for (int i = 1; i <= 4;i++ )
            {
                if (getStringPropetryValueByName("parameter" + i) != "value" + i)
                {
                    success = false;
                    Console.WriteLine("FAIL. Unexpected value. Recieved: " + getStringPropetryValueByName("parameter" + i) + ", expected: " + "value" + i);
                }
            }
            if (!success)
                Console.WriteLine("LIB TEST FAIL: parameters generation fail");
            return success;
        }

        private bool fileTestParse()
        {
            Console.WriteLine("Config lexem parser with file loading starting...");
            configFilePath = "test.cnf";
            loadFileIntoMemory();
            parseFile();
            readyForParse();
            List<String> sample = new List<String> {"[section1]","parameter1","=","value1","parameter2","=","value2",
                                        "parameter3","=","value3","[section2]","parameter4","=","value4"};
            List<String> result = getLexemsList();
            if (sample.Count != result.Count)
            {
                Console.WriteLine("FILE PARSE: COUNT FAIL " + sample.Count + "!=" + result.Count);
                return false;
            }
            bool success = true;
            for (int i = 0; i < sample.Count; i++)
            {
                if (sample[i] != result[i])
                {
                    Console.WriteLine("FILE PARSE: VALUE FAIL " + sample[i] + "!=" + result[i]);
                    success = false;
                }
            }
            return success;
        }
    }

    public class WrongLexemException:Exception
    {
        public WrongLexemException()
        {

        }

        public WrongLexemException(String message)
            : base(message)
        {

        }

        public WrongLexemException(String message, Exception inner)
            : base(message, inner)
        {

        }
    }

    public class MissingLexemException:Exception
    {
        public MissingLexemException()
        {

        }

        public MissingLexemException(String message)
            :base(message)
        {

        }
    }

    public abstract class configParser
    {
        protected String configFilePath;
        protected String configFileContent;
        protected List<String> requireFileExtensions;
        protected configProperty headProperty;
        protected configParser(String configFilePath)
        {
            this.configFilePath = configFilePath;
            headProperty = configProperty.generatePropertyWithoutParent("head", "head");
            requireFileExtensions = new List<string>();
        }
        public void changeFilePath(String newFilePath)
        {
            this.configFilePath = newFilePath;
        }

        public void loadFileIntoMemory()
        {
            StreamReader configFileReader;
            configFileReader = new StreamReader(configFilePath);
            configFileContent = configFileReader.ReadToEnd();
        }
        public abstract void parseFile();
        public abstract Boolean checkFileExtensionForCompability();
        public abstract String getStringPropetryValueByName(String propertyName);
        public abstract Decimal getDecimalPropertyByName(String propertyName);
        public abstract void setStringPropertyValueByName(String propertyName, String propertyValue);
        public abstract void setDecimalPropertyValueByName(String propertyName, Decimal propertyValue);
        public abstract void saveChangedFile();
        public abstract void saveChangesToNewFile(String newFilePath);

    }

    public class configProperty
    {
        List<configProperty> childProperties;
        String name;
        String value;
        configProperty parentProperty;

        private configProperty(String name, String value):this(name,value,null)
        {}

        private configProperty(String name, String value, configProperty parent)
        {
            this.name = name;
            this.value = value;
            this.parentProperty = parent;
            this.childProperties = new List<configProperty>();
        }

        public static configProperty generatePropertyWithParent(String name, String value,configProperty parent)
        {
            return new configProperty(name, value, parent);
        }

        public static configProperty generatePropertyWithoutParent(String name, String value)
        {
            return new configProperty(name, value);
        }

        public void addNewChildProperty(String name, String value)
        {
            childProperties.Add(configProperty.generatePropertyWithParent(name, value, this));
        }

        public void insertExistingChildProperty(configProperty child)
        {
            child.setParent(this);
            childProperties.Add(child);
        }

        public void setParent(configProperty newParent)
        {
            this.parentProperty = newParent;
        }

        public String getValueFromBranchByName(String name)
        {
            configProperty findProperty = getPropertyFromBranchByName(name);
            if (findProperty != null)
                return findProperty.getValue();
            else
                return Constants.EmptySearchResultFromBranch;
        }

        public configProperty getPropertyFromBranchByName(String name)
        {
            //usual Depth-first search
            if (isNameEquals(name))
                return this;
            else
            {
                String findValue = Constants.EmptySearchResultFromBranch;
                foreach (configProperty currentProperty in childProperties)
                    if (currentProperty.getValueFromBranchByName(name) != Constants.EmptySearchResultFromBranch)
                        return currentProperty.getPropertyFromBranchByName(name);
                return null;
            }
        }

        public bool isNameEquals(String otherName)
        {
            return this.name == otherName;
        }

        public String getValue()
        {
            return value;
        }

        public List<configProperty> getChildProperties()
        {
            return childProperties;
        }

        public configProperty getFirstChildPropertyByName(String name)
        {
            return childProperties.First(s => s.isNameEquals(name));
        }

        public bool isHead()
        {
            return name + " = " +value == "head = head";
        }
    }
}
