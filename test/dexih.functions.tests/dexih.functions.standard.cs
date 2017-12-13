﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Dexih.Utils.DataType.DataType;

namespace dexih.functions.tests
{
    public class FunctionStandardFunctions
    {
        [Theory]
        [InlineData(ETypeCode.Boolean, "abc", false)]
        [InlineData(ETypeCode.Boolean, "true", true)]
        [InlineData(ETypeCode.Int32, "abc", false)]
        [InlineData(ETypeCode.Int32, "123", true)]
        [InlineData(ETypeCode.DateTime, "32 january 2015", false)]
        [InlineData(ETypeCode.DateTime, "31 january 2015", true)]
        public void Parameter_Tests(ETypeCode dataType, object value, bool expectedSuccess)
        {
            //test boolean
            dexih.functions.Parameter parameter = new dexih.functions.Parameter("test", dataType);
            try
            {
                parameter.SetValue(value);
                Assert.True(expectedSuccess);
            }
            catch (Exception)
            {
                Assert.False(expectedSuccess);
            }
        }


        [Theory]
        [InlineData("Concat", new object[] { "test1", "test2", "test3" }, "test1test2test3")]
        [InlineData("IndexOf", new object[] { "test string", "s" }, 2)]
        [InlineData("Insert", new object[] { "test string", 5, "new " }, "test new string")]
        [InlineData("Join", new object[] { ",", "test1", "test2", "test3" }, "test1,test2,test3")]
        [InlineData("PadLeft", new object[] { "test", 7, "-" }, "---test")]
        [InlineData("PadRight", new object[] { "test", 7, "-" }, "test---")]
        [InlineData("Remove", new object[] { "testing", 1, 4 }, "tng")]
        [InlineData("Replace", new object[] { "stress test", "es", "aa" }, "straas taat")]
        [InlineData("Split", new object[] { "test1,test2,test3", ",", 3 }, 3)]
        [InlineData("Substring", new object[] { "testing", 1, 4 }, "esti")]
        [InlineData("ToLower", new object[] { "  tEsT1 " }, "  test1 ")]
        [InlineData("ToUpper", new object[] { "  tEsT1 " }, "  TEST1 ")]
        [InlineData("Trim", new object[] { "  tEsT1 " }, "tEsT1")]
        [InlineData("TrimEnd", new object[] { "  tEsT1 " }, "  tEsT1")]
        [InlineData("TrimStart", new object[] { "  tEsT1 " }, "tEsT1 ")]
        [InlineData("Length", new object[] { "test" }, 4)]
        [InlineData("WordCount", new object[] { "word1  word2 word3" }, 3)]
        [InlineData("WordExtract", new object[] { "word1  word2 word3", 2 }, "word3")]
        [InlineData("LessThan", new object[] { 1, 2 }, true)]
        [InlineData("LessThan", new object[] { 2, 1 }, false)]
        [InlineData("LessThanEqual", new object[] { 1, 2 }, true)]
        [InlineData("LessThanEqual", new object[] { 2, 2 }, true)]
        [InlineData("LessThanEqual", new object[] { 2, 1 }, false)]
        [InlineData("GreaterThan", new object[] { 2, 1 }, true)]
        [InlineData("GreaterThanEqual", new object[] { 2, 1 }, true)]
        [InlineData("IsEqual", new object[] { 2, 2 }, true)]
        [InlineData("IsBooleanEqual", new object[] { true, true }, true)]
        [InlineData("IsEqual", new object[] { 3, 2 }, false)]
        [InlineData("IsNumber", new object[] { "123" }, true)]
        [InlineData("IsNumber", new object[] { "123a" }, false)]
        [InlineData("IsNull", new object[] { null }, true)]
        [InlineData("IsBetween", new object[] { 2, 1, 3 }, true)]
        [InlineData("IsBetweenInclusive", new object[] { 2, 1, 3 }, true)]
        [InlineData("RegexMatch", new object[] { "abbbb", "ab*" }, true)]
        [InlineData("Contains", new object[] { "testing", "est" }, true)]
        [InlineData("EndsWith", new object[] { "testing", "ing" }, true)]
        [InlineData("StartsWith", new object[] { "testing", "tes" }, true)]
        [InlineData("IsUpper", new object[] { "TEST", true }, true)]
        [InlineData("IsLower", new object[] { "test", true }, true)]
        [InlineData("IsAlpha", new object[] { "test" }, true)]
        [InlineData("IsAlphaNumeric", new object[] { "test123" }, true)]
        [InlineData("IsPattern", new object[] { "Hello12", "Aaaaa99" }, true)]
        [InlineData("DaysInMonth", new object[] { "2015-09-24" }, 30)]
//        [InlineData("IsDaylightSavingTime", new object[] { "2015-09-24T21:22:48.2698750Z" }, false)]
        [InlineData("IsLeapYear", new object[] { "2015-09-24" }, false)]
        [InlineData("IsWeekend", new object[] { "2015-09-24" }, false)]
        [InlineData("IsWeekDay", new object[] { "2015-09-24" }, true)]
        [InlineData("DayOfMonth", new object[] { "2015-09-24" }, 24)]
        [InlineData("DayOfWeekName", new object[] { "2015-09-24" }, "Thursday")]
        [InlineData("DayOfWeekNumber", new object[] { "2015-09-24" }, 4)]
        [InlineData("WeekOfYear", new object[] { "2015-09-24" }, 39)]
        [InlineData("DayOfYear", new object[] { "2015-09-24" }, 267)]
        [InlineData("Month", new object[] { "2015-09-24" }, 9)]
        [InlineData("ShortMonth", new object[] { "2015-09-24" }, "Sep")]
        [InlineData("LongMonth", new object[] { "2015-09-24" }, "September")]
        [InlineData("Year", new object[] { "2015-09-24" }, 2015)]
        [InlineData("ToLongDateString", new object[] { "2015-09-24" }, "Thursday, 24 September 2015")]
        [InlineData("ToLongTimeString", new object[] { "2015-09-24" }, "12:00:00 AM")]
        [InlineData("ToShortDateString", new object[] { "2015-09-24" }, "24/09/2015")]
        [InlineData("ToShortTimeString", new object[] { "2015-09-24" }, "12:00 AM")]
        [InlineData("DateToString", new object[] { "2015-09-24", "dd MMM yyyy" }, "24 Sep 2015")]
        [InlineData("Abs", new object[] { -3 }, (double)3)]
        [InlineData("DivRem", new object[] { 6, 4 }, 1)]
        [InlineData("Pow", new object[] { 6, 2 }, (double)36)]
        [InlineData("Round", new object[] { 6.5 }, (double)6)]
        [InlineData("Sign", new object[] { -4 }, (double)-1)]
        [InlineData("Sqrt", new object[] { 9 }, (double)3)]
        [InlineData("Truncate", new object[] { 6.4 }, (double)6)]
        [InlineData("IsIn", new object[] { "test2", "test1", "test2", "test3" }, true)]
        [InlineData("GetDistanceTo", new object[] { -38, -145, -34 ,- 151 }, 699082.1288)] //melbourne to sydney distance
        [InlineData("MaxLength", new object[] { "abcdef", 5 }, false)] 
        [MemberData(nameof(OtherFunctions))]
        public void StandardFunctionTest(string functionName, object[] parameters, object expectedResult)
        {
            var function = StandardFunctions.GetFunctionReference(functionName);
            function.OnNull = EErrorAction.Execute;
            var returnValue = function.RunFunction(parameters);

            if (returnValue.GetType() == typeof(double))
            {
                Assert.Equal(expectedResult, Math.Round((double)returnValue, 4));
            }
            else
                Assert.Equal(expectedResult, returnValue);
        }

