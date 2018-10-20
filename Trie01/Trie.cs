using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trie01
{
    [Serializable]
    public partial class Trie : ITrie
    {
        #region Members

        public RootNode root;

        #endregion

        #region Constructor

        public Trie()
        {
            root = new RootNode(null);
            NodeReference rootSelfReference = new NodeReference(root);
            root.InitSelfReference(rootSelfReference);
            
            root.InitArr();

            root.StatsRootSizeForEmpty = root.RootCurrentSize;
        }

        #endregion

        #region InterfaceFunctions

        public bool TryRead(string key, out long value)
        {
            value = -1;
            int nodes_occupied;
            Node[] nodes = null;
            int foundKeyPartSize;
            int foundKeyPartSizeWithoutLastNodeKey;
            bool key_found = SearchNode(key, out nodes, out nodes_occupied, out foundKeyPartSize, out foundKeyPartSizeWithoutLastNodeKey);
            if (key_found)
            {
                value = (long)nodes[nodes_occupied - 1].Value;
            }
            return key_found;
        }

        public bool TryWrite(string key, long value)
        {
            if (value == 111)
            {
                Helper.WriteToLog("root.RootCurrentSize: " + root.RootCurrentSize, Helper.WriteToLogAddRemoveStatistics);
                Helper.WriteToLog("-------------------------------", Helper.WriteToLogAddRemoveStatistics);               
            }

            Helper.WriteToLog("ADD: " + key, Helper.WriteToLogAddRemoveStatistics);

            bool canWrite = CanWrite(key);

            if (!canWrite)
            {
                return false;
            }

            Node p = root;
            byte[] keyBytes = Helper.ConvertStringToBytes(key);
            int keyBytes_Length = keyBytes.Length;

            InsertKeyBytesToNode(p, keyBytes, value);

            Helper.WriteToLog("root.RootCurrentSize: " + root.RootCurrentSize, Helper.WriteToLogAddRemoveStatistics);
            Helper.WriteToLog("root.RootLengthOfKeyBytes: " + root.RootLengthOfKeyBytes, Helper.WriteToLogAddRemoveStatistics);            
            Helper.WriteToLog("-------------------------------", Helper.WriteToLogAddRemoveStatistics);

            CheckParentNodeRefferenceInChildrenFromRoot("ADD middle, key = " + key, true, Helper.CheckParentChildRefNotForTests);

            return true;
        }


        public void Delete(string key)
        {
            Node[] nodes = null;
            int nodes_occupied = 0;
            bool key_found = false;
            int foundKeyPartSize;
            int foundKeyPartSizeWithoutLastNodeKey;
            key_found = SearchNode(key, out nodes, out nodes_occupied, out foundKeyPartSize, out foundKeyPartSizeWithoutLastNodeKey);

            if (key_found)
            {
                Helper.WriteToLog("DELETE: " + key, Helper.WriteToLogAddRemoveStatistics);
                Node current = nodes[nodes_occupied - 1];
                current.Value = null;

                if (current.Arr == null && current.NumOfPopulatedNodes == 0 && current.Value == null)
                {
                    Node parent = current.ParentReference.NodeElement;

                    root.RootLengthOfKeyBytes -= current.GetKeyLength;
                    this.DeleteNode(current);

                    current = null;
                    if (parent.NumOfPopulatedNodes == 0)
                    {
                        if (parent != root)
                        {
                            parent.Arr = null;
                            root.RootAmountOfSubArrays--;
                        }
                    }
                    else if (parent != root && parent.NumOfPopulatedNodes == 1 && parent.Value == null)
                    {
                        //tzimzum
                        for (int j = 0; j < Helper.Sizes.ARR_SIZE; j++)
                        {
                            Node child = parent.Arr[j];
                            if (child != null)
                            {
                                byte[] newParentKey;
                                int parentKeyLength = parent.GetKeyLength;
                                int childSelfLength = 1;
                                int childKeyLength = child.GetKeyLength;
                                int newParentKeyLength = parentKeyLength + childSelfLength + childKeyLength;
                                newParentKey = new byte[newParentKeyLength];

                                int copyStartIndex = 0;
                                if (parentKeyLength > 0)
                                {
                                    Array.Copy(parent.Key, 0, newParentKey, 0, parentKeyLength);
                                    copyStartIndex = parentKeyLength + 0;
                                }

                                newParentKey[copyStartIndex] = (byte)j;
                                copyStartIndex++;

                                if (childKeyLength > 0)
                                {
                                    Array.Copy(child.Key, 0, newParentKey, copyStartIndex, childKeyLength);
                                }

                                root.RootLengthOfKeyBytes -= parent.GetKeyLength;
                                parent.Key = newParentKey;
                                root.RootLengthOfKeyBytes += parent.GetKeyLength;

                                parent.Value = child.Value;
                                parent.Arr = child.Arr;
                                parent.NumOfPopulatedNodes = child.NumOfPopulatedNodes;

                                child.MoveNodeArrChildrenToOtherNode(parent, false);
                                child.SelfReference = null;

                                root.RootAmountOfSubArrays--;
                                root.RootLengthOfKeyBytes -= child.GetKeyLength;
                                child = null;
                                root.RootGlobalAmountOfPopulatedNodes--;

                                break;
                            }
                        }
                    }

                }
                else
                {
                    //do nothing, there are dependent things
                }

                if (current != null)
                {
                    if (current.Value == null && current.Arr != null && current.NumOfPopulatedNodes == 1)
                    {
                        //letzamzem self
                        //tzimzum

                        for (int j = 0; j < Helper.Sizes.ARR_SIZE; j++)
                        {
                            Node child = current.Arr[j];
                            if (child != null)
                            {
                                byte[] newCurrentKey;
                                int currentKeyLength = current.GetKeyLength;
                                int childSelfLength = 1;
                                int childKeyLength = child.GetKeyLength;
                                int newCurrentKey_Length = currentKeyLength + childSelfLength + childKeyLength;
                                newCurrentKey = new byte[newCurrentKey_Length];

                                int copyStartIndex = 0;
                                if (currentKeyLength > 0)
                                {
                                    Array.Copy(current.Key, 0, newCurrentKey, 0, currentKeyLength);
                                    copyStartIndex = currentKeyLength + 0;
                                }

                                newCurrentKey[copyStartIndex] = (byte)j;
                                copyStartIndex++;

                                if (childKeyLength > 0)
                                {
                                    Array.Copy(child.Key, 0, newCurrentKey, copyStartIndex, childKeyLength);
                                }

                                root.RootLengthOfKeyBytes -= currentKeyLength;
                                root.RootLengthOfKeyBytes -= childKeyLength;
                                root.RootLengthOfKeyBytes += newCurrentKey_Length;

                                root.RootAmountOfSubArrays--;

                                child.Key = newCurrentKey;
                                child.ParentReference = current.ParentReference;

                                child.ParentIndex = current.ParentIndex;
                                current.ChangeSelfTo(child);
                                current.ParentReference = null;
                                current = null;

                                this.root.RootGlobalAmountOfPopulatedNodes--;

                                //tzimzum: change references - no need
                                break;
                            }
                        }

                    }
                }

                Helper.WriteToLog("root.RootCurrentSize: " + root.RootCurrentSize, Helper.WriteToLogAddRemoveStatistics);
                Helper.WriteToLog("root.RootLengthOfKeyBytes: " + root.RootLengthOfKeyBytes, Helper.WriteToLogAddRemoveStatistics);
                Helper.WriteToLog("-------------------------------", Helper.WriteToLogAddRemoveStatistics);

                CheckParentNodeRefferenceInChildrenFromRoot("REMOVE middle, key = " + key, true, Helper.CheckParentChildRefNotForTests);
            }
        }

        public void Save(string fileName)
        {
            try
            {
                string serialized = Helper.SerializeObject<Trie>(this);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.WriteAllText(fileName, serialized, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Helper.WriteToLog(ex.ToString(), true);
            }
        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                string strData = File.ReadAllText(filename, Encoding.UTF8);
                var objData = Helper.DeserializeObject<Trie>(strData);

                this.root = objData.root;

                SetParentAndSelfNodeRefferences(this.root);
            }
        }

        #endregion

        #region HelpFunctions

        private void InsertKeyBytesToNode(Node p, byte[] keyBytes, long value)
        {
            int keyBytes_Length = (keyBytes != null ? keyBytes.Length : 0);

            if (p == root)
            {
                byte keyBytes_p_Index = keyBytes[0];
                if (p.Arr[keyBytes_p_Index] == null)
                {
                    Node childNode = CreateNewNode(p.SelfReference, keyBytes_p_Index);
                    if (keyBytes_Length > 1)
                    {
                        childNode.Key = keyBytes.SubArray(1);
                        root.RootLengthOfKeyBytes += childNode.GetKeyLength;
                    }
                    childNode.Value = value;

                    p.NumOfPopulatedNodes++;

                    p.Arr[keyBytes_p_Index] = childNode;
                }
                else
                {
                    InsertKeyBytesToNode(p.Arr[keyBytes_p_Index], keyBytes.SubArray(1), value);
                }
            }
            else
            {
                if (p.Key != null)
                {
                    int p_Key_length = p.GetKeyLength;
                    if (p_Key_length > 0)
                    {
                        byte[] intersection = GetByteArrStartIntersection(p.Key, keyBytes);
                        if (intersection != null)
                        {
                            int intersection_length = intersection.Length;

                            if (p_Key_length == keyBytes_Length && keyBytes_Length == intersection_length)
                            {
                                if (p.Value == null)
                                {
                                    p.Value = value;
                                }
                                else
                                {
                                    //possible duplicate
                                    p.Value = value;

                                    //root.AutoIncrementDuplicatesTest.Increment();
                                    //Helper.WriteToLog("duplicate 01 - " + root.AutoIncrementDuplicatesTest.GetLastAdded, Helper.WriteDuplicatesInfo);
                                    //throw new Exception("check this situation if possible");
                                }                   
                            }
                            else
                            {
                                if (p_Key_length == intersection_length)
                                {
                                    if (keyBytes_Length > intersection_length)
                                    {
                                        byte keyBytes_p_Index = keyBytes[intersection_length];
                                        byte[] keyBytes_SubArr = null;

                                        if (intersection_length + 1 > p_Key_length)
                                        {
                                            keyBytes_SubArr = keyBytes.SubArray(intersection_length + 1);
                                        }

                                        if (p.Arr == null)
                                        {
                                            p.InitArr();

                                            if (p != root)
                                            {
                                                root.RootAmountOfSubArrays++;
                                            }
                                        }
                                        var tmpNodeByteVal = keyBytes_p_Index;
                                        var tmpNode = p.Arr[tmpNodeByteVal];

                                        if (tmpNode != null)
                                        {
                                            InsertKeyBytesToNode(tmpNode, keyBytes_SubArr, value);
                                        }
                                        else
                                        {
                                            Node newNode = CreateNewNode(p.SelfReference, tmpNodeByteVal);

                                            if (keyBytes_SubArr != null)
                                            {
                                                newNode.Key = keyBytes_SubArr;
                                                root.RootLengthOfKeyBytes += newNode.GetKeyLength;
                                            }

                                            newNode.Value = value;
                                            p.Arr[tmpNodeByteVal] = newNode;
                                            p.NumOfPopulatedNodes++;
                                        }
                                    }
                                    else// if(keyBytes_Length <= intersection_length)
                                    {
                                        //impossible
                                    }
                                }
                                else if (keyBytes_Length == intersection_length)
                                {
                                    if (p_Key_length > keyBytes_Length)
                                    {
                                        Node parentNode = CreateNewNode(p.ParentReference, p.ParentIndex);

                                        //no need to increment
                                        //p replaced with Parent Node
                                        root.RootGlobalAmountOfPopulatedNodes--;
        
                                        root.RootLengthOfKeyBytes -= p_Key_length;

                                        parentNode.Key = intersection;
                                        root.RootLengthOfKeyBytes += (intersection != null ? intersection.Length : 0);
                                        parentNode.Value = value;
                                        parentNode.InitArr();

                                        if (parentNode != root)
                                        {
                                            root.RootAmountOfSubArrays++;
                                        }

                                        byte[] p_subArr = p.Key.SubArray(intersection_length);

                                        byte childParentIndex = p_subArr[0];
                                        Node childNode = CreateNewNode(parentNode.SelfReference, childParentIndex);
                   
                                        if (p_subArr.Length > 1)
                                        {
                                            childNode.Key = p_subArr.SubArray(1);
                                            root.RootLengthOfKeyBytes += childNode.GetKeyLength;
                                        }
                                        childNode.Value = p.Value;
                                        childNode.Arr = p.Arr;
                                        childNode.NumOfPopulatedNodes = p.NumOfPopulatedNodes;                                                                               
                                        
                                        p.MoveNodeArrChildrenToOtherNode(childNode, false);

                                        parentNode.Arr[childParentIndex] = childNode;
                                        parentNode.NumOfPopulatedNodes++;

                                        p.ChangeSelfTo(parentNode);
                                    }
                                    else //if (p_Key_length <= keyBytes_Length)
                                    {
                                        //impossible
                                    }
                                }
                                else //if (p_Key_length > intersection_length || keyBytes_Length > intersection_length)
                                {
                                    if (p_Key_length > intersection_length && keyBytes_Length > intersection_length)
                                    {
                                        Node parentNode = CreateNewNode(p.ParentReference, p.ParentIndex);
                                        root.RootGlobalAmountOfPopulatedNodes--;
     
                                        root.RootLengthOfKeyBytes -= p.GetKeyLength;
  
                                        parentNode.Key = intersection;
                                        root.RootLengthOfKeyBytes += parentNode.GetKeyLength;
                                        parentNode.InitArr();
                                        if (parentNode != root)
                                        {
                                            root.RootAmountOfSubArrays++;
                                        }

                                        byte child_p_Key_Parent_Index = p.Key[intersection_length];
                                        Node child_p_Key = CreateNewNode(parentNode.SelfReference, child_p_Key_Parent_Index);
    
                                        if (p_Key_length >= intersection_length + 1)
                                        {
                                            child_p_Key.Key = p.Key.SubArray(intersection_length + 1);
                                            root.RootLengthOfKeyBytes += child_p_Key.GetKeyLength;
                                        }
                                        child_p_Key.Value = p.Value;
                                        child_p_Key.Arr = p.Arr;

                                        child_p_Key.NumOfPopulatedNodes = p.NumOfPopulatedNodes;                       
                                             
                                        p.MoveNodeArrChildrenToOtherNode(child_p_Key, false);
                             
                                        byte child_Key_Parent_Index = keyBytes[intersection_length];
                                        Node child_Key = CreateNewNode(parentNode.SelfReference, child_Key_Parent_Index);
                                        if (keyBytes_Length > intersection_length + 1)
                                        {
                                            child_Key.Key = keyBytes.SubArray(intersection_length + 1);
                                            root.RootLengthOfKeyBytes += child_Key.GetKeyLength;
                                        }
                                        child_Key.Value = value;

                                        parentNode.Arr[child_p_Key_Parent_Index] = child_p_Key;
                                        parentNode.Arr[child_Key_Parent_Index] = child_Key;
                                        parentNode.NumOfPopulatedNodes = 2;

                                        p.ChangeSelfTo(parentNode);
                                    }
                                    else //if (p_Key_length > intersection_length && keyBytes_Length == intersection_length
                                    // || keyBytes_Length > intersection_length && p_Key_length == intersection_length)
                                    {
                                        //watched before
                                    }
                                }
                            }
                        }
                        else //if (intersection == null)
                        {
                            if (p.Key != null)
                            {
                                if (keyBytes != null)
                                {
                                    Node newNode = CreateNewNode(p.SelfReference, keyBytes[0]);
     
                                    root.RootLengthOfKeyBytes -= p.GetKeyLength;                

                                    if (keyBytes_Length > 1)
                                    {
                                        newNode.Key = keyBytes.SubArray(1);
                                        root.RootLengthOfKeyBytes += newNode.GetKeyLength;
                                    }
                                    newNode.Value = value;
                                    Node moveNode = CreateNewNode(p.SelfReference, p.Key[0]);
       
                                    if (p.GetKeyLength > 1)
                                    {
                                        moveNode.Key = p.Key.SubArray(1);
                                        root.RootLengthOfKeyBytes += moveNode.GetKeyLength;
                                    }
                                    moveNode.Value = p.Value;
                                    moveNode.Arr = p.Arr;
                                    moveNode.NumOfPopulatedNodes = p.NumOfPopulatedNodes;
                                    
                                    p.MoveNodeArrChildrenToOtherNode(moveNode, true);
                                    newNode.SetParentReference(p.SelfReference);

                                    p.InitArr();
                                    root.RootAmountOfSubArrays++;

                                    p.NumOfPopulatedNodes = 0;
                                    p.Arr[keyBytes[0]] = newNode;
                                    p.NumOfPopulatedNodes++;                                                                

                                    p.Arr[p.Key[0]] = moveNode;
                                    p.NumOfPopulatedNodes++;

                                    p.Key = null;
                                    p.Value = null;
                                }
                                else
                                {
                                    //move previous node to new level
                                    Node moveNode = CreateNewNode(null, p.Key[0]);
     
                                    root.RootLengthOfKeyBytes--;

                                    if (p.GetKeyLength > 1)
                                    {
                                        moveNode.Key = p.Key.SubArray(1);
                                    }
                                    moveNode.Value = p.Value;
                                    moveNode.Arr = p.Arr;
                                    moveNode.NumOfPopulatedNodes = p.NumOfPopulatedNodes;
                                    p.MoveNodeArrChildrenToOtherNode(moveNode, true);

                                    p.InitArr();
                                    root.RootAmountOfSubArrays++;                             

                                    p.NumOfPopulatedNodes = 0;

                                    //set new values for current node
                                    p.Arr[p.Key[0]] = moveNode;
                                    p.NumOfPopulatedNodes++;
                              
                                    p.Key = null;
                                    p.Value = value;
                                }
                            }
                            else//if (p.Key == null)
                            {
                                if (p.Arr == null)
                                {
                                    p.InitArr();
     
                                    if (root != p)
                                    {
                                        root.RootAmountOfSubArrays++;
                                    }
                                 }

                                byte keyBytesIndex = keyBytes[0];

                                if (p.Arr[keyBytesIndex] != null)
                                {
                                    InsertKeyBytesToNode(p.Arr[keyBytesIndex], keyBytes, value);
                                }
                                else
                                {
                                    Node childNode = CreateNewNode(p.SelfReference, keyBytesIndex);
                                    if (keyBytes_Length > 1)
                                    {
                                        childNode.Key = keyBytes.SubArray(1);
                                        root.RootLengthOfKeyBytes += childNode.GetKeyLength;
                                    }
                                    childNode.Value = value;
                                    p.Arr[keyBytesIndex] = childNode;
                                    p.NumOfPopulatedNodes++;
                                }
                            }
                        }
                    }
                }
                else //if (p.Key == null)
                {
                    if (keyBytes == null || keyBytes.Length == 0)
                    {
                        if (p.Value == null)
                        {
                            p.Value = value;
                        }
                        else
                        {
                            //possible duplicate
                            p.Value = value;

                            //root.AutoIncrementDuplicatesTest.Increment();
                            //Helper.WriteToLog("duplicate 02 - " + root.AutoIncrementDuplicatesTest.GetLastAdded, Helper.WriteDuplicatesInfo);
                            //throw new Exception("check this situation if possible");
                        }
                    }
                    else
                    {
                        if (p.Value == null)
                        {
                            if (p.Arr == null && p.NumOfPopulatedNodes == 0)
                            {
                                //impossible
                                p.Value = value;
                                p.Key = keyBytes;
                                root.RootLengthOfKeyBytes += p.GetKeyLength;
                            }
                            else if (p.Arr != null && p.NumOfPopulatedNodes > 0)
                            {
                                if (p.Arr[keyBytes[0]] != null)
                                {
                                    InsertKeyBytesToNode(p.Arr[keyBytes[0]], keyBytes.SubArray(1), value);
                                }
                                else
                                {
                                    byte keyBytes_p_Index = keyBytes[0];
                                    Node childNode = CreateNewNode(p.SelfReference, keyBytes_p_Index);
                                    if (keyBytes_Length > 1)
                                    {
                                        childNode.Key = keyBytes.SubArray(1);
                                        root.RootLengthOfKeyBytes += childNode.GetKeyLength;
                                    }
                                    childNode.Value = value;
                                    p.NumOfPopulatedNodes++;
                                    p.Arr[keyBytes_p_Index] = childNode;
                                }

                            }
                        }
                        else
                        {
                            if (p.Arr != null && p.NumOfPopulatedNodes > 0 && p.Arr[keyBytes[0]] != null)
                            {
                                InsertKeyBytesToNode(p.Arr[keyBytes[0]], keyBytes.SubArray(1), value);
                            }
                            else
                            {
                                if (p.Arr == null && p.NumOfPopulatedNodes == 0)
                                {
                                    p.InitArr();
                                    if (p != root)
                                    {
                                        root.RootAmountOfSubArrays++;
                                    }
                                }

                                byte keyBytes_p_Index = keyBytes[0];
                                Node childNode = CreateNewNode(p.SelfReference, keyBytes_p_Index);
                                if (keyBytes_Length > 1)
                                {
                                    childNode.Key = keyBytes.SubArray(1);
                                    root.RootLengthOfKeyBytes += childNode.GetKeyLength;
                                }
                                childNode.Value = value;
                                p.NumOfPopulatedNodes++;
                                p.Arr[keyBytes_p_Index] = childNode;
                            }
                        }
                    }
                }
            }
        } 


        private bool CanWrite(string key)
        {
            long addingSize = GetAddingSize(key);

            long currentSize = root.RootCurrentSize;
            long newSize = addingSize + currentSize;

            Helper.WriteToLog("CanWrite() newSize = " + newSize, Helper.WriteToLogAddRemoveStatistics);

            if (newSize > Helper.MAX_LENGTH_SIZE)
            {
                return false;
            }
            return true;
        }        
        

        private void SetParentAndSelfNodeRefferences(Node parent)
        {
            NodeReference parent_SelfReference_New = new NodeReference(parent);
            parent.InitSelfReference(parent_SelfReference_New);

            if (parent.NumOfPopulatedNodes > 0 && parent.Arr != null)
            {
                int childrenFound = 0;
                for (int i = 0, length = Helper.Sizes.ARR_SIZE; i < length; i++)
                {
                    Node child = parent.Arr[i];
                    if (child != null)
                    {
                        childrenFound++;
                        child.ParentReference = parent.SelfReference;
 
                        SetParentAndSelfNodeRefferences(child);                  

                        if (childrenFound >= parent.NumOfPopulatedNodes)
                        {
                            break;
                        }
                    }
                }
            }
        }        


        public void GetNodeActualStatistics(Node parent,
            ref int rootAmountOfSubArrays,
            ref int rootGlobalAmountOfPopulatedNodes,
            ref int rootLengthOfKeyBytes,
            ref long rootStatsRootSizeForEmpty
            )
        {
            if(parent == root)
            {
                rootGlobalAmountOfPopulatedNodes += parent.NumOfPopulatedNodes;
                rootStatsRootSizeForEmpty = root.StatsRootSizeForEmpty;
            }

            if(parent.NumOfPopulatedNodes > 0 && parent.Arr != null)
            {
                int nodesFound = 0;
                for (int i = 0; i < Helper.Sizes.ARR_SIZE; i++)
                {
                    Node node = parent.Arr[i];
                    if(node != null)
                    {
                        if(node != root)
                        {
                            rootAmountOfSubArrays += (node.NumOfPopulatedNodes > 0 && node.Arr != null ? 1 : 0);
                        }
                        rootGlobalAmountOfPopulatedNodes += node.NumOfPopulatedNodes;
                        rootLengthOfKeyBytes += node.GetKeyLength;

                        GetNodeActualStatistics(node,
                            ref rootAmountOfSubArrays,
                            ref rootGlobalAmountOfPopulatedNodes,
                            ref rootLengthOfKeyBytes,
                            ref rootStatsRootSizeForEmpty);

                        nodesFound++;
                        if(nodesFound >= parent.NumOfPopulatedNodes)
                        {
                            break;
                        }
                    }
                }
            }

        }


        public bool CheckParentNodeRefferenceInChildrenFromRoot(string procedureName, bool writeToLog, bool executeTest)
        {
            bool hasErrors = false;
            if (executeTest)
            {
                if (writeToLog)
                {

                    CheckParentNodeRefferenceInChildren(root, procedureName, ref hasErrors);
                    Helper.WriteToLog("hasErrors = " + hasErrors.ToString().ToLower(), writeToLog);
                }
            }
            return hasErrors;
        }


        private void CheckParentNodeRefferenceInChildren(Node parent, string procedureName, ref bool hasErrors)
        {
            if (parent.NumOfPopulatedNodes > 0 && parent.Arr != null)
            {
                int numOfPopulatedNodes = parent.NumOfPopulatedNodes;
                int childrenFound = 0;
                for (int i = 0; i < Helper.Sizes.ARR_SIZE; i++)
                {
                    Node child = parent.Arr[i];
                    if (child != null)
                    {
                        //Helper.WriteToLog("----------------------------", true);

                        childrenFound++;

                        if (/*child.ParentReference.NodeElement != parent.SelfReference.NodeElement
                            ||*/ 
                            child.ParentReference != parent.SelfReference)
                        {                            
                            Helper.WriteToLog("----------------------------", true);
                            
                            hasErrors = true;

                            string parent_CurrentKeyLetters_string = null;
                            string child_CurrentKeyLetters_string = null;
                            string child_parent_CurrentKeyLetters_string = null;

                            if (parent.CurrentKeyLetters != null && parent.CurrentKeyLetters.Length > 0)
                            {
                                int parent_CurrentKeyLetters_Length = parent.CurrentKeyLetters.Length;
                                for (int j = 0; j < parent_CurrentKeyLetters_Length; j++)
                                {
                                    parent_CurrentKeyLetters_string += (j == 0 ? parent.CurrentKeyLetters[j].ToString() : "," + parent.CurrentKeyLetters[j].ToString());
                                }
                            }

                           
                            if (child.CurrentKeyLetters != null && child.CurrentKeyLetters.Length > 0)
                            {
                                int child_CurrentKeyLetters_Length = child.CurrentKeyLetters.Length;
                                for (int j = 0; j < child_CurrentKeyLetters_Length; j++)
                                {
                                    child_CurrentKeyLetters_string += (j == 0 ? child.CurrentKeyLetters[j].ToString() : "," + child.CurrentKeyLetters[j].ToString());
                                }
                            }

                            if (child.ParentReference.NodeElement.CurrentKeyLetters != null
                                && child.ParentReference.NodeElement.CurrentKeyLetters.Length > 0)
                            {
                                int child_parent_CurrentKeyLetters_Length = child.ParentReference.NodeElement.CurrentKeyLetters.Length;
                                for (int j = 0; j < child_parent_CurrentKeyLetters_Length; j++)
                                {
                                    child_parent_CurrentKeyLetters_string += (j == 0
                                            ? child.ParentReference.NodeElement.CurrentKeyLetters[j].ToString()
                                            : "," + child.ParentReference.NodeElement.CurrentKeyLetters[j].ToString());
                                }
                            }

                            Helper.WriteToLog(Environment.NewLine + procedureName + " ERROR - CheckParentNodeRefferenceInChildren: " + Environment.NewLine
                                + "parent.Value = " + parent.Value + Environment.NewLine
                                + "parent.ParentIndex = " + parent.ParentIndex + Environment.NewLine
                                + "parent.CurrentLetter = " + parent.CurrentLetter + Environment.NewLine
                                + "parent.CurrentKeyLetters = " + (String.IsNullOrWhiteSpace(parent_CurrentKeyLetters_string) ? "null" : parent_CurrentKeyLetters_string) + Environment.NewLine
                                + "----" + Environment.NewLine
                                + "child.Value = " + child.Value + Environment.NewLine
                                + "child.ParentIndex = " + child.ParentIndex + Environment.NewLine
                                + "child.CurrentLetter = " + child.CurrentLetter + Environment.NewLine
                                + "child.CurrentKeyLetters = " + (String.IsNullOrWhiteSpace(child_CurrentKeyLetters_string) ? "null" : child_CurrentKeyLetters_string) + Environment.NewLine
                                + "----" + Environment.NewLine
                                + "child.ParentReference.NodeElement.Value = " + child.ParentReference.NodeElement.Value + Environment.NewLine
                                + "child.ParentReference.NodeElement.CurrentLetter = " + child.ParentReference.NodeElement.CurrentLetter + Environment.NewLine

                                + "child.Parent.CurrentKeyLetters = " + (String.IsNullOrWhiteSpace(child_parent_CurrentKeyLetters_string) ? "null" : child_parent_CurrentKeyLetters_string) + Environment.NewLine
                                , true);
                        }
                        else
                        {
                            //Helper.WriteToLog("correct", true);
                        }

                        if (child.NumOfPopulatedNodes > 0 && child.Arr != null)
                        {
                            CheckParentNodeRefferenceInChildren(child, procedureName, ref hasErrors);
                        }

                        if (childrenFound >= numOfPopulatedNodes)
                        {
                            break;
                        }
                    }
                }
            }
        }

        
        private bool SearchNode(string key, out Node[] nodes, out int nodes_occupied, out int foundKeyPartSize, out int foundKeyPartSizeWithoutLastNodeKey)
        {
            bool found = false;
            Node p = root;
            byte[] keyBytes = Helper.ConvertStringToBytes(key);
            int keyBytes_OriginalLength = keyBytes.Length;
            nodes = new Node[keyBytes_OriginalLength];
            nodes_occupied = 0;
            foundKeyPartSize = 0;
            foundKeyPartSizeWithoutLastNodeKey = 0;

            string log_string = String.Empty;
            string log_string_numeric = String.Empty;

            int counter = 0;
            bool ended = false;

            while (!ended)
            {
                if (p == root)
                {
                    if (p.Arr[keyBytes[0]] == null)
                    {
                        ended = true;
                        if (Helper.WriteToLogAndCollectSearchData)
                        {
                            log_string += "/";
                            log_string_numeric += "/";
                        }         
                    }
                    else
                    {
                        p = p.Arr[keyBytes[0]];
                        nodes[counter] = p;
                        counter++;

                        foundKeyPartSize++;
                        foundKeyPartSizeWithoutLastNodeKey++;

                        if (Helper.WriteToLogAndCollectSearchData)
                        {
                            log_string += Helper.ConvertByteToChar(keyBytes[0]) + "|";
                            log_string_numeric += keyBytes[0] + "|";
                        }
                        if (keyBytes.Length > 1)
                        {
                            keyBytes = keyBytes.SubArray(1);
                        }
                        else
                        {
                            keyBytes = null;
                            ended = true;

                            if (Helper.WriteToLogAndCollectSearchData)
                            {
                                log_string += "/";
                                log_string_numeric += "/";
                            }

                            if (p.Value != null && p.Key == null)   
                            {
                                found = true;
                                if (Helper.WriteToLogAndCollectSearchData)
                                {
                                    log_string += p.Value;
                                    log_string_numeric += p.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (p != null)
                    {
                        if (p.Key != null)
                        {
                            int keyBytes_Length = (keyBytes != null ? keyBytes.Length : 0);

                            if (keyBytes_Length == 0)
                            {
                                ended = true;

                                if (Helper.WriteToLogAndCollectSearchData)
                                {
                                    log_string += "/";
                                    log_string_numeric += "/";
                                }
                            }
                            else
                            {
                                var intersectionArr = GetByteArrStartIntersection(keyBytes, p.Key);
                                int intersectionArr_Length = 0;
                                if (intersectionArr != null)
                                {
                                    intersectionArr_Length = intersectionArr.Length;
                                }

                                foundKeyPartSize += intersectionArr_Length;
                                if (Helper.WriteToLogAndCollectSearchData)
                                {
                                    log_string += "{";
                                    log_string_numeric += "{";
                                    for (int i = 0; i < intersectionArr_Length; i++)
                                    {
                                        log_string += Helper.ConvertByteToChar(intersectionArr[i]) + (i < intersectionArr_Length - 1 ? "," : "");
                                        log_string_numeric += intersectionArr[i] + (i < intersectionArr_Length - 1 ? "," : "");
                                    }
                                    log_string += "}";
                                    log_string_numeric += "}";

                                    log_string += "|";
                                    log_string_numeric += "|";

                                }

                                if (p.GetKeyLength > keyBytes_Length)
                                {
                                    ended = true;

                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += "/";
                                        log_string_numeric += "/";
                                    }
                                }
                                else
                                {
                                    if (intersectionArr_Length > 0
                                        && intersectionArr_Length == keyBytes_Length
                                        && p.GetKeyLength == intersectionArr_Length)
                                    {
                                        if (p.Value != null)
                                        {
                                            ended = true;

                                            if (Helper.WriteToLogAndCollectSearchData)
                                            {
                                                log_string += "/";
                                                log_string_numeric += "/";
                                            }

                                            found = true;

                                            if (Helper.WriteToLogAndCollectSearchData)
                                            {
                                                log_string += p.Value;
                                                log_string_numeric += p.Value;
                                            }
                                        }
                                        else
                                        {
                                            ended = true;
                                            if (Helper.WriteToLogAndCollectSearchData)
                                            {
                                                log_string += "/";
                                                log_string_numeric += "/";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (keyBytes_Length > intersectionArr_Length
                                            && intersectionArr_Length == p.GetKeyLength)
                                        {
                                            if (p.Arr == null || p.Arr[keyBytes[intersectionArr_Length]] == null)
                                            {
                                                ended = true;

                                                if (Helper.WriteToLogAndCollectSearchData)
                                                {
                                                    log_string += "/";
                                                    log_string_numeric += "/";
                                                }
                                            }
                                            else
                                            {
                                                foundKeyPartSize++;
                                                
                                                foundKeyPartSizeWithoutLastNodeKey += intersectionArr_Length;
                                                foundKeyPartSizeWithoutLastNodeKey++;

                                                if (Helper.WriteToLogAndCollectSearchData)
                                                {
                                                    log_string += Helper.ConvertByteToChar(keyBytes[intersectionArr_Length]) + "|";
                                                    log_string_numeric += keyBytes[intersectionArr_Length] + "|";
                                                }

                                                p = p.Arr[keyBytes[intersectionArr_Length]];
                                                keyBytes = keyBytes.SubArray(intersectionArr_Length + 1);

                                                nodes[counter] = p;
                                                counter++;
                                            }
                                        }
                                        else
                                        {
                                            ended = true;
                                            if (Helper.WriteToLogAndCollectSearchData)
                                            {
                                                log_string += "/";
                                                log_string_numeric += "/";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (keyBytes == null || keyBytes.Length == 0)
                            {
                                if (p.Value != null)
                                {
                                    ended = true;
                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += "/";
                                        log_string_numeric += "/";
                                    }

                                    found = true;
                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += p.Value;
                                        log_string_numeric += p.Value;
                                    }
                                }
                                else
                                {
                                    ended = true;

                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += "/";
                                        log_string_numeric += "/";
                                    }
                                }
                            }
                            else
                            {
                                if (p.Arr == null || p.Arr[keyBytes[0]] == null)
                                {
                                    ended = true;

                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += "/";
                                        log_string_numeric += "/";
                                    }
                                }
                                else
                                {
                                    p = p.Arr[keyBytes[0]];
                                    nodes[counter] = p;
                                    counter++;

                                    foundKeyPartSize++;
                                    foundKeyPartSizeWithoutLastNodeKey++;

                                    if (Helper.WriteToLogAndCollectSearchData)
                                    {
                                        log_string += Helper.ConvertByteToChar(keyBytes[0]) + "|";
                                        log_string_numeric += keyBytes[0] + "|";
                                    }

                                    if (keyBytes.Length > 1)
                                    {
                                        keyBytes = keyBytes.SubArray(1);
                                    }
                                    else
                                    {
                                        keyBytes = null;   
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Helper.WriteToLog(log_string.PadRight(40)
                + " - found: " + foundKeyPartSize.ToString().PadRight(3)

                + ", actual: " + key.Length.ToString().PadRight(3)
                + ", without_last_key: " + foundKeyPartSizeWithoutLastNodeKey.ToString().PadRight(3)
                + " - " + found.ToString().ToLower().PadRight(7)
                + ", search term: " + key.PadRight(20)
                + ", numeric: " + log_string_numeric
                , Helper.WriteToLogAndCollectSearchData);

            nodes_occupied = counter;
            return found;
        }


        private long GetAddingSize(string key)
        {
            long addingSize = 0;

            Node[] nodes = null;
            int nodes_occupied;
            int foundKeyPartSize;
            int foundKeyPartSizeWithoutLastNodeKey;
            bool key_found = SearchNode(key, out nodes, out nodes_occupied, out foundKeyPartSize, out foundKeyPartSizeWithoutLastNodeKey);

            if (!key_found)
            {
                byte[] keyBytes = Helper.ConvertStringToBytes(key);
                byte[] keyBytesToAdd = keyBytes.SubArray(foundKeyPartSizeWithoutLastNodeKey);
                int keyBytesToAdd_Length = (keyBytesToAdd != null ? keyBytesToAdd.Length : 0);


                if (keyBytesToAdd_Length > 0)
                {
                    if (nodes_occupied == 0)
                    {
                        addingSize +=
                            Helper.Sizes.size_of_Node_element
                            + ((keyBytesToAdd_Length - 1) * sizeof(byte));
                    }
                    else
                    {
                        Node lastNode = nodes[nodes_occupied - 1];

                        if (lastNode.Key != null && lastNode.Key.Length > 0)
                        {
                            int lastNode_Key_Length = lastNode.Key.Length;

                            byte[] intersectionArr = GetByteArrStartIntersection(keyBytesToAdd, lastNode.Key);
                            int intersectionArr_Length = (intersectionArr != null ? intersectionArr.Length : 0);
                            if (intersectionArr_Length == keyBytesToAdd_Length && intersectionArr_Length == lastNode_Key_Length)
                            {
                                //only value to be set
                                addingSize += 0;
                            }
                            else if (intersectionArr_Length > 0
                                && (intersectionArr_Length == keyBytesToAdd_Length || intersectionArr_Length == lastNode_Key_Length)
                                && keyBytesToAdd_Length != lastNode_Key_Length)
                            {
                                //new array size
                                //PLUS
                                //new node size
                                //PLUS
                                //size of (keyBytesToAdd_Length - 1) * sizeof(byte)

                                int key_length_to_add = 0;

                                if (keyBytesToAdd_Length > intersectionArr_Length)
                                {
                                    key_length_to_add = keyBytesToAdd_Length - intersectionArr_Length;
                                }

                                addingSize +=
                                    Helper.Sizes.size_of_Node_Array
                                    + Helper.Sizes.size_of_Node_element
                                    + (key_length_to_add - 1) * sizeof(byte);
                            }
                            else //if (intersectionArr_Length == 0)
                            {
                                //newArray size
                                //PLUS
                                //two nodes size
                                //PLUS
                                //(size of keyBytesToAdd_Length - 1
                                //MINUS
                                //sizeof(byte) * 1 - cost of removing one lastNode.Key[intersection_Length] byte for creating new Node

                                addingSize +=
                                      Helper.Sizes.size_of_Node_Array
                                      + (Helper.Sizes.size_of_Node_element * 2)
                                      + ((keyBytesToAdd_Length - intersectionArr_Length - 2) * sizeof(byte)); //new added key minus length of its first element and substract length of first current key element
                            }
                        }
                        else
                        {
                            if (lastNode.Value == null)
                            {
                                //cost equals:
                                //cost of adding only rest of the key
                                //plus size of adding new value which is 0
                                addingSize += sizeof(byte) * keyBytesToAdd_Length;
                            }
                            else
                            {
                                if (!(lastNode.Arr == null || lastNode.NumOfPopulatedNodes == 0))
                                {
                                    //cost:
                                    //1. adding new Array
                                    //PLUS
                                    //2. populating on of its nodes
                                    //PLUS
                                    //3. adding rest of the key WITHOUT first character
                                    addingSize +=
                                         Helper.Sizes.size_of_Node_element
                                        + (keyBytesToAdd_Length - 1) * sizeof(byte);
                                }
                                else
                                {
                                    //cost:
                                    //1. populating lastNode.Arr[keyBytesToAdd[0]]
                                    //(cost of creating new Node)
                                    //PLUS
                                    //2. setting its newNode.Key to keyBytesToAdd.SubArray(1) - GET SIZE
                                    addingSize +=
                                        Helper.Sizes.size_of_Node_Array
                                        + Helper.Sizes.size_of_Node_element
                                        + ((keyBytesToAdd_Length - 1) * sizeof(byte));
                                }

                            }
                        }
                    }

                }
                else //if (keyBytesToAdd_Length == 0)
                {
                    Node lastNode = nodes[nodes_occupied - 1];
                    if (lastNode.Value != null && lastNode.Key != null)
                    {
                        //add new sub array
                        //add new node element
                        //substract first element of lastNode.Key for new node index
                        addingSize +=
                            Helper.Sizes.size_of_Node_Array
                            + Helper.Sizes.size_of_Node_element
                            - (1 * sizeof(byte));
                    }
                    else
                    {
                        addingSize += 0;
                    }
                }
            }

            return addingSize;
        }


        private byte[] GetByteArrStartIntersection(byte[] arr1, byte[] arr2)
        {
            byte[] res = null;

            if (arr1 != null && arr2 != null)
            {
                int arr1_len = arr1.Length;
                int arr2_len = arr2.Length;

                if (arr1_len > 0 && arr1_len > 0)
                {
                    int min_len;
                    if (arr1_len < arr2_len)
                    {
                        min_len = arr1_len;
                    }
                    else
                    {
                        min_len = arr2_len;
                    }

                    if (min_len > 0)
                    {
                        int intersectMaxIndex = -1;
                        for (int i = 0; i < min_len; i++)
                        {
                            if (arr1[i] == arr2[i])
                            {
                                intersectMaxIndex = i;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (intersectMaxIndex >= 0)
                        {
                            res = new byte[intersectMaxIndex + 1];
                            Array.Copy(arr1, res, intersectMaxIndex + 1);
                        }
                    }
                }
            }
            return res;
        }

        #endregion
    }
}
