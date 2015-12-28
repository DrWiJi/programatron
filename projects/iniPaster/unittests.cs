using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using iniPaster;

namespace testingDLL
{
    public class iniParserTester
    {
        configProperty mainTestProperty;
        List<String[]> configPropertyTestValues;
        customConfigFormatParser confParser;

        public iniParserTester()
        {
            initializeAllTests();
        }

        private void initializeAllTests()
        {
            mainTestProperty = configProperty.generatePropertyWithoutParent("main", "main");
            mainTestProperty.addNewChildProperty("first branch", "left");
            mainTestProperty.addNewChildProperty("second branch", "right");
            mainTestProperty.getFirstChildPropertyByName("first branch").addNewChildProperty("first branch first","left left");
            mainTestProperty.getFirstChildPropertyByName("first branch").addNewChildProperty("first branch second", "left right");
            mainTestProperty.getFirstChildPropertyByName("second branch").addNewChildProperty("second branch first", "right left");
            mainTestProperty.getFirstChildPropertyByName("second branch").addNewChildProperty("second branch second", "right right");
            configPropertyTestValues = new List<String[]>{
                new String[]{"second branch first", "right left"},
                new String[]{"second branch", "right"},
                new String[]{"main","main"},
                new String[]{"first branch first","left left"},
                new String[]{"sdgsdgsdggds",null}

            };
            confParser = new customConfigFormatParser("");
        }

        public void runAllTests()
        {
            Console.WriteLine("Begin iniParser lib tests...");
            bool success = true;
            if(!runConfigPropertiesTests())
            {
                success = false;
            }
            if(!confParser.unitTests())
            {
                success = false;
            }
            if (success)
                Console.WriteLine("ALL TESTS ARE SUCCESSFUL");
            else
                Console.WriteLine("SOME TESTS ARE FAIL");
        }

        private bool runConfigPropertiesTests()
        {
            Console.WriteLine("Begin property class tests...");
            bool success = true;
            foreach(String[] currentTest in configPropertyTestValues)
            {
                String resultValue = mainTestProperty.getValueFromBranchByName(currentTest[0]);
                Console.WriteLine(resultValue);
                if(resultValue==currentTest[1])
                {
                    Console.WriteLine("TRUE");
                }
                else
                {
                    Console.WriteLine("FALSE, EXPECT " + currentTest[1]);
                    success = false;
                }
               
            } 
            return success;
        }
    }
}