        public static IEnumerable<object[]> OtherFunctions
        {
            get
            {
                return new[]
                {
                new object[] { "AddDays", new object[] { "2015-09-24", 1 }, DateTime.Parse("25 Sep 2015")},
                new object[] { "AddHours", new object[] { "2015-09-24", 24 }, DateTime.Parse("25 Sep 2015")},
                new object[] { "AddMilliseconds", new object[] { "2015-09-24", 86400000 }, DateTime.Parse("25 Sep 2015")},
                new object[] { "AddMinutes", new object[] { "2015-09-24", 1440 }, DateTime.Parse("25 Sep 2015")},
                new object[] { "AddMonths", new object[] { "2015-09-24", 1 }, DateTime.Parse("24 Oct 2015")},
                new object[] { "AddSeconds", new object[] { "2015-09-24", 86400 }, DateTime.Parse("25 Sep 2015")},
                new object[] { "AddYears", new object[] { "2015-09-24", 1 }, DateTime.Parse("24 Sep 2016")},
                new object[] { "Add", new object[] { 1, 2 }, (Decimal) 3},
                new object[] { "Ceiling", new object[] { 6.4 }, (Decimal)7 },
                new object[] { "Divide", new object[] { 6, 2 }, (Decimal)3 },
                new object[] { "Floor", new object[] { 6.4 }, (Decimal)6 },
                new object[] { "Multiply", new object[] { 6, 2 }, (Decimal)12 },
                new object[] { "Negate", new object[] { 6 }, (Decimal) (-6)},
                new object[] { "Remainder", new object[] { 6, 4 }, (Decimal)2 },
                new object[] { "Subtract", new object[] { 6, 2 }, (Decimal)4 },
                new object[] { "IsDateTimeEqual", new object[] { DateTime.Parse("25 Sep 2015"), DateTime.Parse("25 Sep 2015") }, true }
            };
            }
        }

