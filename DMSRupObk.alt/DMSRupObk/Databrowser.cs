//Eine TreeBuilder-Klasse, die alle notwendigen Methoden enthält, die komplette Verzeichnisstruktur eines 
//Wurzelknotens(eines Hauptverzeichnisses) in einen TreeView auszugeben.

using System;
using System.Windows.Forms;
using System.IO;

namespace DataBrowser {

    class TreeBuilder {

        private TreeView treeView;
        private TreeNode rootNode;

        //Normaler Konstruktor, der die Referenz eines TreeViews übernimmt
        public TreeBuilder(TreeView treeView) {
            this.treeView = treeView;
        }

        //"Advanced"-Konstruktor, der sofort den TreeView aufbaut
        public TreeBuilder(TreeView treeView, TreeNode rootNode) : this(treeView) {
            buildTree(rootNode);
        }

        //Methode buildTree(), die den Hauptbaum mit Wurzelknoten erstellt
        public void buildTree(TreeNode rootNode) {
            treeView.Nodes.Clear();
            this.rootNode = rootNode;
            treeView.Nodes.Add(rootNode);
            addChildNodes(treeView.Nodes[0]);
            treeView.Nodes[0].Expand();
        }

        //Wohl wichtigste Methode addChildNodes(), die jedem Knoten die Kindknoten hinzufügt
        private void addChildNodes(TreeNode node) {
            DirectoryInfo dir = new DirectoryInfo(node.FullPath);
            try {
                foreach(DirectoryInfo dirItem in dir.GetDirectories()) {
                    TreeNode newNode = new TreeNode(dirItem.Name);
                    node.Nodes.Add(newNode);
                    newNode.Nodes.Add("*");
                }
            } catch(UnauthorizedAccessException err) {
                MessageBox.Show(err.ToString());
            }
        }

        //Diese Methode muss aufgerufen werden, wenn ein Knoten expandiert wird.
        public void expandNode(TreeViewCancelEventArgs e) {
            if(e.Node.Nodes[0].Text == "*") {
                treeView.BeginUpdate();
                e.Node.Nodes.Clear();
                addChildNodes(e.Node);
                treeView.EndUpdate();
            }
        }

    }

}

