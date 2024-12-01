using CSharpEssentials.Rules;

namespace CSharpEssentials;

public static partial class RuleEngine
{
    public static Result<TResult> Evaluate<TContext, TResult>(IRuleBase<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default) =>
            InternalEvaluate(rule, context, cancellationToken);

    private static Result<TResult> Evaluate<TContext, TResult>(IRule<TContext, TResult> rule, TContext context)
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
    private static async ValueTask<Result<TResult>> Evaluate<TContext, TResult>(IAsyncRule<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
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

    private static Result<TResult> Evaluate<TContext, TResult>(ILinearRule<TContext, TResult> rule, TContext context)
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

    private static async ValueTask<Result<TResult>> Evaluate<TContext, TResult>(ILinearAsyncRule<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
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

    private static Result<TResult> Evaluate<TContext, TResult>(IConditionalRule<TContext, TResult> rule, TContext context)
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

    private static async ValueTask<Result<TResult>> Evaluate<TContext, TResult>(IConditionalAsyncRule<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
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

    private static Result<TResult> Evaluate<TContext, TResult>(IAndRule<TContext, TResult> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            if (result.IsFailure)
                return result;

            var andResult = Result<TResult>.And(rule.Rules.Select(r => InternalEvaluate(r, context)));
            if (andResult.IsFailure)
                return andResult.Errors;
            return andResult.Value[0];
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.AndRule, ex);
        }
    }

    private static async ValueTask<Result<TResult>> Evaluate<TContext, TResult>(IAndAsyncRule<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            if (result.IsFailure)
                return result;

            var andResult = Result<TResult>.And(rule.Rules.Select(r => InternalEvaluate(r, context, cancellationToken)));
            if (andResult.IsFailure)
                return andResult.Errors;
            return andResult.Value[0];
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.AndAsyncRule, ex);
        }
    }

    private static Result<TResult> Evaluate<TContext, TResult>(IOrRule<TContext, TResult> rule, TContext context)
    {
        try
        {
            var result = rule.Evaluate(context);
            if (result.IsFailure)
                return result;

            return Result<TResult>.Or(rule.Rules.Select(r => InternalEvaluate(r, context)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.OrRule, ex);
        }
    }

    private static async ValueTask<Result<TResult>> Evaluate<TContext, TResult>(IOrAsyncRule<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await rule.EvaluateAsync(context, cancellationToken);
            if (result.IsFailure)
                return result;

            return Result<TResult>.Or(rule.Rules.Select(r => InternalEvaluate(r, context, cancellationToken)));
        }
        catch (Exception ex)
        {
            return RuleErrors.RuleEngineEvaluateError(RuleTypes.OrAsyncRule, ex);
        }
    }

    private static Result<TResult> InternalEvaluate<TContext, TResult>(IRuleBase<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
    {
        return rule switch
        {
            IAsyncRule<TContext, TResult> asyncRule => InternalEvaluateAsync(asyncRule, context, cancellationToken).GetAwaiter().GetResult(),
            IConditionalRule<TContext, TResult> conditionalRule => Evaluate(conditionalRule, context),
            ILinearRule<TContext, TResult> linearRule => Evaluate(linearRule, context),
            IAndRule<TContext, TResult> andRule => Evaluate(andRule, context),
            IOrRule<TContext, TResult> orRule => Evaluate(orRule, context),
            IRule<TContext, TResult> simpleRule => Evaluate(simpleRule, context),
            _ => RuleErrors.RuleEngineNotFoundError(rule.GetType().Name)
        };
    }

    private static async Task<Result<TResult>> InternalEvaluateAsync<TContext, TResult>(IRuleBase<TContext, TResult> rule, TContext context, CancellationToken cancellationToken = default)
    {
        return rule switch
        {
            IConditionalAsyncRule<TContext, TResult> asyncConditionalRule => await Evaluate(asyncConditionalRule, context, cancellationToken),
            ILinearAsyncRule<TContext, TResult> asyncLinearRule => await Evaluate(asyncLinearRule, context, cancellationToken),
            IAndAsyncRule<TContext, TResult> asyncAndRule => await Evaluate(asyncAndRule, context, cancellationToken),
            IOrAsyncRule<TContext, TResult> asyncOrRule => await Evaluate(asyncOrRule, context, cancellationToken),
            IAsyncRule<TContext, TResult> asyncRule => await Evaluate(asyncRule, context, cancellationToken),
            _ => RuleErrors.RuleEngineNotFoundError(rule.GetType().Name)
        };
    }
}