        [Theory]
        [InlineData("Sum", (double)110)]
        [InlineData("Average", 5.5)]
        [InlineData("Median", 5.5)]
        [InlineData("StdDev", 2.8723)]
        [InlineData("Variance", 8.25)]
        [InlineData("Min", (double)1)]
        [InlineData("Max", (double)10)]
        [InlineData("First", "1")]
        [InlineData("Last", "10")]
        [InlineData("CountDistinct", 10)]
        public void AggregateFunctionTest(string functionName, object expectedResult)
        {
            var function = StandardFunctions.GetFunctionReference(functionName);

            for (int a = 0; a < 2; a++) // run all tests twice to ensure reset functions are working
            {
                for (int i = 1; i <= 10; i++)
                {
                    var functionResult = function.RunFunction(new object[] { i });
                    functionResult = function.RunFunction(new object[] { i });
                }

                var aggregateResult = function.ReturnValue();
                Assert.NotNull(aggregateResult);

                if(aggregateResult.GetType() == typeof(double))
                {
                    Assert.Equal(expectedResult, Math.Round((double)aggregateResult, 4));
                }
                else
                    Assert.Equal(expectedResult, aggregateResult);

                function.Reset();
            }
        }

        [Theory]
        [InlineData("FirstWhen", 1, "1")]
        [InlineData("LastWhen", 1, "9")]
        public void AggregateWhenFunctionTest(string functionName, object test, object expectedResult)
        {
            var function = StandardFunctions.GetFunctionReference(functionName);

            for (int a = 0; a < 2; a++) // run all tests twice to ensure reset functions are working
            {
                for (int i = 1; i <= 10; i++)
                {
                    int value = i % 2;

                    var functionResult = function.RunFunction(new object[] { test, value, i });
                }

                var aggregateResult = function.ReturnValue();
                Assert.NotNull(aggregateResult);

                if (aggregateResult.GetType() == typeof(double))
                {
                    Assert.Equal(expectedResult, Math.Round((double)aggregateResult, 4));
                }
                else
                    Assert.Equal(expectedResult, aggregateResult);

                function.Reset();
            }
        }

        [Fact]
        public void MinMaxDateTest()
        {
            var minFunction = StandardFunctions.GetFunctionReference("MinDate");
            var maxFunction = StandardFunctions.GetFunctionReference("MaxDate");

            DateTime baseDate = DateTime.Now;

            for (int a = 0; a < 2; a++) // run all tests twice to ensure reset functions are working
            {
                for (int i = 0; i <= 10; i++)
                {
                    var minFunctionResult = minFunction.RunFunction(new object[] { baseDate.AddDays(i) });
                    var maxFunctionResult = maxFunction.RunFunction(new object[] { baseDate.AddDays(i) });
                }

                var minResult = minFunction.ReturnValue();
                var maxResult = maxFunction.ReturnValue();
                Assert.NotNull(minResult);
                Assert.NotNull(maxResult);
                Assert.Equal(baseDate, (DateTime)minResult);
                Assert.Equal(baseDate.AddDays(10), (DateTime)maxResult);

                minFunction.Reset();
                maxFunction.Reset();
            }
        }

        [Fact]
        public void CountTest()
        {
            var function = StandardFunctions.GetFunctionReference("Count");

            for (int a = 0; a < 2; a++) // run all tests twice to ensure reset functions are working
            {
                for (int i = 1; i <= 10; i++)
                {
                    var functionResult = function.RunFunction(new object[] { });
                }

                var aggregateResult = function.ReturnValue();
                Assert.NotNull(aggregateResult);
                Assert.Equal(10, aggregateResult);

                function.Reset();
            }
        }

        [Fact]
        public void ConcatAggTest()
        {
            var function = StandardFunctions.GetFunctionReference("ConcatAgg");

            for (int a = 0; a < 2; a++) // run all tests twice to ensure reset functions are working
            {
                for (int i = 1; i <= 10; i++)
                {
                    var functionResult = function.RunFunction(new object[] { i, "," });
                }

                var aggregateResult = function.ReturnValue();
                Assert.NotNull(aggregateResult);
                Assert.Equal("1,2,3,4,5,6,7,8,9,10", aggregateResult);

                function.Reset();
            }

        }

        [Fact]
        public void Function_XPathValue()
        {
            //Get a rows that exists.
            Function XPathValue = StandardFunctions.GetFunctionReference("XPathValues");
            object[] Param = new object[] { "<root><row>0</row><row>1</row><row>2</row><row>3</row><row>4</row><row>5</row></root>", "//row[1]", "//row[2]", "//row[3]" };
            Assert.True((bool)XPathValue.RunFunction(Param, new string[] { "value1", "value2", "value3" }));
            Assert.Equal("0", (string)XPathValue.Outputs[0].Value);
            Assert.Equal("1", (string)XPathValue.Outputs[1].Value);
            Assert.Equal("2", (string)XPathValue.Outputs[2].Value);
        }

