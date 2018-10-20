using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Trie01
{
    public partial class Trie
    {
        #region MainDeclarations

        #region Node

        [Serializable]
        [DataContract(IsReference = true)]
        public class Node
        {
            public byte[] Key { get; set; }

            public long? Value { get; set; }

            public Node[] Arr;

            private int _numOfPopulatedNodes;
        
            public int NumOfPopulatedNodes
            {
                get
                {
                    return _numOfPopulatedNodes;
                }
                set
                {
                    _numOfPopulatedNodes = value;
                }
            }

            [XmlIgnore]
            [ScriptIgnore]
            public Node Parent;

            public byte? ParentIndex;

            [XmlIgnore]
            [ScriptIgnore]
            public NodeReference SelfReference { get; set; }

            [XmlIgnore]
            [ScriptIgnore]
            public NodeReference ParentReference { get; set; }

            public void InitSelfReference(NodeReference newReference)
            {
                this.SelfReference = newReference;
            }

            public char? CurrentLetter
            {
                get
                {
                    if (ParentIndex != null)
                    {
                        return Helper.ConvertByteToChar((byte)ParentIndex);
                    }
                    else
                    {
                        return null;
                    }                    
                }
            }

            public char[] CurrentKeyLetters
            {
                get
                {
                    if (this.Key != null)
                    {
                        char[] chars = new char[this.Key.Length];
                        for (int i = 0, length = this.Key.Length; i < length; i++)
                        {
                            chars[i] = Helper.ConvertByteToChar(this.Key[i]);
                        }
                        return chars;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            public Node(NodeReference parentReference, byte? parentIndex)
            {
                this.ParentReference = parentReference;
                this.ParentIndex = parentIndex;
                NodeReference selfReference = new NodeReference(this);
                this.InitSelfReference(selfReference);
            }


            internal Node()
            {

            }

            public void InitArr()
            {
                Arr = new Node[Helper.Sizes.ARR_SIZE];
            }

            public void ChangeSelfTo(Node otherNode)
            {
                this.ParentReference.NodeElement.Arr[(byte)this.ParentIndex] = otherNode;
            }

            public void Clean()
            {
                Arr = null;
                Value = null;
                Node parentNode = this.ParentReference.NodeElement;
                parentNode.Arr[(byte)ParentIndex] = null;
                parentNode.NumOfPopulatedNodes--;
            }

            public int GetKeyLength
            {
                get
                {
                    return this.Key != null ? this.Key.Length : 0;
                }
            }

            public void MoveNodeArrChildrenToOtherNode(Node otherNode, bool connectOtherNodeAsChild)
            {
                otherNode.SetSelfReference(this.SelfReference);
                otherNode.SetSelfReferenceNode(otherNode);

                this.SetSelfReference(new NodeReference(this));
                if (connectOtherNodeAsChild)
                {
                    otherNode.SetParentReference(this.SelfReference);
                }
            }

            public void SetSelfReference(NodeReference selfReference)
            {
                SelfReference = selfReference;
            }

            public void SetSelfReferenceNode(Node node)
            {
                this.SelfReference.NodeElement = node;
            }

            public void SetParentReference(NodeReference parentReference)
            {
                ParentReference = parentReference;
            }
        }

        #endregion

        #region RootNode

        [Serializable]
        [DataContract(IsReference = true)]
        public class RootNode : Node
        {

            public RootNode(NodeReference parentReference)
                : base(parentReference, null)
            {

            }

            internal RootNode()
            {

            }

            private int _rootAmountOfSubArrays;
            public int RootAmountOfSubArrays
            {
                get { return _rootAmountOfSubArrays; }
                set
                {
                    if (_rootAmountOfSubArrays > value)
                    {
                        Helper.WriteToLog("decrease: " + value, false);
                    }
                    else if (_rootAmountOfSubArrays < value)
                    {
                        Helper.WriteToLog("increase: " + value, false);
                    }
                    _rootAmountOfSubArrays = value;
                }
            }

            //for test purposes only - begin
            //[XmlIgnore]
            //[ScriptIgnore]
            //public AutoIncrementValue AutoIncrementDuplicatesTest = new AutoIncrementValue(0);            
            //for test purposes only - end


            public int RootGlobalAmountOfPopulatedNodes {get; set;}

            public int RootLengthOfKeyBytes { get; set; }

            [XmlIgnore]
            [ScriptIgnore]
            public long StatsRootSizeForEmpty = 0;



            [XmlIgnore]
            [ScriptIgnore]
            public long RootCurrentSize
            {
                get
                {
                    int res = 0;

                    int amount_of_all_KeyBytes = this.RootLengthOfKeyBytes; //all key bytes

                    int size_of_all_KeyBytes = amount_of_all_KeyBytes * sizeof(byte); //byte[] Key in all Nodes

                    res += (Helper.Sizes.size_of_Node_Array * this.RootAmountOfSubArrays)
                        + Helper.Sizes.size_of_RootNode_element
                        + (Helper.Sizes.size_of_Node_element * this.RootGlobalAmountOfPopulatedNodes)
                        + size_of_all_KeyBytes
                        + sizeof(int);//StatsRootSizeForEmpty - stats check


                    return res;
                }
            }
        }

        #endregion

        #region NodeReference

        [Serializable]
        [DataContract(IsReference = true)]
        public class NodeReference
        {
            [XmlIgnore]
            [ScriptIgnore]
            public Node NodeElement { get; set; }

            public NodeReference(Node nodeElement)
            {
                this.NodeElement = nodeElement;
            }

            public NodeReference()
            {

            }
        }

        #endregion

        #region Methods

        public Node CreateNewNode(NodeReference parentReference, byte? parentIndex)
        {
            var newNode = new Node(parentReference, parentIndex);

            this.root.RootGlobalAmountOfPopulatedNodes++;
            return newNode;
        }
        public void DeleteNode(Node node)
        {
            Node parentNode = node.ParentReference.NodeElement;
            parentNode.Arr[(byte)node.ParentIndex].Clean();
            this.root.RootGlobalAmountOfPopulatedNodes--;
        }

        #endregion

        #endregion
    }

}
