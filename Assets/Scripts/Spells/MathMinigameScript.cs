using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Linq;

public class MathMinigameScript : MonoBehaviour
{
    int chosenSpellsIndexHolder;
    public GameObject enterAnwserPanel;
    public Animator animator;
    public int howManyEmpty;
    public List<float> tempNumbersEq;

    StringBuilder equasion = new StringBuilder();
    StringBuilder equasionShow = new StringBuilder();
    StringBuilder equasionCheck = new StringBuilder();
    int character;
    bool checkNextNum = false;
    float numBefore;
    List<int> numBeforeDividers;
    bool hasDiv = false;
    double result;
    bool isCorrect;
    List<GameObject> placeHolders;
    List<float> usedNums;
    List<DragableObject> usedGemsObjects;

    private void Start()
    {
        placeHolders = new List<GameObject>();

        foreach (Transform child in enterAnwserPanel.transform)
        {
            placeHolders.Add(child.gameObject);
        }

    }

    public void SlideUp(int chosenSpellsIndex, Difficulty dif)
    {
        chosenSpellsIndexHolder = chosenSpellsIndex;
        usedNums = new List<float>();
        usedGemsObjects = new List<DragableObject>();
        animator.Play("MathPanelSlideAnim");
        MakeEquastion(dif);
    }

    public void SlideOff()
    {
        Debug.Log("kys");
        animator.Play("SlideOffScreenAnim");
        RemoveUsedGems();
        if (isCorrect)
        {
            GameManager.instance.player.GetComponent<PlayerController>().ProceedAttack(chosenSpellsIndexHolder);
            GameManager.instance.player.GetComponent<PlayerController>().StartCooldown(chosenSpellsIndexHolder, GameManager.instance.chosenSpells[chosenSpellsIndexHolder].cd);
        }
        else
        {
            GameManager.instance.player.GetComponent<PlayerController>().canUseAbility = true;
        }
    }

    void RemoveUsedGems()
    {
        foreach (var gem in usedNums)
        {
            GameManager.instance.numbersEq.Remove(gem);
        }
        foreach (var gemObject in usedGemsObjects)
        {
            Destroy(gemObject.gameObject);
        }
        GameManager.instance.UpdateEqNums();
    }

    void MoveBackUsedGems()
    {
        foreach (var child in placeHolders)
        {
            usedGemsObjects.Add(child.GetComponent<NumDropPlaceHolders>().draggable);
        }
        foreach (var gemObject in usedGemsObjects)
        {
            if (gemObject != null)
            {
                Destroy(gemObject.gameObject);
            }
        }
        GameManager.instance.UpdateEqNums();
        GameManager.instance.UpdateEqNums();
    }

    public void CheckAnwser()
    {
        for (int i = placeHolders.Count - 1; i >= 0; i--)
        {
            var child = placeHolders[i];

            if (child.GetComponent<NumDropPlaceHolders>().valueOfGem != 21)
            {
                equasionCheck = equasionCheck.Remove(2 * (child.GetComponent<NumDropPlaceHolders>().placeHolderId - 1), 1);
                equasionCheck = equasionCheck.Insert(2 * (child.GetComponent<NumDropPlaceHolders>().placeHolderId - 1), child.GetComponent<NumDropPlaceHolders>().valueOfGem.ToString());
                usedNums.Add(child.GetComponent<NumDropPlaceHolders>().valueOfGem);
                usedGemsObjects.Add(child.GetComponent<NumDropPlaceHolders>().draggable);
            }
            else
            {
                equasionCheck = equasionCheck.Remove(2 * (child.GetComponent<NumDropPlaceHolders>().placeHolderId - 1), 1);
                equasionCheck = equasionCheck.Insert(2 * (child.GetComponent<NumDropPlaceHolders>().placeHolderId - 1), "-21");
            }

        }

        Debug.Log("checked eq: " + equasionCheck.ToString());

        StringToFormula stf = new StringToFormula();
        double resultCheck = stf.Eval(equasionCheck.ToString());

        if (resultCheck == result)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }

        SlideOff();