        [Fact]
        public void Function_JSONValue()
        {
            //Get a rows that exists.
            Function JSONValues = StandardFunctions.GetFunctionReference("JsonValues");
            object[] Param = new object[] { "{ 'value1': '1', 'value2' : '2', 'value3': '3', 'array' : {'v1' : '1', 'v2' : '2'} }", "value1", "value2", "value3", "array", "badvalue" };
            Assert.False((bool)JSONValues.RunFunction(Param, new string[] { "value1", "value2", "value3", "array", "badvalue" }));
            Assert.Equal("1", (string)JSONValues.Outputs[0].Value);
            Assert.Equal("2", (string)JSONValues.Outputs[1].Value);
            Assert.Equal("3", (string)JSONValues.Outputs[2].Value);
            Assert.Null((string)JSONValues.Outputs[4].Value);

            //get the sub Json string, and run another parse over this.
            string MoreValues = (string)JSONValues.Outputs[3].Value;
            Param = new object[] { MoreValues, "v1", "v2" };
            JSONValues = StandardFunctions.GetFunctionReference("JsonValues");
            Assert.True((bool)JSONValues.RunFunction(Param, new string[] { "v1", "v2" }));
            Assert.Equal("1", (string)JSONValues.Outputs[0].Value);
            Assert.Equal("2", (string)JSONValues.Outputs[1].Value);

        }


        [Fact]
        public void RowFunctions_GenerateSequence()
        {
            //Use a for loop to similate gen sequence.
            Function Sequence = StandardFunctions.GetFunctionReference("GenerateSequence");
            object[] Param = new object[] { 0, 10, 2 };
            for (int i = 0; i <= 10; i += 2)
            {
                Assert.True((bool)Sequence.RunFunction(Param));
                Assert.Equal(i, (int)Sequence.Outputs[0].Value);
            }
            //last value should be false as the sequence has been exceeded.
            Assert.False((bool)Sequence.RunFunction(Param));
        }

        [Fact]
        public void RowFunctions_SplitColumnToRows()
        {
            //Use a for loop to similate gen sequence.
            Function SplitColumnToRows = StandardFunctions.GetFunctionReference("SplitColumnToRows");
            object[] Param = new object[] { "|", "|value2|value3||value5||", 6 };
            string[] Compare = new string[] { "", "value2", "value3", "", "value5", "", "" };
            for (int i = 0; i <= 6; i++)
            {
                Assert.True((bool)SplitColumnToRows.RunFunction(Param));
                Assert.Equal(Compare[i], (string)SplitColumnToRows.Outputs[0].Value);
            }

            //last value should be false as the sequence has been exceeded.
            Assert.False((bool)SplitColumnToRows.RunFunction(Param));
        }

        [Fact]
        public void RowFunctions_XPathNodesToRows()
        {
            //Use a for loop to similate gen sequence.
            Function XPathNodesToRows = StandardFunctions.GetFunctionReference("XPathNodesToRows");
            object[] Param = new object[] { "<root><row>0</row><row>1</row><row>2</row><row>3</row><row>4</row><row>5</row></root>", "//row", 5 };
            for (int i = 0; i <= 5; i++)
            {
                Assert.True((bool)XPathNodesToRows.RunFunction(Param));
                Assert.Equal(i.ToString(), (string)XPathNodesToRows.Outputs[0].Value);
            }

            //last value should be false as the sequence has been exceeded.
            Assert.False((bool)XPathNodesToRows.RunFunction(Param));
        }

        [Fact]
        public void RowFunctions_JsonElementsToRows()
        {
            //Use a for loop to similate gen sequence.
            Function JSONElementsToRows = StandardFunctions.GetFunctionReference("JsonElementsToRows");
            object[] Param = new object[] { "{'results' : [{'value1' : 'r1v1', 'value2' : 'r1v2'}, {'value1' : 'r2v1', 'value2' : 'r2v2'}]} ", "results[*]", 2 };
            for (int i = 1; i <= 2; i++)
            {
                Assert.True((bool)JSONElementsToRows.RunFunction(Param));
                string JsonResult = (string)JSONElementsToRows.Outputs[0].Value;
                var results = JObject.Parse(JsonResult);
                Assert.Equal("r" + i.ToString() + "v1", results.SelectToken("value1").ToString());
                Assert.Equal("r" + i.ToString() + "v2", results.SelectToken("value2").ToString());
            }

            //last value should be false as the sequence has been exceeded.
            Assert.False((bool)JSONElementsToRows.RunFunction(Param));
        }
    }
}
