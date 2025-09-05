using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Italbytz.AI.Util;
using LpSolveDotNet;
using Microsoft.Extensions.Logging;

namespace Italbytz.AI.Search.Continuous;

public class LPSolver : ILPSolver
{
    public LPSolver(ILoggerFactory loggerFactory)
    {
        LoggingExtensions.InitLoggers(loggerFactory);
        LpSolve.Init();
    }

    public ILPSolution Solve(string model, LPFileFormat format)
    {
        var lpTempFile = Path.GetTempFileName();
        using var outputFile = new StreamWriter(lpTempFile);
        outputFile.Write(model);
        outputFile.Close();

        return SolveFile(lpTempFile, format);
    }

    public ILPSolution Solve(ILPModel model)
    {
        var variables = model.ObjectiveFunction.Length;
        using var lp = LpSolve.make_lp(0, variables);
        if (model.Maximization) lp.set_maxim();
        lp.set_obj_fn(new[] { 0.0 }.Concat(model.ObjectiveFunction).ToArray());
        for (var i = 0; i < variables; i++)
        {
            lp.set_bounds(i + 1, model.Bounds[i].Item1, model.Bounds[i].Item2);
            lp.set_int(i + 1, model.IntegerVariables[i]);
        }

        foreach (var constraint in model.Constraints)
        {
            var constraintType = constraint.ConstraintType switch
            {
                ConstraintType.FR => lpsolve_constr_types.FR,
                ConstraintType.LE => lpsolve_constr_types.LE,
                ConstraintType.GE => lpsolve_constr_types.GE,
                ConstraintType.EQ => lpsolve_constr_types.EQ,
                ConstraintType.OF => lpsolve_constr_types.OF,
                _ => throw new ArgumentOutOfRangeException()
            };
            lp.add_constraint(
                new[] { 0.0 }.Concat(constraint.Coefficients).ToArray(),
                constraintType, constraint.RHS);
        }

        lp.print_lp();
        return RunLP(lp);
    }

    public ILPSolution SolveFile(string filename, LPFileFormat format)
    {
        var lp = format switch
        {
            LPFileFormat.lp_solve => LpSolve.read_LP(filename, 0, null),
            LPFileFormat.MPS_FIXED => LpSolve.read_MPS(filename, 0,
                lpsolve_mps_options.MPS_FIXED),
            LPFileFormat.MPS_FREE => LpSolve.read_MPS(filename, 0,
                lpsolve_mps_options.MPS_FREE),
            LPFileFormat.MPS_IBM => LpSolve.read_MPS(filename, 0,
                lpsolve_mps_options.MPS_IBM),
            LPFileFormat.MPS_NEGOBJCONST => LpSolve.read_MPS(filename, 0,
                lpsolve_mps_options.MPS_NEGOBJCONST),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format,
                null)
        };

        lp.print_lp();
        return RunLP(lp);
    }

    private ILPSolution RunLP(LpSolve lp)
    {
        lp.put_logfunc(Logfunc, IntPtr.Zero);
        var statuscode = lp.solve();

        var objTempFile = Path.GetTempFileName();
        lp.set_outputfile(objTempFile);
        lp.print_objective();
        lp.set_outputfile(null);

        double objective;
        using (var inputFile = new StreamReader(objTempFile))
        {
            var line = inputFile.ReadToEnd();
            var replacement = line.Replace("Value of objective function: ", "");
            objective = double.Parse(replacement,
                CultureInfo.CreateSpecificCulture("en-US"));
        }

        var solTempFile = Path.GetTempFileName();
        lp.set_outputfile(solTempFile);
        lp.print_solution(1);
        lp.set_outputfile(null);

        double[] solution;

        using (var inputFile = new StreamReader(solTempFile))
        {
            var lines = inputFile.ReadToEnd().Split('\n');
            solution = new double[lines.Length - 3];
            for (var i = 0; i < lines.Length - 3; i++)
            {
                var replacement = lines[i + 2].Replace("x" + i, "");
                replacement = replacement.Replace("C" + (i + 1), "");
                solution[i] = double.Parse(replacement,
                    CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        return new LPSolution { Objective = objective, Solution = solution };
    }

    private void Logfunc(IntPtr lp, IntPtr userhandle, string buf)
    {
        this.Log(LogLevel.Information, buf);
    }
}