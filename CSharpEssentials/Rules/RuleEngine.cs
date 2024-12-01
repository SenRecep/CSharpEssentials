using CSharpEssentials.Rules;

namespace CSharpEssentials;

public static partial class RuleEngine
{
    public static Result Evaluate<TContext>(IRuleBase<TContext> rule, TContext context, CancellationToken cancellationToken=default) => 
        InternalEvaluate(rule, context, cancellationToken);

    private static Result Evaluate<TContext>(IRule<TContext> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            return result;
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.SimpleRule, ex);
        }
    }

    private static async ValueTask<Result> Evaluate<TContext>(IAsyncRule<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.SimpleAsyncRule, ex);
        }
    }

    private static Result Evaluate<TContext>(ILinearRule<TContext> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            if (result.IsFailure)
                return result;
            if (rule.Next is null)
                return result;

            return InternalEvaluate(rule.Next, context);
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.LinearRule, ex);
        }
    }

    private static async ValueTask<Result> Evaluate<TContext>(ILinearAsyncRule<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            if (result.IsFailure)
                return result;
            if (rule.Next is null)
                return result;

            return InternalEvaluate(rule.Next, context, cancellationToken);
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.LinearAsyncRule, ex);
        }
    }

    private static Result Evaluate<TContext>(IConditionalRule<TContext> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            return result.IsFailure
                ? rule.Failure is null
                    ? result
                    : InternalEvaluate(rule.Failure, context)
                : rule.Success is null
                    ? result
                    : InternalEvaluate(rule.Success, context);
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.ConditionalRule, ex);
        }
    }

    private static async ValueTask<Result> Evaluate<TContext>(IConditionalAsyncRule<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            return result.IsFailure
                ? rule.Failure is null
                    ? result
                    : InternalEvaluate(rule.Failure, context, cancellationToken)
                : rule.Success is null
                    ? result
                    : InternalEvaluate(rule.Success, context, cancellationToken);
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.ConditionalAsyncRule, ex);
        }
    }

    private static Result Evaluate<TContext>(IAndRule<TContext> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            if (result.IsFailure)
                return result;

            return Result.And(rule.Rules.Select(r => InternalEvaluate(r, context)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.AndRule, ex);
        }
    }

    private static async ValueTask<Result> Evaluate<TContext>(IAndAsyncRule<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            if (result.IsFailure)
                return result;

            return Result.And(rule.Rules.Select(r => InternalEvaluate(r, context, cancellationToken)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.AndAsyncRule, ex);
        }
    }

    private static Result Evaluate<TContext>(IOrRule<TContext> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            if (result.IsFailure)
                return result;

            return Result.Or(rule.Rules.Select(r => InternalEvaluate(r, context)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.OrRule, ex);
        }
    }

    private static async ValueTask<Result> Evaluate<TContext>(IOrAsyncRule<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            if (result.IsFailure)
                return result;

            return Result.Or(rule.Rules.Select(r => InternalEvaluate(r, context, cancellationToken)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.OrAsyncRule, ex);
        }
    }

    private static Result InternalEvaluate<TContext>(IRuleBase<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        return rule switch
        {
            IAsyncRule<TContext> asyncRule => InternalEvaluateAsync(asyncRule, context, cancellationToken).GetAwaiter().GetResult(),
            IConditionalRule<TContext> conditionalRule => Evaluate(conditionalRule, context),
            ILinearRule<TContext> linearRule => Evaluate(linearRule, context),
            IAndRule<TContext> andRule => Evaluate(andRule, context),
            IOrRule<TContext> orRule => Evaluate(orRule, context),
            IRule<TContext> simpleRule => Evaluate(simpleRule, context),
            _ => RuleErrors.RuleEngineNotFoundError(rule.GetType().Name)
        };
    }

    private static async Task<Result> InternalEvaluateAsync<TContext>(IRuleBase<TContext> rule, TContext context, CancellationToken cancellationToken = default)
    {
        return rule switch
        {
            IConditionalAsyncRule<TContext> asyncConditionalRule => await Evaluate(asyncConditionalRule, context, cancellationToken),
            ILinearAsyncRule<TContext> asyncLinearRule => await Evaluate(asyncLinearRule, context, cancellationToken),
            IAndAsyncRule<TContext> asyncAndRule => await Evaluate(asyncAndRule, context, cancellationToken),
            IOrAsyncRule<TContext> asyncOrRule => await Evaluate(asyncOrRule, context, cancellationToken),
            IAsyncRule<TContext> asyncRule => await Evaluate(asyncRule, context, cancellationToken),
            _ => RuleErrors.RuleEngineNotFoundError(rule.GetType().Name)
        };
    }
}
