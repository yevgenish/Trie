using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Trie01
{
    public static class Helper
    {
               
        #region Sizes

        private const bool IS_TEST = false;

        //PROD
        private const int ARR_SIZE_PROD = 256;
        //TEST
        private const int ARR_SIZE_TEST = 26;

        public const long MAX_LENGTH_SIZE = 32 * 1024;//32 kilobyte// 100;// 1000;


        public static class Sizes
        {
            public static int ARR_SIZE
            {
                get
                {
                    if (IS_TEST)
                    {
                        return ARR_SIZE_TEST;
                    }
                    else
                    {
                        return ARR_SIZE_PROD;
                    }
                }
            }

            

            public static int size_of_Node_element
            {
                get
                {
                    return sizeof(bool) + sizeof(long) //long? Value
                        + sizeof(int) // int NumOfPopulatedNodes
                        + IntPtr.Size // Node Parent Reference
                        + IntPtr.Size //Node Self Reference
                        + IntPtr.Size //Node Self Reference Node element
                        + sizeof(bool) + sizeof(byte); //byte? ParentIndex
                                                       //+ IntPtr.Size; //pointer to self - not required, already counted in size_of_Node_Array

                }
            }


            public static int size_of_RootNode_element
            {
                get
                {
                    return size_of_Node_element//size of regular node element
                        + sizeof(int) //int _rootAmountOfSubArrays
                        + sizeof(int) //int RootLengthOfKeyBytes
                        + sizeof(int) //int RootSubArraysAmountOfPopulatedNodes;
                        + size_of_Node_Array
                        + IntPtr.Size; //pointer to self
                }
            }

            public static int size_of_Node_Array
            {
                get
                {
                    return (IntPtr.Size * ARR_SIZE) //Node[] Arr elements
                        + IntPtr.Size; //pointer to self
                }
            }
        }

        #endregion


        #region LogHelper

        public static bool WriteToLogAndCollectSearchData = false;

        public static bool WriteToLogAddRemoveStatistics = false;

        public static bool WriteTestLogData = false;

        public static bool WriteDuplicatesInfo = false;

        public static bool WriteDuplicatesLogData = false;

        public static bool CheckParentChildRefNotForTests = false;

        public static void WriteToLog(string text, bool toWrite)
        {
            if (toWrite)
            {
                Debug.WriteLine(text);
            }
        }

        #endregion


        #region Serialization

        public enum SerializationFormat
        {
            xml = 1
        }

        public const SerializationFormat serializationFormat = SerializationFormat.xml;

        public static string SerializeObject<T>(this T toSerialize)
        {
            string result = null;

            if (serializationFormat == SerializationFormat.xml)
            {
                result = SerializeObject_Xml(toSerialize);
            }
            return result;
        }

        public static T DeserializeObject<T>(string strData) where T : class
        {
            T result = null;
            if (serializationFormat == SerializationFormat.xml)
            {
                result = DeserializeObject_Xml<T>(strData);
            }
            return result;
        }

        private static string SerializeObject_Xml<T>(this T toSerialize)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.ToString(), true);
            }
            return null;
        }

        private static T DeserializeObject_Xml<T>(string strData) where T : class
        {
            try
            {
                T result;
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(strData))
                {
                    result = (T)serializer.Deserialize(reader);
                }
                return result;
            }

            catch (Exception ex)
            {
                WriteToLog(ex.ToString(), true);
            }
            return default(T);
        }

        #endregion


        #region StringToBytesConverter

        public static byte[] ConvertStringToBytes(string text)
        {
            if (IS_TEST)
            {
                return ConvertStringToBytes_Test(text);
            }
            else
            {
                return ConvertStringToBytes_Prod(text);
            }
        }

        private static byte[] ConvertStringToBytes_Test(string text)
        {
            int text_Length = text.Length;
            byte[] encoded = new byte[text_Length];
            for (int i = 0; i < text_Length; i++)
            {
                encoded[i] = (byte)(text[i] - 'a');
            }
            return encoded;
        }

        public static char ConvertByteToChar(byte number)
        {
            byte a_padding = (IS_TEST ? (byte)'a' : 0);

            var result = Convert.ToChar(a_padding + number);
            return result;
        }

        private static byte[] ConvertStringToBytes_Prod(string text)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(text);
            return encoded;
        }

        public static string ConvertBytesToString(byte[] encoded)
        {
            string text = Encoding.UTF8.GetString(encoded);
            return text;
        }

        #endregion


        #region SubArrayHelper

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            if (result.Length == 0)
            {
                result = null;
            }
            return result;
        }

        public static T[] SubArray<T>(this T[] data, int index)
        {
            int length = data.Length - index;
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            if (result.Length == 0)
            {
                result = null;
            }
            return result;
        }

        public static void Shuffle<T>(this T[] array)
        {
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n);
                n--;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        #endregion

    }
}