        Debug.Log(isCorrect);
    }

    public void MakeEquastion(Difficulty dif)
    {
        equasion.Clear();
        equasionShow.Clear();
        equasionCheck.Clear();


        tempNumbersEq = new List<float>();

        switch (dif)
        {
            case Difficulty.Easy:
                howManyEmpty = 2 + UnityEngine.Random.Range(0, 2);
                break;

            case Difficulty.Medium:
                howManyEmpty = 3 + UnityEngine.Random.Range(0, 3);
                break;

            case Difficulty.Hard:
                howManyEmpty = 5 + UnityEngine.Random.Range(0, 3);
                break;

            case Difficulty.Impossible:
                howManyEmpty = 8 + UnityEngine.Random.Range(0, 4);
                break;
        }

        foreach (var placeHolder in placeHolders)
        {
            placeHolder.SetActive(false);
            if (placeHolders.FindIndex(item => item == placeHolder) < howManyEmpty)
            {
                placeHolder.SetActive(true);
            }
        }

        if (howManyEmpty > GameManager.instance.numbersEq.Count)
        {
            Error();
        }
        else
        {
            foreach (var number in GameManager.instance.numbersEq)
            {
                tempNumbersEq.Add(number);
            }

            for (int i = 0; i < howManyEmpty; i++)
            {
            changeAddedNum:;
                var randomNum = UnityEngine.Random.Range(0, tempNumbersEq.Count - 1);
                float addedNumber = tempNumbersEq[randomNum];

                if (checkNextNum)
                {
                    if (!hasDiv)
                    {
                        foreach (var div in numBeforeDividers)
                        {
                            if (addedNumber == div)
                            {
                                hasDiv = true;
                            }
                        }
                    }

                    if (hasDiv || tempNumbersEq.Contains(1) || tempNumbersEq.Contains(2) || tempNumbersEq.Contains(4) || tempNumbersEq.Contains(5))
                    {
                        if (addedNumber != 0 && (addedNumber == 1 || addedNumber == 2 || addedNumber == 4 || addedNumber == 5 || numBefore % addedNumber == 0))
                        {
                            checkNextNum = false;
                        }
                        else
                        {
                            goto changeAddedNum;
                        }
                    }
                    else
                    {
                        checkNextNum = false;
                        character = UnityEngine.Random.Range(0, 3);
                        switch (character)
                        {
                            case 0:
                                equasion.Remove(equasion.Length - 2, 2).Append("- ");
                                equasionShow.Remove(equasionShow.Length - 2, 2).Append("- ");
                                equasionCheck.Remove(equasionCheck.Length - 1, 1).Append("-");
                                break;

                            case 1:
                                equasion.Remove(equasion.Length - 2, 2).Append("+ ");
                                equasionShow.Remove(equasionShow.Length - 2, 2).Append("+ ");
                                equasionCheck.Remove(equasionCheck.Length - 1, 1).Append("+");
                                break;

                            case 2:
                                equasion.Remove(equasion.Length - 2, 2).Append("* ");
                                equasionShow.Remove(equasionShow.Length - 2, 2).Append("* ");
                                equasionCheck.Remove(equasionCheck.Length - 1, 1).Append("*");
                                break;
                        }
                    }
                }

                equasion.Append(addedNumber).Append(" ");
                equasionShow.Append("\u25A1 ");

                equasionCheck.Append(" ");

                tempNumbersEq.RemoveAt(randomNum);

                if (i != howManyEmpty - 1)
                {
                    character = UnityEngine.Random.Range(0, 4);

                    switch (character)
                    {
                        case 0:
                            equasion.Append("- ");
                            equasionShow.Append("- ");
                            equasionCheck.Append("-");
                            break;

                        case 1:
                            equasion.Append("+ ");
                            equasionShow.Append("+ ");
                            equasionCheck.Append("+");
                            break;

                        case 2:
                            equasion.Append("* ");
                            equasionShow.Append("* ");
                            equasionCheck.Append("*");
                            break;

                        case 3:
                            equasion.Append("/ ");
                            equasionShow.Append("/ ");
                            equasionCheck.Append("/");
                            numBefore = addedNumber;
                            numBeforeDividers = GetDivisors((int)addedNumber).ToList();
                            hasDiv = false;
                            checkNextNum = true;
                            break;
                    }
                }

            }

            StringToFormula stf = new StringToFormula();
            result = stf.Eval(equasion.ToString());

            equasion.Append("= ").Append(result);
            equasionShow.Append("= ").Append(result);

            if ((dif == Difficulty.Easy && result > 200) || (dif == Difficulty.Medium && result > 500) || (dif == Difficulty.Hard && result > 1000))
            {
                MakeEquastion(dif);
                goto end;
            }

            transform.GetChild(0).GetComponent<TMP_Text>().text = equasionShow.ToString();
        }

    end:;
    }

    private static IEnumerable<int> GetDivisors(int n)
    {
        if (n <= 0) { yield return default; }

        int iterator = (int)Math.Sqrt(n);

        for (int i = 1; i <= iterator; i++)
        {
            if (n % i == 0)
            {
                yield return i;

                if (i != n / i) { yield return n / i; }
            }
        }
    }

    public static IEnumerable<int> GetDivisors(int n, bool AscendingOrder = false)
    {
        return !AscendingOrder ? GetDivisors(n) : GetDivisors(n).OrderBy(x => x);
    }


    public class StringToFormula
    {
        private static readonly string[] operators = { "+", "-", "/", "%", "*", "^" };
        private static readonly Func<double, double, double>[] operations = {
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 % a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

        public bool TryEval(string expression, out double value)
        {
            try
            {
                value = Eval(expression);
                return true;
            }
            catch
            {
                value = 0.0;
                return false;
            }
        }

        public double Eval(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return 0.0;

            if (double.TryParse(expression, out double value))
                return value;

            List<string> tokens = GetTokens(expression);
            tokens.Add("$"); // Append end of expression token
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count - 1)
            {
                string token = tokens[tokenIndex];
                string nextToken = tokens[tokenIndex + 1];

                switch (token)
                {
                    case "(":
                        {
                            string subExpr = GetSubExpression(tokens, ref tokenIndex);
                            operandStack.Push(Eval(subExpr));
                            continue;
                        }
                    case ")":
                        throw new ArgumentException("Mis-matched parentheses in expression");

                    // Handle unary ops
                    case "-":
                    case "+":
                        {
                            if (!IsOperator(nextToken) && operatorStack.Count == operandStack.Count)
                            {
                                operandStack.Push(double.Parse($"{token}{nextToken}"));
                                tokenIndex += 2;
                                continue;
                            }
                        }
                        break;
                }

                if (IsOperator(token))
                {
                    while (operatorStack.Count > 0 && OperatorPrecedence(token) <= OperatorPrecedence(operatorStack.Peek()))
                    {
                        if (!ResolveOperation())
                        {
                            throw new ArgumentException(BuildOpError());
                        }
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                if (!ResolveOperation())
                    throw new ArgumentException(BuildOpError());
            }

            return operandStack.Pop();

            bool IsOperator(string token)
            {
                return Array.IndexOf(operators, token) >= 0;
            }
            int OperatorPrecedence(string op)
            {
                switch (op)
                {
                    case "^":
                        return 3;
                    case "*":
                    case "/":
                    case "%":
                        return 2;

                    case "+":
                    case "-":
                        return 1;
                    default:
                        return 0;
                }
            }

            string BuildOpError()
            {
                string op = operatorStack.Pop();
                string rhs = operandStack.Any() ? operandStack.Pop().ToString() : "null";
                string lhs = operandStack.Any() ? operandStack.Pop().ToString() : "null";
                return $"Operation not supported: {lhs} {op} {rhs}";
            }

            bool ResolveOperation()
            {
                if (operandStack.Count < 2)
                {
                    return false;
                }

                string op = operatorStack.Pop();
                double rhs = operandStack.Pop();
                double lhs = operandStack.Pop();

                operandStack.Push(operations[Array.IndexOf(operators, op)](lhs, rhs));
                Debug.Log($"Resolve {lhs} {op} {rhs} = {operandStack.Peek()}");
                return true;
            }
        }

        private static string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                switch (token)
                {
                    case "(": parenlevels += 1; break;
                    case ")": parenlevels -= 1; break;
                }

                if (parenlevels > 0)
                    subExpr.Append(token);

                index += 1;
            }

            if (parenlevels > 0)
                throw new ArgumentException("Mis-matched parentheses in expression");

            return subExpr.ToString();
        }

        private static List<string> GetTokens(string expression)
        {
            string operators = "()^*/%+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }


    void Error()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = "Too little numbers.";
    }

    public void Cancel()
    {
        animator.Play("SlideOffScreenAnim");
        MoveBackUsedGems();
        GameManager.instance.player.GetComponent<PlayerController>().StartCooldown(chosenSpellsIndexHolder, GameManager.instance.chosenSpells[chosenSpellsIndexHolder].cd);
        GameManager.instance.player.GetComponent<PlayerController>().canUseAbility = true;
    }
}