namespace Italbytz.AI.Util.MathUtils.Permute;

public class CombinationGenerator
{
    public static double Ncr(int n, int r)
    {
        int rfact = 1, nfact = 1, nrfact = 1, temp1 = n - r, temp2 = r;
        if (r > n - r)
        {
            temp1 = r;
            temp2 = n - r;
        }

        for (var i = 1; i <= n; i++)
        {
            if (i <= temp2)
            {
                rfact *= i;
                nrfact *= i;
            }
            else if (i <= temp1)
            {
                nrfact *= i;
            }

            nfact *= i;
        }

        return nfact / (double)(rfact * nrfact);
    }

    public static int[] GenerateNextCombination(int[] temp, int n, int r)
    {
        var m = r;
        var maxVal = n;
        while (temp[m - 1] == maxVal)
        {
            m = m - 1;
            maxVal--;
        }

        temp[m - 1]++;
        for (var j = m; j < r; j++) temp[j] = temp[j - 1] + 1;
        return temp;
    }
}