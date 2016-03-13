using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guessing_Game
{
    static class Program
    {
        const string title = "Guessing Game";
        const string startMessage = "Think about an animal...";
        const string approximateQuestion = "Does the animal that you thought about {0}?";
        const string tryGuessQuestion = "Is the animal that you thought about a {0}?";
        const string askAnimalQuestion = "What was the animal that you thought about?";
        const string getAnimalTraitQuestion = "A {0} ________ but a {1} does not (Fill it with an animal trait, like 'lives in water').";
        const string winMessage = "I win again!";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AnimalTree questionTree = new AnimalTree();

            do
            {
                DialogResult wantToPlay = MessageBox.Show(startMessage, title, MessageBoxButtons.OKCancel);

                if (wantToPlay == DialogResult.OK)
                    questionTree.root = Guess(questionTree.root);
                else
                    return;
            }
            while (questionTree.root != null);
        }
        
        static Node Guess(Node currentQuestion)
        {
            if (currentQuestion != null && !String.IsNullOrEmpty(currentQuestion.Trait))
            {
                DialogResult askQuestionDialog = MessageBox.Show(String.Format(approximateQuestion, currentQuestion.Trait),
                                                                title,
                                                                MessageBoxButtons.YesNo);

                if (askQuestionDialog == DialogResult.Yes)
                    Guess(currentQuestion.Yes);
                else
                    Guess(currentQuestion.No);
            }
            else
            {
                DialogResult tryGuessDialog = MessageBox.Show(String.Format(tryGuessQuestion, currentQuestion.Animal),
                                                              title,
                                                              MessageBoxButtons.YesNo);

                if (tryGuessDialog == DialogResult.Yes)
                {
                    DialogResult correctDialog = MessageBox.Show(winMessage, title);
                }
                else
                {                    
                    //Ask for what animal you thought about
                    var formReadInformation = new ReadInformation(askAnimalQuestion);
                    var result = formReadInformation.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return currentQuestion;

                    string animal = formReadInformation.Answer;

                    //Ask for what this animal does
                    formReadInformation = new ReadInformation(String.Format(getAnimalTraitQuestion, animal, currentQuestion.Animal));
                    result = formReadInformation.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return currentQuestion;

                    string trait = formReadInformation.Answer;

                    var correctAnimal = new Node { Animal = animal };
                    var wrongAnimal = new Node { Animal = currentQuestion.Animal };

                    currentQuestion.InsertAnimal(correctAnimal, ref currentQuestion, true);
                    currentQuestion.InsertAnimal(wrongAnimal, ref currentQuestion, false);

                    currentQuestion.Trait = trait;
                    currentQuestion.Animal = String.Empty;
                }
            }

            return currentQuestion;
        }
    }

    class AnimalTree
    {
        public Node root;

        public AnimalTree()
        {
            root = new Node { Trait = "lives in water" };

            var waterAnimal = new Node { Animal = "shark" };
            var noWaterAnimal = new Node { Animal = "monkey" };

            root.InsertAnimal(waterAnimal, ref root, true);
            root.InsertAnimal(noWaterAnimal, ref root, false);
        }        
    }

    class Node
    {
        public string Animal { get; set; }
        public string Trait { get; set; }
        public Node Yes { get; set; }
        public Node No { get; set; }

        public Node() { }

        public void InsertAnimal(Node newNode, ref Node currentNode, bool isYes)
        {
            if (isYes)
                currentNode.Yes = newNode;
            else
                currentNode.No = newNode;
        }
    }
}
