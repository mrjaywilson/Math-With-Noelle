using System.Collections.Generic;

public class Round
{
    private List<int> _numberList = new List<int>();

    public int NumberSet { get; set; }

    public ProblemInfo ProblemInfo { get; private set; }

    public void GetNewRound()
    {
        if (ProblemInfo == null)
        {
            ProblemInfo = new ProblemInfo();
        }

        CheckNumberList();

        SetProblem();
        SetAnswers();

    }

    private void CheckNumberList()
    {
        if (_numberList.Count == 0)
        {
            for (int i = 1; i < 10; i++)
            {
                _numberList.Add(i);
            }
        }
    }

    private void SetProblem()
    {

        int randomNumber;

        if (NumberSet == 0)
        {
            GetRandomNumberEqation();
        }
        else
        {
            GetBasicNumberEquation();
        }
    }

    private void GetRandomNumberEqation()
    {
        ProblemInfo.FirstNumber = UnityEngine.Random.Range(1, 10);
        ProblemInfo.SecondNumber = UnityEngine.Random.Range(1, 10);
    }

    private void GetBasicNumberEquation()
    {
        var random = new System.Random();
        var index = random.Next(_numberList.Count - 1);

        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            ProblemInfo.FirstNumber = NumberSet;
            ProblemInfo.SecondNumber = _numberList[index];
        }
        else
        {
            ProblemInfo.FirstNumber = _numberList[index];
            ProblemInfo.SecondNumber = NumberSet;
        }

        _numberList.Remove(_numberList[index]);
    }

    private void SetAnswers()
    {
        var correctAnswerChoice = UnityEngine.Random.Range(1, 4);

        switch (correctAnswerChoice)
        {
            case 1:
                ProblemInfo.AnswerOne = ProblemInfo.FirstNumber * ProblemInfo.SecondNumber;
                ProblemInfo.AnswerTwo = UnityEngine.Random.Range(1, 100);
                ProblemInfo.AnswerThree = UnityEngine.Random.Range(1, 100);
                break;
            case 2:
                ProblemInfo.AnswerOne = UnityEngine.Random.Range(1, 100);
                ProblemInfo.AnswerTwo = ProblemInfo.FirstNumber * ProblemInfo.SecondNumber;
                ProblemInfo.AnswerThree = UnityEngine.Random.Range(1, 100);
                break;
            case 3:
                ProblemInfo.AnswerOne = UnityEngine.Random.Range(1, 100);
                ProblemInfo.AnswerTwo = UnityEngine.Random.Range(1, 100);
                ProblemInfo.AnswerThree = ProblemInfo.FirstNumber * ProblemInfo.SecondNumber;
                break;
        }
    }

    public string GetFirstNumber() => ProblemInfo.FirstNumber.ToString();
    public string GetSecondNumber() => ProblemInfo.SecondNumber.ToString();
    public string GetAnswerOne() => ProblemInfo.AnswerOne.ToString();
    public string GetAnswerTwo() => ProblemInfo.AnswerTwo.ToString();
    public string GetAnswerThree() => ProblemInfo.AnswerThree.ToString();

    internal bool CheckAnswer(int answerNumber)
    {
        bool result = false;

        switch (answerNumber)
        {
            case 1:

                result = ProblemInfo.AnswerOne == GetAnswer();
                break;

            case 2:

                result = ProblemInfo.AnswerTwo == GetAnswer();
                break;

            case 3:

                result = ProblemInfo.AnswerThree == GetAnswer();
                break;
        }

        return result;
    }

    private int GetAnswer() => ProblemInfo.FirstNumber * ProblemInfo.SecondNumber;
}

public class ProblemInfo
{
    public int FirstNumber { get; set; }
    public int SecondNumber { get; set; }
    public int AnswerOne { get; set; }
    public int AnswerTwo { get; set; }
    public int AnswerThree { get; set; }
}
